// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using RazorEngine.Templating;

namespace SharpDoc
{
    internal class DynamicHelper : IDynamicMetaObjectProvider
    {
        public DynamicHelper()
        {
            Methods = new Dictionary<string, List<HelperMethod>>();
        }

        private Dictionary<string, List<HelperMethod>> Methods { get; set;}

        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject( Expression parameter)
        {
            return new DynamicDictionaryMetaObject(parameter, this);
        }

        private class HelperMethod
        {
            public ITemplate Template { get; set; }

            public MethodInfo Method { get; set; }
        }

        public void RegisterHelper(ITemplate template, MethodInfo methodInfo)
        {
            string name = methodInfo.Name;
            List<HelperMethod> list;
            if (!Methods.TryGetValue(name, out list))
            {
                list = new List<HelperMethod>();
                Methods.Add(name, list);
            }
            list.Add(new HelperMethod { Method = methodInfo, Template = template } );
        }

        #region Nested type: DynamicDictionaryMetaObject

        private class DynamicDictionaryMetaObject : DynamicMetaObject
        {
            internal DynamicDictionaryMetaObject(
                Expression parameter,
                DynamicHelper value)
                : base(parameter, BindingRestrictions.Empty, value)
            {
            }

            public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
            {
                var dictionary = (DynamicHelper)Value;

                var  helperMethodInstanceList = dictionary.Methods[binder.Name];
                HelperMethod helperMethodInstance = null;

                // Find correct method
                foreach (var helperMethod in helperMethodInstanceList)
                {
                    var parameterInfos = helperMethod.Method.GetParameters();
                    if ( helperMethod.Method.GetParameters().Length == args.Length)
                    {
                        bool isMethodFound = true;
                        for (int i = 0; i < parameterInfos.Length; i++)
                        {
                            var parameterInfo = parameterInfos[i];
                            var argumentType = args[i].RuntimeType ?? parameterInfo.ParameterType;
                            if (!parameterInfo.ParameterType.IsAssignableFrom(argumentType))
                            {
                                isMethodFound = false;
                            }
                        }
                        if (isMethodFound)
                        {
                            helperMethodInstance = helperMethod;
                            break;
                        }
                    }
                }

                if (helperMethodInstance == null)
                {
                    // Todo, write parameters
                    throw new InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Unable to find target method [{0}]", binder.Name));
                }

                var methodParams = new Expression[args.Length];
                for (int i = 0; i < args.Length; i++)
                    methodParams[i] = args[i].Expression;

                var methodToString = typeof (TemplateWriter).GetMethod("ToString");

                var computeResult =
                    Expression.Call(Expression.Call(Expression.Constant(helperMethodInstance.Template), helperMethodInstance.Method, methodParams),
                                    methodToString);

                return new DynamicMetaObject(computeResult, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
            }
        }

        #endregion
    }
}