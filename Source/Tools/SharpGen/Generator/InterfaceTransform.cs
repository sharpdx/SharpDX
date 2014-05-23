// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Linq;
using System.Text.RegularExpressions;
using SharpGen.Logging;
using SharpGen.Config;
using SharpGen.CppModel;
using SharpGen.Model;

namespace SharpGen.Generator
{
    /// <summary>
    /// Transforms a C++ interface to a C# interface.
    /// </summary>
    public class InterfaceTransform : TransformBase
    {
        private readonly Dictionary<Regex, InnerInterfaceMethod> _mapMoveMethodToInnerInterface = new Dictionary<Regex, InnerInterfaceMethod>();
        private readonly CsTypeBase DefaultInterfaceCppObject = new CsInterface { Name = Global.Name + ".CppObject" };
        private readonly CsTypeBase DefaultCallbackable = new CsInterface { Name = Global.Name + ".ICallbackable" };
        private readonly CsTypeBase DefaultComObjectCallback = new CsInterface { Name = Global.Name + ".ComObjectCallback" };

        /// <summary>
        /// Moves the methods to an inner C# interface.
        /// </summary>
        /// <param name="methodNameRegExp">The method name regexp query.</param>
        /// <param name="innerInterface">The C# inner interface.</param>
        /// <param name="propertyNameAccess">The name of the property to access the inner interface.</param>
        /// <param name="inheritedInterfaceName">Name of the inherited interface.</param>
        public void MoveMethodsToInnerInterface(string methodNameRegExp, string innerInterface, string propertyNameAccess,
                                                 string inheritedInterfaceName = null)
        {
            _mapMoveMethodToInnerInterface.Add(new Regex("^" + methodNameRegExp + "$"),
                                               new InnerInterfaceMethod(innerInterface, propertyNameAccess,
                                                                        inheritedInterfaceName));
        }

        /// <summary>
        /// Gets the method transformer.
        /// </summary>
        /// <value>The method transformer.</value>
        private MethodTransform MethodTranform { get { return Manager.MethodTransform; } }

        /// <summary>
        /// Prepares the specified C++ element to a C# element.
        /// </summary>
        /// <param name="cppElement">The C++ element.</param>
        /// <returns>The C# element created and registered to the <see cref="TransformManager"/></returns>
        public override CsBase Prepare(CppElement cppElement)
        {
            var cppInterface = (CppInterface) cppElement;

            // IsFullyMapped to false => The structure is being mapped
            var cSharpInterface = new CsInterface(cppInterface) {IsFullyMapped = false};
            CsNamespace nameSpace = Manager.ResolveNamespace(cppInterface);
            cSharpInterface.Name = NamingRules.Rename(cppInterface);
            nameSpace.Add(cSharpInterface);

            Manager.BindType(cppInterface.Name, cSharpInterface);

            return cSharpInterface;
        }

        /// <summary>
        /// Processes the specified C# element to complete the mapping process between the C++ and C# element.
        /// </summary>
        /// <param name="csElement">The C# element.</param>
        public override void Process(CsBase csElement)
        {
            Process((CsInterface) csElement);
        }

        /// <summary>
        /// Processes the specified interface type.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        public void Process(CsInterface interfaceType )
        {
            if (interfaceType.IsFullyMapped)
                return;

            // Set IsFullyMapped to avoid recursive mapping
            interfaceType.IsFullyMapped = true;

            // var cppInterface = interfaceType.CppElement as CppInterface;
            var cppInterface = (CppInterface)interfaceType.CppElement;

            // Associate Parent
            var parentType = Manager.FindBindType(cppInterface.ParentName);
            if (parentType != null)
            {
                interfaceType.Base = parentType;

                // Process base if it's not mapped already
                if (!parentType.IsFullyMapped)
                    Process(parentType);
            }
            else
            {
                if (!interfaceType.IsCallback)
                    interfaceType.Base = DefaultInterfaceCppObject;
            }

            // Warning, if Guid is null we need to recover it from a declared GUID
            if (string.IsNullOrEmpty(cppInterface.Guid))
            {
                // Go up to the root base interface
                var rootBase = parentType;
                while (rootBase != null && rootBase is CsInterface && ((CsInterface)rootBase).Base != null)
                    rootBase = ((CsInterface) rootBase).Base;

                // look for GUID only for ComObjects
                var cppGuid = cppInterface.ParentInclude.Find<CppGuid>("^IID_" + cppInterface.Name + "$").FirstOrDefault();
                if (cppGuid == null)
                {
                    // If Guid == null && BaseRoot != null && BaseRoot is a ComObject
                    // then we probably missed a guid
                    if (rootBase != null && rootBase.QualifiedName == Global.Name + ".ComObject")
                        Logger.Warning("cannot find GUID");
                } 
                else
                    interfaceType.Guid = cppGuid.Guid.ToString();
            }

            // Handle Methods
            var generatedMethods = new List<CsMethod>();
            var intPtrType = Manager.ImportType(typeof(IntPtr));
            foreach (var cppMethod in cppInterface.Methods)
            {
                var cSharpMethod = (CsMethod)MethodTranform.Prepare(cppMethod);
                generatedMethods.Add(cSharpMethod);
                interfaceType.Add(cSharpMethod);

                MethodTranform.Process(cSharpMethod);

                // Add specialized method for ComArray
                DuplicateMethodSpecial(interfaceType, cSharpMethod, intPtrType);

                //MapMethod(cSharpMethod);
                //RegisterNativeInterop(cSharpMethod);

				// Allow overloads by changing the name to <name>#
				string origCppName = cppInterface.Name + "::" + cppMethod.Name;
				string chosenCppName = origCppName;
				for (int i = 0; Manager.FindBindType(chosenCppName) != null; i++)
					chosenCppName = origCppName + i.ToString();
					
                Manager.BindType(chosenCppName, cSharpMethod);
            }

            // Dispatch method to inner interface if any
            var mapInnerInterface = new Dictionary<string, CsInterface>();

            // Make a copy of the methods
            var methods = interfaceType.Methods.ToList();
            foreach (var csMethod in methods)
            {
                string cppName = interfaceType.CppElementName + "::" + csMethod.CppElement.Name;
                foreach (var keyValuePair in _mapMoveMethodToInnerInterface)
                {
                    if (keyValuePair.Key.Match(cppName).Success)
                    {
                        string innerInterfaceName = keyValuePair.Value.InnerInterface;
                        string parentInterfaceName = keyValuePair.Value.InheritedInterfaceName;

                        CsInterface innerCsInterface;
                        CsInterface parentCsInterface = null;

                        if (parentInterfaceName != null)
                        {
                            if (!mapInnerInterface.TryGetValue(parentInterfaceName, out parentCsInterface))
                            {
                                parentCsInterface = new CsInterface(null) { Name = parentInterfaceName };
                                mapInnerInterface.Add(parentInterfaceName, parentCsInterface);
                            }
                        }

                        if (!mapInnerInterface.TryGetValue(innerInterfaceName, out innerCsInterface))
                        {
                            // TODO custom cppInterface?
                            innerCsInterface = new CsInterface(cppInterface)
                                { Name = innerInterfaceName, PropertyAccesName = keyValuePair.Value.PropertyAccessName, Base = parentCsInterface ?? DefaultInterfaceCppObject };

                            // Add inner interface to root interface
                            interfaceType.Add(innerCsInterface);
                            interfaceType.Parent.Add(innerCsInterface);

                            // Move method to inner interface
                            mapInnerInterface.Add(innerInterfaceName, innerCsInterface);
                        }

                        interfaceType.Remove(csMethod);
                        innerCsInterface.Add(csMethod);
                        break;
                    }
                }
            }

            // Remove dispatched methods from outer interface
            //foreach (var innerInterface in mapInnerInterface)
            //{
            //    foreach (var method in innerInterface.Value.Methods)
            //        cppInterface.Remove(method.CppElement);
            //}

            // If interfaceType is DualCallback, then need to generate a default implem
            if (interfaceType.IsDualCallback)
            {
                var tagForInterface = cppInterface.GetTagOrDefault<MappingRule>();
                var nativeCallback = new CsInterface(interfaceType.CppElement as CppInterface)
                                         {
                                             Name = interfaceType.Name + "Native",
                                             Visibility = Visibility.Internal
                                         };

                // Update nativeCallback from tag
                if (tagForInterface != null)
                {
                    if (tagForInterface.NativeCallbackVisibility.HasValue)
                        nativeCallback.Visibility = tagForInterface.NativeCallbackVisibility.Value;
                    if (tagForInterface.NativeCallbackName != null)
                        nativeCallback.Name = tagForInterface.NativeCallbackName;
                }

                nativeCallback.Base = interfaceType.Base;

                if (nativeCallback.IsBaseComObject)
                {
                    nativeCallback.Base = DefaultComObjectCallback;
                }

                // If Parent is a DualInterface, then inherit from Default Callback
                if (interfaceType.Base is CsInterface)
                {
                    var parentInterface = interfaceType.Base as CsInterface;
                    if (parentInterface.IsDualCallback)
                        nativeCallback.Base = parentInterface.NativeImplem;
                }

                nativeCallback.IBase = interfaceType;
                interfaceType.NativeImplem = nativeCallback;

                foreach (var innerElement in interfaceType.Items)
                {
                    if (innerElement is CsMethod)
                    {
                        var method = (CsMethod)innerElement;
                        var newCsMethod = (CsMethod)method.Clone();
                        var tagForMethod = method.CppElement.GetTagOrDefault<MappingRule>();
                        bool keepMethodPublic = tagForMethod.IsKeepImplementPublic.HasValue && tagForMethod.IsKeepImplementPublic.Value;
                        if (!keepMethodPublic)
                        {
                            newCsMethod.Visibility = Visibility.Internal;
                            newCsMethod.Name = newCsMethod.Name + "_";
                        }
                        nativeCallback.Add(newCsMethod);
                    }
                    else
                    {
                        Logger.Warning("Unhandled innerElement {0} for DualCallbackInterface {1}", innerElement, interfaceType.Name);
                    }
                }
                nativeCallback.IsCallback = false;
                nativeCallback.IsDualCallback = true;
                interfaceType.Parent.Add(nativeCallback);
            }
            else
            {
                // If interface is a callback and parent is ComObject, then remove it
                var parentInterface = interfaceType.Base as CsInterface;
                if (parentInterface != null && parentInterface.IsDualCallback)
                {
                    interfaceType.Base = parentInterface.NativeImplem;
                }
                else
                {
                    // Refactor Properties
                    CreateProperties(generatedMethods);
                }
            }

            // If interface is a callback and parent is ComObject, then remove it
            if (interfaceType.IsCallback )
            {
                if (interfaceType.IsBaseComObject)
                    interfaceType.Base = null;
                if (interfaceType.Base == null)
                    interfaceType.Base = DefaultCallbackable;
            }
        }

        private static void DuplicateMethodSpecial(CsInterface interfaceType, CsMethod csMethod, CsTypeBase intPtrType)
        {
            bool hasComArrayLike = false;
            foreach(var csParameter in csMethod.Parameters)
            {
                if(csParameter.IsInComArrayLike)
                {
                    hasComArrayLike = true;
                    break;
                }
            }

            // Look for at least one parameter ComArray candidate
            if (hasComArrayLike)
            {
                // Create a new method and transforms all array of ComObject to ComArray<ComObject>
                var newMethod = (CsMethod)csMethod.Clone();
                foreach (var csSubParameter in newMethod.Parameters)
                {
                    if (csSubParameter.IsInComArrayLike)
                        csSubParameter.PublicType = new CsComArray((CsInterface)csSubParameter.PublicType);
                }
                interfaceType.Add(newMethod);
            }

            if(hasComArrayLike || csMethod.RequestRawPtr)
            {
                // Create private method with raw pointers for arrays, with all arrays as pure IntPtr
                // In order to be able to generate method taking single element
                var rawMethod = (CsMethod)csMethod.Clone();
                rawMethod.Visibility = Visibility.Private;
                foreach(var csSubParameter in rawMethod.Parameters)
                {
                    if(csSubParameter.IsArray || csSubParameter.IsComObject || csSubParameter.HasPointer)
                    {
                        csSubParameter.PublicType = intPtrType;
                        csSubParameter.IsArray = false;
                    }
                }
                interfaceType.Add(rawMethod);
            }
        }

        private static readonly Regex MatchGet = new Regex(@"^\s*(\<[Pp]\>)?\s*(Gets?|Retrieves?|Returns)");

        /// <summary>
        /// Creates C# properties for method that respect the following convention:
        /// TODO describe the convention to create properties from methods here.
        /// </summary>
        /// <param name="methods">The methods.</param>
        private static void CreateProperties(IEnumerable<CsMethod> methods)
        {
            var cSharpProperties = new Dictionary<string, CsProperty>();

            foreach (var cSharpMethod in methods)
            {
                bool isIs = cSharpMethod.Name.StartsWith("Is");
                bool isGet = cSharpMethod.Name.StartsWith("Get") || isIs;
                bool isSet = cSharpMethod.Name.StartsWith("Set");
                if (!(isGet || isSet))
                    continue;
                string propertyName = isIs ? cSharpMethod.Name : cSharpMethod.Name.Substring("Get".Length);

                int parameterCount = cSharpMethod.ParameterCount;
                var parameterList = cSharpMethod.Parameters;

                CsProperty csProperty;
                bool isPropertyToAdd = false;

                if (!cSharpProperties.TryGetValue(propertyName, out csProperty))
                {
                    csProperty = new CsProperty(propertyName);
                    isPropertyToAdd = true;
                }

                // If the property has already a getter and a setter, this must be an error, remove the property
                // (Should never happen, unless there are some polymorphism on the interface's methods)
                if (csProperty.Getter != null && csProperty.Setter != null)
                {
                    cSharpProperties.Remove(propertyName);
                    continue;
                }

                // Check Getter
                if (isGet)
                {
                    if ((cSharpMethod.IsHResult || !cSharpMethod.HasReturnType) && parameterCount == 1 &&
                        parameterList[0].IsOut && !parameterList[0].IsArray)
                    {
                        csProperty.Getter = cSharpMethod;
                        csProperty.PublicType = parameterList[0].PublicType;
                        csProperty.IsPropertyParam = true;
                    }
                    else if (parameterCount == 0 && cSharpMethod.HasReturnType)
                    {
                        csProperty.Getter = cSharpMethod;
                        csProperty.PublicType = csProperty.Getter.ReturnType.PublicType;
                    }
                    else
                    {
                        // If there is a getter, but the setter is not valid, then remove the getter
                        if (csProperty.Setter != null)
                            cSharpProperties.Remove(propertyName);
                        continue;
                    }
                }
                else
                {
                    // Check Setter
                    if ((cSharpMethod.IsHResult || !cSharpMethod.HasReturnType) && parameterCount == 1 &&
                        (parameterList[0].IsRefIn || parameterList[0].IsIn || parameterList[0].IsRef) && !parameterList[0].IsArray)
                    {
                        csProperty.Setter = cSharpMethod;
                        csProperty.PublicType = parameterList[0].PublicType;
                    }
                    else if (parameterCount == 1 && !cSharpMethod.HasReturnType)
                    {
                        csProperty.Setter = cSharpMethod;
                        csProperty.PublicType = csProperty.Setter.ReturnType.PublicType;
                    }
                    else
                    {
                        // If there is a getter, but the setter is not valid, then remove the getter
                        if (csProperty.Getter != null)
                            cSharpProperties.Remove(propertyName);
                        continue;
                    }
                }

                // Check when Setter and Getter together that they have the same return type
                if (csProperty.Setter != null && csProperty.Getter != null)
                {
                    bool removeProperty = false;

                    //// Don't add property that doesn't match with return type
                    //if (property.Setter != property.Getter.IsHResult)
                    //    continue;
                    if (csProperty.IsPropertyParam)
                    {
                        var getterParameter = csProperty.Getter.Parameters.First();
                        var setterParameter = csProperty.Setter.Parameters.First();
                        if (getterParameter.PublicType.QualifiedName != setterParameter.PublicType.QualifiedName)
                        {
                            removeProperty = true;
                        }
                    }
                    else
                    {
                        var getterType = csProperty.Getter.ReturnType;
                        var setterType = csProperty.Setter.Parameters.First();
                        if (getterType.PublicType.QualifiedName != setterType.PublicType.QualifiedName)
                            removeProperty = true;
                    }
                    if (removeProperty)
                    {
                        cSharpProperties.Remove(propertyName);
                    }
                }

                if (isPropertyToAdd)
                    cSharpProperties.Add(propertyName, csProperty);
            }

            // Add the property to the parentContainer
            foreach (var cSharpProperty in cSharpProperties)
            {
                var property = cSharpProperty.Value;

                var getterOrSetter = property.Getter ?? property.Setter;

                // Associate the property with the Getter element
                property.CppElement = getterOrSetter.CppElement;

                // If We have a getter, then we need to modify the documentation in order to print that we have Gets and Sets.
                if (property.Getter != null && property.Setter != null && !string.IsNullOrEmpty(property.Description))
                {
                    property.Description = MatchGet.Replace(property.Description, "$1$2 or sets");
                }

                var parent = getterOrSetter.Parent;

                // If Getter has no property, 
                if ((property.Getter != null && !property.Getter.AllowProperty) || (property.Setter != null && !property.Setter.AllowProperty))
                    continue;

                // Update visibility for getter and setter (set to internal)
                if (property.Getter != null)
                {
                    property.Getter.Visibility = Visibility.Internal;
                    property.IsPersistent = property.Getter.IsPersistent;
                    if (property.IsPersistent)
                        parent.HasPersistent = true;
                }

                if (property.Setter != null)
                    property.Setter.Visibility = Visibility.Internal;

                if (property.Getter != null && property.Name.StartsWith("Is"))
                    property.Getter.Name = property.Getter.Name + "_";

                parent.Add(property);
            }
        }

        /// <summary>
        /// Private class used for inner interface method creation.
        /// </summary>
        private class InnerInterfaceMethod
        {
            public readonly string InnerInterface;
            public readonly string PropertyAccessName;
            public readonly string InheritedInterfaceName;

            public InnerInterfaceMethod(string innerInterface, string propertyAccess, string inheritedInterfaceName)
            {
                InnerInterface = innerInterface;
                PropertyAccessName = propertyAccess;
                InheritedInterfaceName = inheritedInterfaceName;
            }
        }
    }
}