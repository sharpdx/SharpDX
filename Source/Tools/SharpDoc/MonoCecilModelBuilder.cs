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
using System.Xml;

using Mono.Cecil;

using SharpCore;
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
        private Dictionary<string, NDocumentApi> mapModuleToDoc = new Dictionary<string, NDocumentApi>();
        private Dictionary<string, IModelReference>  membersCache = new Dictionary<string, IModelReference>();

        public Func<IModelReference, string> PageIdFunction { get; set; }

        private string CurrentMergeGroup { get; set; }

        private NAssembly CurrentAssembly { get; set; }

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
            CurrentMergeGroup = assemblySource.MergeGroup;

            _source = assemblySource;
            _registry = memberRegistry;

            var assemblyDefinition = (AssemblyDefinition)assemblySource.Assembly;

            var assemblyName = assemblyDefinition.Name.Name;
            var assemblyId = "A:" + assemblyName;

            var assembly = (NAssembly)_registry.FindById(assemblyId);
            // If new assembly
            if (assembly == null)
            {
                assembly = new NAssembly { Name = assemblyName };
                assembly.FullName = assembly.Name;
                assembly.Id = assemblyId;
                assembly.PageId = PageIdFunction(assembly);
                assembly.PageTitle = assembly.Name + " " + assembly.Category;
                assembly.Version = assemblyDefinition.Name.Version.ToString();
                assembly.FileName = Path.GetFileName(Utility.GetProperFilePathCapitalization(assemblySource.Filename));

                // Apply documentation from AssemblyDoc special class
                assembly.DocNode = _source.Document.FindMemberDoc("T:" + assembly.Name + "." + AssemblyDocClass);

                _registry.Register(assembly, assembly);
            }
            assembly.SetApiGroup(CurrentMergeGroup, true);
            CurrentAssembly = assembly;

            Logger.Message("Processing assembly [{0}] [{1}]", assembly.FullName, CurrentMergeGroup);

            // Process namespaces
            // Namespaces are created only if a type is actually public
            foreach (var module in assemblyDefinition.Modules)
            {
                foreach (var type in module.Types)
                {
                    // Todo add configurable filter
                    if (!type.IsPublic)
                        continue;

                    // Skip empty namespaces and special <Module>
                    if (string.IsNullOrEmpty(type.Namespace) || type.Namespace.StartsWith("<"))
                        continue;

                    // Create naemespace
                    var parentNamespace = AddNamespace(assembly, type.Namespace);
                    parentNamespace.SetApiGroup(CurrentMergeGroup, true);

                    AddType(parentNamespace, type);
                }
            }

            //// Remove empty namespaces
            //foreach (var removeNamespace in namespaces.Values)
            //{
            //    if (removeNamespace.Types.Count == 0)
            //        assembly.Namespaces.Remove(removeNamespace);
            //}

            // Sort namespace in alphabetical order

            //@assembly.Namespaces.Sort((left, right) => left.PageId.CompareTo(right.PageId));

            //foreach (var namespaceName in @assembly.Namespaces)
            //    namespaces[namespaceName].Types.Sort((left, right) => left.PageId.CompareTo(right.PageId));

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
            var namespaceId = "N:" + name;

            var @namespace = (NNamespace)_registry.FindById(namespaceId, CurrentAssembly);

            if (@namespace == null)
            {
                @namespace = new NNamespace(name) { Assembly = assembly, Id = "N:" + name };
                @namespace.FullName = @namespace.Name;
                @namespace.PageId = PageIdFunction(@namespace);
                @namespace.PageTitle = @namespace.Name + " " + @namespace.Category + " (" + assembly.Name + ")";

                _registry.Register(assembly, @namespace);

                // Apply documentation on namespace from NamespaceDoc special class
                @namespace.DocNode = _source.Document.FindMemberDoc("T:" + name + "." + NamespaceDocClass);

                // Add See Alsos
                @namespace.SeeAlsos.Add(new NSeeAlso(@namespace.Assembly));

                assembly.Namespaces.Add(@namespace);
            }

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

            var typeId = DocIdHelper.GetXmlId(typeDef);
            var type = (NType)_registry.FindById(typeId, CurrentAssembly);

            // If this is a new type create and register it
            if (type == null)
            {
                if (typeDef.IsInterface)
                {
                    type = NewInstance<NInterface>(@namespace, typeDef);
                    type.MemberType = NMemberType.Interface;
                }
                else if (typeDef.IsEnum)
                {
                    type = NewInstance<NEnum>(@namespace, typeDef);
                    type.MemberType = NMemberType.Enum;
                }
                else if (typeDef.IsValueType)
                {
                    type = NewInstance<NStruct>(@namespace, typeDef);
                    type.MemberType = NMemberType.Struct;
                }
                else if (MonoCecilHelper.IsDelegate(typeDef))
                {
                    type = NewInstance<NDelegate>(@namespace, typeDef);
                    type.MemberType = NMemberType.Delegate;
                }
                else if (typeDef.IsClass)
                {
                    type = NewInstance<NClass>(@namespace, typeDef);
                    type.MemberType = NMemberType.Class;
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Unsupported type [{0}]", typeDef.FullName));
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

                // Revert back declaration abstract sealed class are actually declared as static class 
                if (type is NClass && type.IsAbstract && type.IsFinal)
                {
                    type.IsStatic = true;
                    type.IsAbstract = false;
                    type.IsFinal = false;
                }
                else if (type is NEnum || type is NStruct || type is NDelegate)
                {
                    // We know that enum is sealed, so don't duplicate it
                    type.IsFinal = false;
                }

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

                type.SeeAlsos.Add(new NSeeAlso(@namespace));
                type.SeeAlsos.Add(new NSeeAlso(@namespace.Assembly));
            }

            // Add current group
            type.SetApiGroup(CurrentMergeGroup, true);

            // Because a type in a different assemblies can have new methods/properties/fields...etc.
            // We need to check that there is no new members

            // Add methods, constructors, operators. Todo add configurable filter
            foreach (var method in typeDef.Methods.Where(this.IsMemberToDisplay))
                AddMethod(type, method);

            // Add fields. Todo add configurable filter
            foreach (var field in typeDef.Fields.Where(this.IsMemberToDisplay))
                AddField(type, field);

            // Add events. Todo add configurable filter
            foreach (var eventInfo in typeDef.Events.Where(this.IsMemberToDisplay))
                AddEvent(type, eventInfo);

            // Add properties Todo add configurable filter
            foreach (var property in typeDef.Properties.Where(this.IsMemberToDisplay))
                AddProperty(type, property);

            // Recalculate a PageId based on the number of overriding methods.
            var counters = new Dictionary<string, int>();
            foreach (var member in type.Members)
            {
                string id =  PageIdFunction(member);

                if (!counters.ContainsKey(id))
                    counters.Add(id, 0);
                else
                {
                    counters[id]++;
                    id = id + "_" + counters[id];
                }

                member.PageId = id;
            }

            // Tag methods that are overriden
            foreach (var method in type.MethodsAndConstructors)
            {
                var id = PageIdFunction(method);
                if (counters.ContainsKey(id) && counters[id] > 0)
                    method.HasOverrides = true;
            }

            // Sort members
            type.Members.Sort((left, right) => left.Name.CompareTo(right.Name));

            // For enumeration, we are only using MsdnId from enum
            if (type is NEnum)
            {
                foreach (var field in type.Fields)
                    field.MsdnId = type.MsdnId;
            }

            // Add nested types to the global namespace.
            foreach (var nestedType in typeDef.NestedTypes)
                AddType(@namespace, nestedType);

            UpdatePageTitle(type);
        }

        /// <summary>
        /// Determines whether [is member to display] [the specified member ref].
        /// </summary>
        /// <param name="memberRef">The member ref.</param>
        /// <returns>
        ///   <c>true</c> if [is member to display] [the specified member ref]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsMemberToDisplay(MemberReference memberRef)
        {
            // TODO Mono.Cecil doesn't provide a standard way to access public/protected access
            // So this is a temporary hardcoded workaround, though we must be able to select the level to display (public/protected/private...etc.)

            var method = memberRef as MethodDefinition;
            if (method != null && (method.IsPublic || method.IsFamilyOrAssembly || method.IsFamily))
                return true;

            var field = memberRef as FieldDefinition;
            if (field != null && (field.IsPublic || field.IsFamilyOrAssembly || field.IsFamily))
                return true;

            var eventInfo = memberRef as EventDefinition;
            if (eventInfo != null && (eventInfo.AddMethod.IsPublic || eventInfo.AddMethod.IsFamilyOrAssembly || eventInfo.AddMethod.IsFamily))
                return true;

            var property = memberRef as PropertyDefinition;
            if (property != null && (
                (property.GetMethod != null && (property.GetMethod.IsPublic || property.GetMethod.IsFamilyOrAssembly || property.GetMethod.IsFamily)) || (property.SetMethod != null && (property.SetMethod.IsPublic || property.SetMethod.IsFamilyOrAssembly || property.SetMethod.IsFamily))))
                return true;
            return false;
        }

        /// <summary>
        /// Creates a method from a method definition.
        /// </summary>
        /// <param name="methodDef">The method def.</param>
        /// <returns>A NMethod</returns>
        private NMethod CreateMethodFromDefinition(NNamespace @namespace, MethodDefinition methodDef)
        {
            NMethod method;

            // Create the associated type
            if (methodDef.IsConstructor)
            {
                method = NewInstance<NConstructor>(@namespace, methodDef);

                // Constructors must have typename instead of .ctor
                method.Name = method.DeclaringType.Name;
            }
            else if (methodDef.IsSpecialName && methodDef.Name.StartsWith(MethodOperatorPrefix))
            {
                method = NewInstance<NOperator>(@namespace, methodDef);
                method.Name = method.Name.Substring(MethodOperatorPrefix.Length, method.Name.Length - MethodOperatorPrefix.Length);
            }
            else
            {
                method = NewInstance<NMethod>(@namespace, methodDef);
            }

            // Setup visibility
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
                method.Overrides = GetMethodReference(@namespace, MonoCecilHelper.GetBaseMethodInTypeHierarchy(methodDef));

                // implements
                method.Implements = GetMethodReference(@namespace, MonoCecilHelper.GetBaseMethodInInterfaceHierarchy(methodDef));

                // If this method doesn't have any documentation, use inherited documentation
                if (string.IsNullOrEmpty(method.Description))
                {
                    method.DocNode = method.Overrides != null ? method.Overrides.DocNode : method.Implements != null ? method.Implements.DocNode : method.DocNode;
                }
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
        /// <param name="isSpecialMethod">if set to <c>true</c> [is property].</param>
        /// <returns></returns>
        private NMethod AddMethod(NMember parent, MethodDefinition methodDef, bool isSpecialMethod = false)
        {
            // Don't add getter and setters for properties and events
            if (methodDef == null || (!isSpecialMethod && methodDef.IsSpecialName && (methodDef.IsGetter || methodDef.IsSetter || methodDef.IsAddOn || methodDef.IsRemoveOn)))
                return null;

            var method = CreateMethodFromDefinition(parent.Namespace, methodDef);

            // If not a get/set then handle it
            if (!isSpecialMethod)
            {
                var oldMethod = (NMethod)_registry.FindById(method.Id, CurrentAssembly);
                method = oldMethod ?? method;

                if (oldMethod == null)
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

                    // Add SeeAlso
                    method.SeeAlsos.Add(new NSeeAlso(parent));
                    method.SeeAlsos.Add(new NSeeAlso(parent.Namespace));
                    method.SeeAlsos.Add(new NSeeAlso(parent.Namespace.Assembly));

                    UpdatePageTitle(method);
                }
            }

            method.SetApiGroup(CurrentMergeGroup, true);

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
            if (fullName == typeof(object).FullName)
                return "object";
            return name;
        }

        private void LoadSystemAssemblyDoc(INMemberReference memberRef, MemberReference typeReference)
        {
            NDocumentApi doc = null;
            var assemblyPath = Path.GetFullPath(typeReference.Module.FullyQualifiedName);
            if (!mapModuleToDoc.TryGetValue(assemblyPath, out doc))
            {
                var frameworkPath = Utility.GetFrameworkRootDirectory();
                if (assemblyPath.StartsWith(frameworkPath))
                {
                    var assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
                    var assemblyDirectory = Path.GetDirectoryName(assemblyPath);

                    // TODO: improve replace
                    assemblyDirectory = assemblyDirectory.Replace(@"\Framework64\", @"\Framework\");

                    var assemblyXml = Path.Combine(Path.Combine(assemblyDirectory, "en"), assemblyName + ".xml");
                    Logger.Message("Load system documentation [{0}]", assemblyName);
                    doc = NDocumentApi.Load(assemblyXml);
                }
                mapModuleToDoc.Add(assemblyPath, doc);
            }

            if (doc != null)
            {
                memberRef.DocNode = doc.FindMemberDoc(memberRef.Id);
            }
        }

        private void FillMemberReference(INMemberReference memberRef, MemberReference cecilMemberRef)
        {
            memberRef.Id = DocIdHelper.GetXmlId(cecilMemberRef);
            memberRef.PageId = PageIdFunction(memberRef);
            memberRef.Name = ReplacePrimitive(cecilMemberRef.Name, cecilMemberRef.FullName);
            memberRef.FullName = cecilMemberRef.FullName;

            // Load system documentation if needed
            LoadSystemAssemblyDoc(memberRef, cecilMemberRef);

            var typeDef = cecilMemberRef as TypeReference;
            if (typeDef != null)
            {
                memberRef.IsGenericInstance = typeDef.IsGenericInstance;
                memberRef.IsGenericParameter = typeDef.IsGenericParameter;
                memberRef.IsArray = typeDef.IsArray;
                memberRef.IsPointer = typeDef.IsPointer;
                memberRef.IsSentinel = typeDef.IsSentinel;
                FillGenericParameters(memberRef, typeDef);

                // Handle generic instance
                var genericInstanceDef = typeDef as GenericInstanceType;

                if (genericInstanceDef != null)
                {
                    memberRef.ElementType = GetTypeReference(genericInstanceDef.ElementType);

                    if (genericInstanceDef.GenericArguments.Count > 0)
                    {
                        foreach (var genericArgumentDef in genericInstanceDef.GenericArguments)
                        {
                            var genericArgument = new NTypeReference();
                            FillMemberReference(genericArgument, genericArgumentDef);
                            memberRef.GenericArguments.Add(genericArgument);
                        }

                        // Remove `number from Name
                        memberRef.Name = BuildGenericName(typeDef.Name, memberRef.GenericArguments);
                        memberRef.FullName = BuildGenericName(typeDef.FullName, memberRef.GenericArguments);
                    }
                }
                else if (memberRef.GenericParameters.Count > 0)
                {
                    // If generic parameters, than rewrite the name/fullname
                    memberRef.Name = BuildGenericName(typeDef.Name, memberRef.GenericParameters);
                    memberRef.FullName = BuildGenericName(typeDef.FullName, memberRef.GenericParameters);
                }
            }
            else
            {
                var genericParameterProvider = cecilMemberRef as IGenericParameterProvider;
                if (genericParameterProvider != null)
                {
                    this.FillGenericParameters(memberRef, genericParameterProvider);
                    memberRef.Name = BuildGenericName(memberRef.Name, memberRef.GenericParameters);
                    memberRef.FullName = BuildGenericName(memberRef.FullName, memberRef.GenericParameters);
                }
            }

            var member = memberRef as NMember;

            // Add custom attributes for this member
            if (member != null && cecilMemberRef is ICustomAttributeProvider)
            {
                var attributes = ((ICustomAttributeProvider)cecilMemberRef).CustomAttributes;
                foreach (var customAttribute in attributes)
                    member.Attributes.Add(CustomAttributeToString(member, customAttribute));
            }
        }

        private NTypeReference GetTypeReference(TypeReference typeDef)
        {
            if (typeDef == null)
                return null;

            var typeReference = new NTypeReference();
            this.FillMemberReference(typeReference, typeDef);
            return typeReference;
        }

        private INMemberReference GetMethodReference(NNamespace @namespace, MethodDefinition methodDef)
        {
            if (methodDef == null)
                return null;
            return CreateMethodFromDefinition(@namespace, methodDef);
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
            var parameter = new NParameter { Name = parameterDef.Name };
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
            var eventId = DocIdHelper.GetXmlId(eventDef);
            var @event = (NEvent)_registry.FindById(eventId, CurrentAssembly);

            if (@event == null)
            {
                @event = NewInstance<NEvent>(parent.Namespace, eventDef);
                _registry.Register(parent.Namespace.Assembly, @event);

                @event.MemberType = NMemberType.Event;
                @event.EventType = this.GetTypeReference(eventDef.EventType);

                parent.AddMember(@event);
                parent.HasEvents = true;

                var addMethod = AddMethod(@event, eventDef.AddMethod, true);
                var removeMethod = AddMethod(@event, eventDef.RemoveMethod, true);

                // Setup visibility based on event add/remove methods
                var refMethod = addMethod ?? removeMethod;
                @event.Visibility = refMethod.Visibility;
                @event.IsStatic = refMethod.IsStatic;
                @event.IsFinal = refMethod.IsFinal;
                @event.IsAbstract = refMethod.IsAbstract;

                // Add SeeAlso
                @event.SeeAlsos.Add(new NSeeAlso(parent));
                @event.SeeAlsos.Add(new NSeeAlso(parent.Namespace));
                @event.SeeAlsos.Add(new NSeeAlso(parent.Namespace.Assembly));

                UpdatePageTitle(@event);
            }

            @event.SetApiGroup(CurrentMergeGroup, true);
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

            var fieldId = DocIdHelper.GetXmlId(fieldDef);
            var field = (NField)_registry.FindById(fieldId, CurrentAssembly);

            if (field == null)
            {
                field = NewInstance<NField>(parent.Namespace, fieldDef);
                _registry.Register(parent.Namespace.Assembly, field);
                field.MemberType = NMemberType.Field;
                field.FieldType = GetTypeReference(fieldDef.FieldType);

                // Setup visibility
                field.IsStatic = fieldDef.IsStatic;

                if (fieldDef.IsPublic)
                    field.Visibility = NVisibility.Public;
                else if (fieldDef.IsPrivate)
                    field.Visibility = NVisibility.Private;
                else if (fieldDef.IsAssembly)
                    field.Visibility = NVisibility.Internal;
                else if (fieldDef.IsFamily)
                    field.Visibility = NVisibility.Protected;
                else if (fieldDef.IsFamilyOrAssembly)
                    field.Visibility = NVisibility.ProtectedInternal;

                if (fieldDef.Constant != null)
                    field.ConstantValue = GetTextFromValue(fieldDef.Constant);

                parent.AddMember(field);
                parent.HasFields = true;

                field.SeeAlsos.Add(new NSeeAlso(parent));
                field.SeeAlsos.Add(new NSeeAlso(parent.Namespace));
                field.SeeAlsos.Add(new NSeeAlso(parent.Namespace.Assembly));

                UpdatePageTitle(field);
            }

            field.SetApiGroup(CurrentMergeGroup, true);
        }

        /// <summary>
        /// Adds the property.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="propertyDef">The property def.</param>
        private void AddProperty(NType parent, PropertyDefinition propertyDef)
        {
            var propertyId = DocIdHelper.GetXmlId(propertyDef);
            var property = (NProperty)_registry.FindById(propertyId, CurrentAssembly);

            if (property == null)
            {
                property = NewInstance<NProperty>(parent.Namespace, propertyDef);
                _registry.Register(parent.Namespace.Assembly, property);
                property.Namespace = parent.Namespace;

                property.MemberType = NMemberType.Property;
                property.PropertyType = GetTypeReference(propertyDef.PropertyType);
                property.GetMethod = AddMethod(property, propertyDef.GetMethod, true);
                property.SetMethod = AddMethod(property, propertyDef.SetMethod, true);

                parent.AddMember(property);
                parent.HasProperties = true;

                // Setup visibility based on method
                var refMethod = property.GetMethod ?? property.SetMethod;
                property.Visibility = refMethod.Visibility;
                property.IsStatic = refMethod.IsStatic;
                property.IsFinal = refMethod.IsFinal;
                property.IsAbstract = refMethod.IsAbstract;

                property.SeeAlsos.Add(new NSeeAlso(parent));
                property.SeeAlsos.Add(new NSeeAlso(parent.Namespace));
                property.SeeAlsos.Add(new NSeeAlso(parent.Namespace.Assembly));

                UpdatePageTitle(property);
            }

            property.SetApiGroup(CurrentMergeGroup, true);
        }

        /// <summary>
        /// New instance of a <see cref="IModelReference"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberRef">The type def.</param>
        /// <returns></returns>
        private T NewInstance<T>(NNamespace @nameSpace, MemberReference memberRef) where T : NMember, new()
        {
            var id = DocIdHelper.GetXmlId(memberRef);
            var member = new T {Name = memberRef.Name, FullName = memberRef.FullName, Id = id};
            member.PageId = PageIdFunction(member);
            member.DocNode = _source.Document.FindMemberDoc(member.Id);
            member.DeclaringType = GetTypeReference(memberRef.DeclaringType);
            member.Namespace = @nameSpace;
            this.FillMemberReference(member, memberRef);
            // Add generic parameter contraints
            return member;
        }

        private void UpdatePageTitle(NMember member)
        {
            member.PageTitle = (member.DeclaringType != null ? member.DeclaringType.Name + "." : string.Empty) + member.Name + " " + member.Category + " (" + member.Namespace.FullName + ")";           
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
        private static string CustomAttributeToString(NMember member, CustomAttribute customAttribute)
        {
            var builder = new StringBuilder();
            builder.Append(customAttribute.AttributeType.Name);

            // Setup Obsolete flag
            if (customAttribute.AttributeType.Name == "ObsoleteAttribute")
            {
                member.IsObsolete = true;
            }

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
                this.FillMemberReference(genericParameter, genericParameterDef);

                // Fill constraint
                foreach (var constraint in genericParameterDef.Constraints)
                    genericParameter.Constraints.Add( GetTypeReference(constraint));

                member.GenericParameters.Add(genericParameter);
            }
        }
   }
}