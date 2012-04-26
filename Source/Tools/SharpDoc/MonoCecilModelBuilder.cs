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
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;
using SharpCore.Logging;
using SharpDoc.Model;

namespace SharpDoc
{
    /// <summary>
    /// Mono.Cecil implementation of a <see cref="IModelBuilder"/>.
    /// </summary>
    internal class MonoCecilModelBuilder : IModelBuilder
    {
        private NAssemblySource _source;
        private MemberRegistry _registry;
        private const string AssemblyDocClass = "AssemblyDoc";
        private const string NamespaceDocClass = "NamespaceDoc";
        private const string MethodOperatorPrefix = "op_";

        /// <summary>
        /// Loads from an assembly source definition all types to document.
        /// </summary>
        /// <param name="assemblySource">The assembly source definition.</param>
        /// <param name="memberRegistry">The member registry to populate with types.</param>
        /// <returns>
        /// An assembly documentator that contains all documented types, methods.
        /// </returns>
        public NAssembly LoadFrom(NAssemblySource assemblySource, MemberRegistry memberRegistry)
        {
            _source = assemblySource;
            _registry = memberRegistry;

            var assemblyDefinition = (AssemblyDefinition)assemblySource.Assembly;

            var assembly = new NAssembly {Name = assemblyDefinition.Name.Name};
            assembly.FullName = assembly.Name;
            assembly.Id = "A:" + assembly.Name;
            assembly.NormalizedId = DocIdHelper.StripXmlId(assembly.Id);
            assembly.Version = assemblyDefinition.Name.Version.ToString();
            assembly.FileName = Path.GetFileName(assemblySource.Filename);
            _registry.Register(assembly, assembly);

            Logger.Message("Processing assembly [{0}]", assembly.FullName);

            // Apply documentation from AssemblyDoc special class
            assembly.DocNode = _source.FindMemberDoc("T:" + assembly.Name + "." + AssemblyDocClass);

            var namespaces = new Dictionary<string, NNamespace>();
            foreach (var module in assemblyDefinition.Modules)
            {
                foreach (var type in module.Types)
                {
                    NNamespace parentNamespace;

                    // Skip empty namespaces and special <Module>
                    if (string.IsNullOrEmpty(type.Namespace) || type.Namespace.StartsWith("<"))
                        continue;

                    // Create naemespace
                    if (!namespaces.TryGetValue(type.Namespace, out parentNamespace))
                    {
                        parentNamespace = AddNamespace(assembly, type.Namespace);
                        namespaces.Add(type.Namespace, parentNamespace);
                    }

                    AddType(parentNamespace, type);
                }
            }

            // Remove empty namespaces
            foreach (var removeNamespace in namespaces.Values)
            {
                if (removeNamespace.Types.Count == 0)
                    assembly.Namespaces.Remove(removeNamespace);
            }

            // Sort namespace in alphabetical order
            var namespaceNames = new List<string>(namespaces.Keys);
            namespaceNames.Sort();

            foreach (var namespaceName in namespaceNames)
                namespaces[namespaceName].Types.Sort((left, right) => left.NormalizedId.CompareTo(right.NormalizedId));

            return assembly;
        }

        /// <summary>
        /// Adds the namespace.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private NNamespace AddNamespace(NAssembly assembly, string name)
        {
            var @namespace = new NNamespace(name) {Assembly = assembly, Id = "N:" + name};
            @namespace.FullName = @namespace.Name;
            @namespace.NormalizedId = DocIdHelper.StripXmlId(@namespace.Id);      
            _registry.Register(assembly, @namespace);

            // Apply documentation on namespace from NamespaceDoc special class
            @namespace.DocNode = _source.FindMemberDoc("T:" + name + "." + NamespaceDocClass);

            // Add See Alsos
            @namespace.SeeAlsos.Add( new NSeeAlso(@namespace.Assembly) );

            assembly.Namespaces.Add(@namespace);
            return @namespace;
        }

        /// <summary>
        /// Adds the type.
        /// </summary>
        /// <param name="namespace">The @namespace.</param>
        /// <param name="typeDef">The type def.</param>
        private void AddType(NNamespace @namespace, TypeDefinition typeDef)
        {
            // Todo add configurable filter
            if (!typeDef.IsPublic)
                return;

            NType type;
            if (typeDef.IsInterface)
            {
                type = NewInstance<NInterface>(typeDef);
                type.MemberType = NMemberType.Interface;
            }
            else if (typeDef.IsEnum)
            {
                type = NewInstance<NEnum>(typeDef);
                type.MemberType = NMemberType.Enum;
            }
            else if (typeDef.IsValueType)
            {
                type = NewInstance<NStruct>(typeDef);
                type.MemberType = NMemberType.Struct;
            }
            else if (MonoCecilHelper.IsDelegate(typeDef))
            {
                type = NewInstance<NDelegate>(typeDef);
                type.MemberType = NMemberType.Delegate;
            }
            else if (typeDef.IsClass)
            {
                type = NewInstance<NClass>(typeDef);
                type.MemberType = NMemberType.Class;
            }
            else
            {
                type = null;
                type.MemberType = NMemberType.Delegate;
            }
            _registry.Register(@namespace.Assembly, type);
            @namespace.AddType(type);
            
            // Add reference to all base types
            var baseTypeDefinition = typeDef;
            do
            {
                baseTypeDefinition = MonoCecilHelper.GetBaseType(baseTypeDefinition);
                if (baseTypeDefinition != null)
                    type.Bases.Add(GetTypeReference(baseTypeDefinition));
            } while (baseTypeDefinition != null);

            // Flattened hierarchy initialized with all base types
            // Derived types will be added at a later pass, when types in all asemblies 
            // will be discovered.
            var reverseBases = new List<INMemberReference>(type.Bases);
            reverseBases.Reverse();
            for (int i = 0; i < reverseBases.Count; i++)
                type.FlattenedHierarchy.Add(new Tuple<int, INMemberReference>(i, reverseBases[i]));
            // Add this type itself to its flattened hierarchy
            type.FlattenedHierarchy.Add(new Tuple<int, INMemberReference>(reverseBases.Count, type));

            // Add reference to inherited interfaces
            foreach (var typeReference in typeDef.Interfaces)
                type.Interfaces.Add(GetTypeReference(typeReference));

            if (typeDef.IsPublic)
                type.Visibility = NVisibility.Public;
            else if (typeDef.IsNotPublic)
                type.Visibility = NVisibility.Private;
            type.IsAbstract = typeDef.IsAbstract;
            type.IsFinal = typeDef.IsSealed;

            // Reconstruct StructLayout attribute
            if (typeDef.IsExplicitLayout || typeDef.IsSequentialLayout)
            {
                var layout = new StringBuilder();
                layout.Append("StructLayoutAttribute(");
                if (typeDef.IsExplicitLayout)
                    layout.Append("LayoutKind.Explicit");
                else if (typeDef.IsSequentialLayout)
                    layout.Append("LayoutKind.Sequential");
                else
                    layout.Append("LayoutKind.Auto");

                if (typeDef.IsUnicodeClass)
                    layout.Append(", CharSet = CharSet.Unicode");
                else if (typeDef.IsAnsiClass)
                    layout.Append(", CharSet = CharSet.Ansi");
                else if (typeDef.IsAutoClass)
                    layout.Append(", CharSet = CharSet.Auto");

                if (typeDef.PackingSize != 0)
                    layout.Append(", Pack = ").Append(typeDef.PackingSize);

                if (typeDef.ClassSize != 0)
                    layout.Append(", Size = ").Append(typeDef.ClassSize);

                layout.Append(")");
                type.Attributes.Add(layout.ToString());
            }

            // Reconstruct Serializable attribute
            if (typeDef.IsSerializable)
                type.Attributes.Add("SerializableAttribute");

            //else if (typeDef.IsAssembly)
            //   type.Visibility = NVisibility.Internal;
            //else if (typeDef.IsFamily)
            //    type.Visibility = NVisibility.Protected;
            //else if (typeDef.IsFamilyOrAssembly)
            //    type.Visibility = NVisibility.ProtectedInternal;

            // Add methods, constructors, operators. Todo add configurable filter
            foreach (var method in typeDef.Methods.Where(method => method.IsPublic || method.IsFamilyOrAssembly))
                AddMethod(type, method);

            // Add fields. Todo add configurable filter
            foreach (var field in typeDef.Fields.Where(field => field.IsPublic || field.IsFamilyOrAssembly))
                AddField(type, field);

            // Add events. Todo add configurable filter
            foreach (var eventInfo in typeDef.Events.Where(eventInfo => eventInfo.AddMethod.IsPublic || eventInfo.AddMethod.IsFamilyOrAssembly))
                AddEvent(type, eventInfo);

            // Add properties Todo add configurable filter
            foreach (var property in typeDef.Properties.Where(property => (property.GetMethod != null && (property.GetMethod.IsPublic || property.GetMethod.IsFamilyOrAssembly)) || (property.SetMethod != null && (property.SetMethod.IsPublic || property.SetMethod.IsFamilyOrAssembly))))
                AddProperty(type, property);

            // Recalculate a NormalizedId based on the number of overriding methods.
            var counters = new Dictionary<string, int>();
            foreach (var member in type.Members)
            {
                string id =  DocIdHelper.StripXmlId(member.Id);

                if (!counters.ContainsKey(id))
                    counters.Add(id, 0);
                else
                {
                    counters[id]++;
                    id = id + "_" + counters[id];
                }

                member.NormalizedId = id;
            }

            // Tag methods that are overriden
            foreach (var method in type.MethodsAndConstructors)
            {
                var id = DocIdHelper.StripXmlId(method.Id);
                if (counters.ContainsKey(id) && counters[id] > 0)
                    method.HasOverrides = true;
            }

            // Sort members
            type.Members.Sort((left, right) => left.Name.CompareTo(right.Name));

            type.SeeAlsos.Add(new NSeeAlso(@namespace));
            type.SeeAlsos.Add(new NSeeAlso(@namespace.Assembly));

            // Add nested types to the global namespace.
            foreach (var nestedType in typeDef.NestedTypes)
                AddType(@namespace, nestedType);
        }

        /// <summary>
        /// Creates a method from a method definition.
        /// </summary>
        /// <param name="methodDef">The method def.</param>
        /// <returns>A NMethod</returns>
        private NMethod CreateMethodFromDefinition(MethodDefinition methodDef)
        {
            NMethod method;

            if (methodDef.IsConstructor)
            {
                method = NewInstance<NConstructor>(methodDef);
                // Constructors must have typename instead of .ctor
                method.Name = method.DeclaringType.Name;
            }
            else if (methodDef.IsSpecialName && methodDef.Name.StartsWith(MethodOperatorPrefix))
            {
                method = NewInstance<NOperator>(methodDef);
                method.Name = method.Name.Substring(MethodOperatorPrefix.Length, method.Name.Length - MethodOperatorPrefix.Length);
            }
            else
            {
                method = NewInstance<NMethod>(methodDef);

                //// Methods with generic parameters, override the name by adding the generics
                //if (method.GenericParameters.Count > 0)
                //{
                //    var newMethodName = new StringBuilder(method.Name);
                //    BuildMethodSignatureGenericParameters(method, newMethodName);
                //    method.Name = newMethodName.ToString();
                //}
            }

            method.IsVirtual = methodDef.IsVirtual;
            method.IsStatic = methodDef.IsStatic;
            method.IsFinal = methodDef.IsFinal;
            method.IsAbstract = methodDef.IsAbstract;

            if (methodDef.IsPublic)
                method.Visibility = NVisibility.Public;
            else if (methodDef.IsPrivate)
                method.Visibility = NVisibility.Private;
            else if (methodDef.IsAssembly)
                method.Visibility = NVisibility.Internal;
            else if (methodDef.IsFamily)
                method.Visibility = NVisibility.Protected;
            else if (methodDef.IsFamilyOrAssembly)
                method.Visibility = NVisibility.ProtectedInternal;

            if (method.IsVirtual)
            {
                // overrides
                method.Overrides = GetMethodReference(MonoCecilHelper.GetBaseMethodInTypeHierarchy(methodDef));

                // implements
                method.Implements = GetMethodReference(MonoCecilHelper.GetBaseMethodInInterfaceHierarchy(methodDef));
            }

            method.ReturnType = GetTypeReference(methodDef.ReturnType);
            method.MemberType = NMemberType.Method;

            foreach (var parameter in methodDef.Parameters)
                AddParameter(method, parameter);

            // Display name for this method
            var signature = new StringBuilder();
            signature.Append(method.Name);
            BuildMethodSignatureParameters(method, signature);
            method.Signature = signature.ToString();

            // Force update of documentation for parameters
            method.OnDocNodeUpdate();

            return method;
        }

        /// <summary>
        /// Adds the method.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="methodDef">The method def.</param>
        /// <param name="isProperty">if set to <c>true</c> [is property].</param>
        /// <returns></returns>
        private NMethod AddMethod(NMember parent, MethodDefinition methodDef, bool isProperty = false)
        {
            // Don't add getter and setters for properties and events
            if (methodDef == null || (!isProperty && methodDef.IsSpecialName && (methodDef.IsGetter || methodDef.IsSetter || methodDef.IsAddOn || methodDef.IsRemoveOn)))
                return null;

            var method = CreateMethodFromDefinition(methodDef);

            // If not a get/set then handle it
            if (!isProperty)
            {
                _registry.Register(parent.Namespace.Assembly, method);
                parent.AddMember(method);

                var parentType = parent as NType;
                if (parentType != null)
                {
                    if (method is NConstructor)
                    {
                        method.Name = parent.Name;
                        parentType.HasConstructors = true;
                    }
                    else if (method is NOperator)
                    {
                        parentType.HasOperators = true;
                    }
                    else
                    {
                        parentType.HasMethods = true;
                    }
                }
            }

            // Add SeeAlso
            method.SeeAlsos.Add(new NSeeAlso(parent));
            method.SeeAlsos.Add(new NSeeAlso(parent.Namespace));
            method.SeeAlsos.Add(new NSeeAlso(parent.Namespace.Assembly));

            return method;
        }

        private static string ReplacePrimitive(string name, string fullName)
        {
            if (fullName == typeof(string).FullName)
                return "string";
            if (fullName == typeof(int).FullName)
                return "int";
            if (fullName == typeof(uint).FullName)
                return "uint";
            if (fullName == typeof(short).FullName)
                return "short";
            if (fullName == typeof(ushort).FullName)
                return "ushort";
            if (fullName == typeof(long).FullName)
                return "long";
            if (fullName == typeof(ulong).FullName)
                return "ulong";
            if (fullName == typeof(bool).FullName)
                return "bool";
            if (fullName == typeof(void).FullName)
                return "void";
            if (fullName == typeof(float).FullName)
                return "float";
            if (fullName == typeof(double).FullName)
                return "double";
            return name;
        }

        private void FillTypeReference(INMemberReference typeReference, TypeReference typeDef)
        {
            typeReference.Id = DocIdHelper.GetXmlId(typeDef);
            typeReference.NormalizedId = DocIdHelper.StripXmlId(typeReference.Id);
            typeReference.Name = ReplacePrimitive(typeDef.Name, typeDef.FullName);
            typeReference.FullName = typeDef.FullName;
            typeReference.IsGenericInstance = typeDef.IsGenericInstance;
            typeReference.IsGenericParameter = typeDef.IsGenericParameter;
            typeReference.IsArray = typeDef.IsArray;
            typeReference.IsPointer = typeDef.IsPointer;
            typeReference.IsSentinel = typeDef.IsSentinel;
            FillGenericParameters(typeReference, typeDef);

            // Handle generic instance
            var genericInstanceDef = typeDef as GenericInstanceType;

            if (genericInstanceDef != null )
            {
                typeReference.ElementType = GetTypeReference(genericInstanceDef.ElementType);

                if (genericInstanceDef.GenericArguments.Count > 0)
                {
                    foreach (var genericArgumentDef in genericInstanceDef.GenericArguments)
                    {
                        var genericArgument = new NTypeReference();
                        FillTypeReference(genericArgument, genericArgumentDef);
                        typeReference.GenericArguments.Add(genericArgument);
                    }
                    // Remove `number from Name
                    typeReference.Name = BuildGenericName(typeDef.Name, typeReference.GenericArguments);
                    typeReference.FullName = BuildGenericName(typeDef.FullName, typeReference.GenericArguments);
                }
            } else if (typeReference.GenericParameters.Count > 0)
            {
                // If generic parameters, than rewrite the name/fullname
                typeReference.Name = BuildGenericName(typeDef.Name, typeReference.GenericParameters);
                typeReference.FullName = BuildGenericName(typeDef.FullName, typeReference.GenericParameters);
            }
        }

        private NTypeReference GetTypeReference(TypeReference typeDef)
        {
            if (typeDef == null)
                return null;

            var typeReference = new NTypeReference();
            FillTypeReference(typeReference, typeDef);
            return typeReference;
        }

        private INMemberReference GetMethodReference(MethodDefinition methodDef)
        {
            if (methodDef == null)
                return null;
            return CreateMethodFromDefinition(methodDef);
        }


        /// <summary>
        /// Builds the method signature parameters.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="builder">The builder.</param>
        private static void BuildMethodSignatureGenericParameters(NMethod self, StringBuilder builder)
        {
            if (self.GenericParameters.Count > 0)
            {
                builder.Append("<");
                for (int i = 0; i < self.GenericParameters.Count; i++)
                {
                    var genericParameter = self.GenericParameters[i];
                    if (i > 0)
                        builder.Append(", ");
                    builder.Append(genericParameter.Name);
                }
                builder.Append(">");
            }
        }

        /// <summary>
        /// Builds the method signature parameters.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="builder">The builder.</param>
        private static void BuildMethodSignatureParameters(NMethod self, StringBuilder builder)
        {
            // BuildMethodSignatureGenericParameters(self, builder);

            builder.Append("(");
            var parameters = self.Parameters;
            for (int i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                if (i > 0)
                    builder.Append(", ");                    
                if (parameter.ParameterType.IsSentinel)
                {
                    builder.Append("..., ");
                }
                builder.Append(parameter.ParameterTypeName);
                if (parameter.ParameterType.IsPointer)
                    builder.Append("*");
            }
            builder.Append(")");
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameterDef">The parameter def.</param>
        private void AddParameter(NMethod method, ParameterDefinition parameterDef)
        {
            var parameter = new NParameter {Name = parameterDef.Name};
            parameter.FullName = parameter.Name;
            parameter.IsIn = parameterDef.IsIn;
            parameter.IsOut = parameterDef.IsOut;
            parameter.IsOptional = parameterDef.IsOptional;
            parameter.IsReturnValue = parameterDef.IsReturnValue;
            parameter.ParameterType = GetTypeReference(parameterDef.ParameterType);
            method.AddParameter(parameter);
        }

        /// <summary>
        /// Adds the event.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="eventDef">The event def.</param>
        private void AddEvent(NType parent, EventDefinition eventDef)
        {
            var @event = NewInstance<NEvent>(eventDef);
            _registry.Register(parent.Namespace.Assembly, @event);
            @event.MemberType = NMemberType.Event;
            parent.AddMember(@event);
            parent.HasEvents = true;
        }

        /// <summary>
        /// Adds the field.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="fieldDef">The field def.</param>
        private void AddField(NType parent, FieldDefinition fieldDef)
        {
            if (fieldDef.IsSpecialName)
                return;
            var field = NewInstance<NField>(fieldDef);
            _registry.Register(parent.Namespace.Assembly, field);
            field.MemberType = NMemberType.Field;
            field.FieldType = GetTypeReference(fieldDef.FieldType);

            if (fieldDef.Constant != null )
                field.ConstantValue = GetTextFromValue(fieldDef.Constant);

            parent.AddMember(field);
            parent.HasFields = true;

            field.SeeAlsos.Add(new NSeeAlso(parent));
            field.SeeAlsos.Add(new NSeeAlso(parent.Namespace));
            field.SeeAlsos.Add(new NSeeAlso(parent.Namespace.Assembly));
        }

        /// <summary>
        /// Adds the property.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="propertyDef">The property def.</param>
        private void AddProperty(NType parent, PropertyDefinition propertyDef)
        {
            var property = NewInstance<NProperty>(propertyDef);
            _registry.Register(parent.Namespace.Assembly, property);
            property.Namespace = parent.Namespace;

            property.MemberType = NMemberType.Property;
            property.PropertyType = GetTypeReference(propertyDef.PropertyType);
            property.GetMethod = AddMethod(property, propertyDef.GetMethod, true);
            property.SetMethod = AddMethod(property, propertyDef.SetMethod, true);

            parent.AddMember(property);
            parent.HasProperties = true;

            property.SeeAlsos.Add(new NSeeAlso(parent));
            property.SeeAlsos.Add(new NSeeAlso(parent.Namespace));
            property.SeeAlsos.Add(new NSeeAlso(parent.Namespace.Assembly));
        }

        /// <summary>
        /// New instance of a <see cref="IModelReference"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberRef">The type def.</param>
        /// <returns></returns>
        private T NewInstance<T>(MemberReference memberRef) where T : NMember, new()
        {
            var id = DocIdHelper.GetXmlId(memberRef);
            var member = new T {Name = memberRef.Name, FullName = memberRef.FullName, Id = id, NormalizedId = DocIdHelper.StripXmlId(id)};
            member.DocNode = _source.FindMemberDoc(member.Id);
            member.DeclaringType = GetTypeReference(memberRef.DeclaringType);

            if (memberRef is TypeReference) 
            {
                FillTypeReference(member, (TypeReference) memberRef);
            }
            else if (memberRef is IGenericParameterProvider)
            {
                FillGenericParameters(member, (IGenericParameterProvider) memberRef);
                member.Name = BuildGenericName(member.Name, member.GenericParameters);
                member.FullName = BuildGenericName(member.FullName, member.GenericParameters);
            }

            // Add custom attributes for this member
            if (memberRef is ICustomAttributeProvider)
            {
                var attributes = ((ICustomAttributeProvider) memberRef).CustomAttributes;
                foreach (var customAttribute in attributes)
                    member.Attributes.Add(CustomAttributeToString(customAttribute));
            }

            // Add generic parameter contraints
            return member;
        }

        /// <summary>
        /// Gets the name of a generic type. Intead of List`1 returns List&lt;T&gt;
        /// </summary>
        /// <param name="name">The original name that contains a ` indicating the usage of generics.</param>
        /// <param name="genericParameters">The generic parameters/arguments.</param>
        /// <returns></returns>
        private static string BuildGenericName(string name, IEnumerable<NTypeReference> genericParameters)
        {
            int genericIndexTag = name.IndexOf('`');
            if (genericIndexTag < 0)
                genericIndexTag = name.Length;

            var nameBuilder = new StringBuilder(name.Substring(0, genericIndexTag));
            int i = 0;
            foreach (var genericParameter in genericParameters)
            {
                nameBuilder.Append(i == 0 ? "<" : ",");
                nameBuilder.Append(genericParameter.Name);
                i++;
            }
            if (i > 0)
                nameBuilder.Append(">");
            return nameBuilder.ToString();
        }


        /// <summary>
        /// Convert a custom attribute declaration to a textual declaration.
        /// </summary>
        /// <param name="customAttribute">The custom attribute.</param>
        /// <returns></returns>
        private static string CustomAttributeToString(CustomAttribute customAttribute)
        {
            var builder = new StringBuilder();
            builder.Append(customAttribute.AttributeType.Name);

            if (customAttribute.HasConstructorArguments || customAttribute.HasProperties)
            {
                builder.Append("(");

                // Parse Constructor Arguments
                for (int i = 0; i < customAttribute.ConstructorArguments.Count; i++)
                {
                    var customAttributeArgument = customAttribute.ConstructorArguments[i];
                    builder.Append(GetTextFromValue(customAttributeArgument.Value));
                    if ((i+1) < customAttribute.ConstructorArguments.Count || customAttribute.HasProperties)
                        builder.Append(", ");
                }

                // Parse Property Arguments
                var propertyBuilder = new StringBuilder();
                for (int i = 0; i < customAttribute.Properties.Count; i++)
                {
                    var customProperty = customAttribute.Properties[i];
                    var value = customProperty.Argument.Value;

                    if (value != null)
                    {
                        propertyBuilder.Append(customProperty.Name);
                        propertyBuilder.Append(" = ");
                        propertyBuilder.Append(GetTextFromValue(value));
                    }
                    propertyBuilder.Append(", ");
                }
                builder.Append(propertyBuilder.ToString().TrimEnd(' ', ','));
              

                builder.Append(")");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets the text from value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static string GetTextFromValue(object value)
        {
            if (value == null)
                return "null";
            if (value is string)
                return "\"" + value + "\"";
            if (value is bool)
                return (bool) value ? "true" : "false";
            return value.ToString();
        }

        /// <summary>
        /// Fills the generic parameters.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="genericParameterProvider">The generic parameter provider.</param>
        private void FillGenericParameters(INMemberReference member, IGenericParameterProvider genericParameterProvider)
        {
            foreach (var genericParameterDef in genericParameterProvider.GenericParameters)
            {
                var genericParameter = new NGenericParameter
                                           {
                                               HasConstraints = genericParameterDef.HasConstraints,
                                               HasCustomAttributes = genericParameterDef.HasCustomAttributes,
                                               HasDefaultConstructorConstraint = genericParameterDef.HasDefaultConstructorConstraint,
                                               HasNotNullableValueTypeConstraint = genericParameterDef.HasNotNullableValueTypeConstraint,
                                               HasReferenceTypeConstraint = genericParameterDef.HasReferenceTypeConstraint
                                           };
                FillTypeReference(genericParameter, genericParameterDef);

                // Fill constraint
                foreach (var constraint in genericParameterDef.Constraints)
                    genericParameter.Constraints.Add( GetTypeReference(constraint));

                member.GenericParameters.Add(genericParameter);
            }
        }
   }
}