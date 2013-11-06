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
using System.IO;
using System.Text;
#if W8CORE
using SharpDX.Text;
#endif
using System.Text.RegularExpressions;

using SharpDX.Direct3D11;
using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>The model reader class.</summary>
    public class ModelReader : BinarySerializer
    {
        /// <summary>The shared PTR.</summary>
        private DataPointer sharedPtr;

        /// <summary>Initializes a new instance of the <see cref="ModelReader"/> class.</summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="textureLoader">The texture loader.</param>
        /// <exception cref="System.ArgumentNullException">
        /// graphicsDevice
        /// or
        /// stream
        /// or
        /// textureLoader
        /// </exception>
        public ModelReader(GraphicsDevice graphicsDevice, Stream stream, ModelMaterialTextureLoaderDelegate textureLoader) : base(stream, SerializerMode.Read, Encoding.ASCII)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (textureLoader == null)
            {
                throw new ArgumentNullException("textureLoader");
            }

            GraphicsDevice = graphicsDevice;
            TextureLoaderDelegate = textureLoader;
            ArrayLengthType = ArrayLengthType.Int;
        }

        /// <summary>Allocates the shared memory.</summary>
        /// <param name="size">The size.</param>
        internal void AllocateSharedMemory(int size)
        {
            sharedPtr = new DataPointer(Utilities.AllocateMemory(size), size);
            ToDispose(sharedPtr.Pointer);
        }

        /// <summary>Gets the shared memory pointer.</summary>
        /// <value>The shared memory pointer.</value>
        internal IntPtr SharedMemoryPointer
        {
            get
            {
                return sharedPtr.Pointer;
            }
        }

        /// <summary>The model.</summary>
        protected Model Model;

        /// <summary>The current mesh.</summary>
        protected ModelMesh CurrentMesh;

        /// <summary>The graphics device.</summary>
        protected readonly GraphicsDevice GraphicsDevice;

        /// <summary>The embedded textures.</summary>
        private List<Texture> EmbeddedTextures = new List<Texture>();

        /// <summary>The texture loader delegate.</summary>
        protected readonly ModelMaterialTextureLoaderDelegate TextureLoaderDelegate;

        /// <summary>Creates the model.</summary>
        /// <returns>Model.</returns>
        protected virtual Model CreateModel()
        {
            return new Model();
        }

        /// <summary>Creates the model material.</summary>
        /// <returns>Material.</returns>
        protected virtual Material CreateModelMaterial()
        {
            return new Material();
        }

        /// <summary>Creates the model bone.</summary>
        /// <returns>ModelBone.</returns>
        protected virtual ModelBone CreateModelBone()
        {
            return new ModelBone();
        }

        /// <summary>Creates the model mesh.</summary>
        /// <returns>ModelMesh.</returns>
        protected virtual ModelMesh CreateModelMesh()
        {
            return new ModelMesh();
        }

        /// <summary>Creates the model mesh part.</summary>
        /// <returns>ModelMeshPart.</returns>
        protected virtual ModelMeshPart CreateModelMeshPart()
        {
            return new ModelMeshPart();
        }

        /// <summary>Creates the model material collection.</summary>
        /// <param name="capacity">The capacity.</param>
        /// <returns>MaterialCollection.</returns>
        protected virtual MaterialCollection CreateModelMaterialCollection(int capacity)
        {
            return new MaterialCollection(capacity);
        }

        /// <summary>Creates the model bone collection.</summary>
        /// <param name="capacity">The capacity.</param>
        /// <returns>ModelBoneCollection.</returns>
        protected virtual ModelBoneCollection CreateModelBoneCollection(int capacity)
        {
            return new ModelBoneCollection(capacity);
        }

        /// <summary>Creates the model mesh collection.</summary>
        /// <param name="capacity">The capacity.</param>
        /// <returns>ModelMeshCollection.</returns>
        protected virtual ModelMeshCollection CreateModelMeshCollection(int capacity)
        {
            return new ModelMeshCollection(capacity);
        }

        /// <summary>Creates the model mesh part collection.</summary>
        /// <param name="capacity">The capacity.</param>
        /// <returns>ModelMeshPartCollection.</returns>
        protected virtual ModelMeshPartCollection CreateModelMeshPartCollection(int capacity)
        {
            return new ModelMeshPartCollection(capacity);
        }

        /// <summary>Creates the vertex buffer binding collection.</summary>
        /// <param name="capacity">The capacity.</param>
        /// <returns>VertexBufferBindingCollection.</returns>
        protected virtual VertexBufferBindingCollection CreateVertexBufferBindingCollection(int capacity)
        {
            return new VertexBufferBindingCollection(capacity);
        }

        /// <summary>Creates the property collection.</summary>
        /// <param name="capacity">The capacity.</param>
        /// <returns>PropertyCollection.</returns>
        protected virtual PropertyCollection CreatePropertyCollection(int capacity)
        {
            return new PropertyCollection(capacity);
        }

        /// <summary>Creates the material property collection.</summary>
        /// <param name="capacity">The capacity.</param>
        /// <returns>PropertyCollection.</returns>
        protected virtual PropertyCollection CreateMaterialPropertyCollection(int capacity)
        {
            return new PropertyCollection(capacity);
        }

        /// <summary>Creates the buffer collection.</summary>
        /// <param name="capacity">The capacity.</param>
        /// <returns>BufferCollection.</returns>
        protected virtual BufferCollection CreateBufferCollection(int capacity)
        {
            return new BufferCollection(capacity);
        }

        /// <summary>Creates the vertex buffer binding.</summary>
        /// <returns>VertexBufferBinding.</returns>
        protected virtual VertexBufferBinding CreateVertexBufferBinding()
        {
            return new VertexBufferBinding();
        }

        /// <summary>Creates the material texture.</summary>
        /// <returns>MaterialTexture.</returns>
        protected virtual MaterialTexture CreateMaterialTexture()
        {
            return new MaterialTexture();
        }

        /// <summary>Creates the material texture stack.</summary>
        /// <param name="capacity">The capacity.</param>
        /// <returns>MaterialTextureStack.</returns>
        protected virtual MaterialTextureStack CreateMaterialTextureStack(int capacity)
        {
            return new MaterialTextureStack(capacity);
        }

        /// <summary>Reads the model.</summary>
        /// <returns>Model.</returns>
        public Model ReadModel()
        {
            Model = CreateModel();
            var model = Model;
            ReadModel(ref model);
            return model;
        }

        /// <summary>Reads the model.</summary>
        /// <param name="model">The model.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        protected virtual void ReadModel(ref Model model)
        {
            // Starts the whole ModelData by the magiccode "TKMD"
            // If the serializer don't find the TKMD, It will throw an
            // exception that will be catched by Load method.
            BeginChunk(ModelData.MagicCode);

            // Writes the version
            int version = Reader.ReadInt32();
            if (version != ModelData.Version)
            {
                throw new NotSupportedException(string.Format("EffectData version [0x{0:X}] is not supported. Expecting [0x{1:X}]", version, ModelData.Version));
            }

            // Allocated the shared memory used to load this Model
            AllocateSharedMemory(Reader.ReadInt32());

            // Textures / preload embedded textures
            BeginChunk("TEXS");
            int textureCount = Reader.ReadInt32();
            for (int i = 0; i < textureCount; i++)
            {
                byte[] textureData = null;
                Serialize(ref textureData);
                EmbeddedTextures.Add(Texture.Load(GraphicsDevice, new MemoryStream(textureData)));
            }
            EndChunk();

            // Material section
            BeginChunk("MATL");
            ReadMaterials(ref model.Materials);
            EndChunk();

            // Bones section
            BeginChunk("BONE");
            ReadBones(ref model.Bones);
            EndChunk();

            //// DISABLE_SKINNED_BONES
            //// Skinned Bones section
            //BeginChunk("SKIN");
            //ReadBones(ref model.SkinnedBones);
            //EndChunk();

            // Mesh section
            BeginChunk("MESH");
            ReadMeshes(ref model.Meshes);
            EndChunk();

            // Serialize attributes
            ReadProperties(ref model.Properties);

            // Close TKMD section
            EndChunk();
        }

        /// <summary>Reads the bones.</summary>
        /// <param name="bones">The bones.</param>
        /// <exception cref="System.InvalidOperationException">Invalid children index for bone</exception>
        protected virtual void ReadBones(ref ModelBoneCollection  bones)
        {
            // Read all bones
            ReadList(ref bones, CreateModelBoneCollection, CreateModelBone, ReadBone);

            // Fix all children bones
            int count = bones.Count;
            for (int i = 0; i < count; i++)
            {
                var bone = bones[i];
                // If bone has no children, then move on
                if (bone.Children == null) continue;

                var children = bone.Children;
                var childIndices = children.ChildIndices;
                foreach (int childIndex in childIndices)
                {
                    if (childIndex < 0)
                    {
                        children.Add(null);
                    }
                    else if (childIndex < count)
                    {
                        children.Add(bones[childIndex]);
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid children index for bone");
                    }
                }
                children.ChildIndices = null;
            }
        }

        /// <summary>The name automatic property key delegate delegate.</summary>
        /// <param name="name">The name.</param>
        /// <returns>PropertyKey.</returns>
        public delegate PropertyKey NameToPropertyKeyDelegate(string name);

        /// <summary>Reads the properties.</summary>
        /// <param name="properties">The properties.</param>
        protected virtual void ReadProperties(ref PropertyCollection properties)
        {
            ReadProperties(ref properties, name => new PropertyKey(name));
        }

        /// <summary>Reads the properties.</summary>
        /// <param name="properties">The properties.</param>
        /// <param name="nameToKey">The name automatic key.</param>
        /// <exception cref="System.ArgumentNullException">nameToKey</exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        protected virtual void ReadProperties(ref PropertyCollection properties, NameToPropertyKeyDelegate nameToKey)
        {
            if (nameToKey == null)
            {
                throw new ArgumentNullException("nameToKey");
            }

            int count = Reader.ReadInt32();

            if (properties == null)
            {
                properties = CreatePropertyCollection(count);
            }

            for (int i = 0; i < count; i++)
            {
                string name = null;
                object value = null;
                Serialize(ref name);
                SerializeDynamic(ref value, SerializeFlags.Nullable);;

                var key = nameToKey(name);
                if (key == null)
                {
                    throw new InvalidOperationException(string.Format("Cannot convert property name [{0}] to null key", name));
                }

                properties[key] = value;
            }
        }

        /// <summary>Reads the materials.</summary>
        /// <param name="materials">The materials.</param>
        protected virtual void ReadMaterials(ref MaterialCollection materials)
        {
            ReadList(ref materials, CreateModelMaterialCollection, CreateModelMaterial, ReadMaterial);
        }

        /// <summary>Reads the meshes.</summary>
        /// <param name="meshes">The meshes.</param>
        protected virtual void ReadMeshes(ref ModelMeshCollection meshes)
        {
            ReadList(ref meshes, CreateModelMeshCollection, CreateModelMesh, ReadMesh);
        }

        /// <summary>Reads the vertex buffers.</summary>
        /// <param name="vertices">The vertices.</param>
        protected virtual void ReadVertexBuffers(ref VertexBufferBindingCollection vertices)
        {
            ReadList(ref vertices, CreateVertexBufferBindingCollection, CreateVertexBufferBinding, ReadVertexBuffer);
        }

        /// <summary>Reads the mesh parts.</summary>
        /// <param name="meshParts">The mesh parts.</param>
        protected virtual void ReadMeshParts(ref ModelMeshPartCollection meshParts)
        {
            ReadList(ref meshParts, CreateModelMeshPartCollection, CreateModelMeshPart, ReadMeshPart);
        }

        /// <summary>Reads the index buffers.</summary>
        /// <param name="list">The list.</param>
        protected virtual void ReadIndexBuffers(ref BufferCollection list)
        {
            int count = Reader.ReadInt32();
            list = CreateBufferCollection(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(ReadIndexBuffer());
            }
        }

        /// <summary>Reads the material.</summary>
        /// <param name="material">The material.</param>
        protected virtual void ReadMaterial(ref Material material)
        {
            var textureStackCount = Reader.ReadInt32();
            var properties = CreateMaterialPropertyCollection(textureStackCount + 32);
            material.Properties = properties;
            for (int i = 0; i < textureStackCount; i++)
            {
                string keyName = null;
                Serialize(ref keyName);

                MaterialTextureStack textureStack = null;
                ReadList(ref textureStack, CreateMaterialTextureStack, CreateMaterialTexture, ReadMaterialTexture);

                var key = MaterialKeysBase.FindKeyByName(keyName) ?? new PropertyKey(keyName);

                properties[key] = textureStack;
            }

            ReadProperties(ref material.Properties, name => MaterialKeysBase.FindKeyByName(name) ?? new PropertyKey(name));
        }

        /// <summary>The regex match embedded texture.</summary>
        private static Regex RegexMatchEmbeddedTexture = new Regex(@"^\*(\d+)$");

        /// <summary>Reads the material texture.</summary>
        /// <param name="materialTexture">The material texture.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        protected virtual void ReadMaterialTexture(ref MaterialTexture materialTexture)
        {
            // Loads the texture
            string filePath = null;
            Serialize(ref filePath);
            materialTexture.Name = Path.GetFileNameWithoutExtension(filePath);

            if (!string.IsNullOrEmpty(filePath))
            {
                var match = RegexMatchEmbeddedTexture.Match(filePath);

                if (match.Success)
                {
                    var textureIndex = int.Parse(match.Groups[1].Value);
                    if (textureIndex > EmbeddedTextures.Count)
                    {
                        throw new InvalidOperationException(string.Format("Out of range embedded texture with index [{0}] vs max [{1}]", textureIndex, EmbeddedTextures.Count));
                    }
                    materialTexture.Texture = EmbeddedTextures[textureIndex];
                }
                else
                {
                    // If the texture name is empty, the texture was probably not located when compiling, so we skip it
                    // TODO Check if we want another behavior?
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        materialTexture.Texture = TextureLoaderDelegate(filePath);
                    }
                }
            }

            materialTexture.Index = Reader.ReadInt32();
            materialTexture.UVIndex = Reader.ReadInt32();
            materialTexture.BlendFactor = Reader.ReadSingle();
            materialTexture.Operation = (MaterialTextureOperator)Reader.ReadByte();
            materialTexture.WrapMode = (TextureAddressMode)Reader.ReadInt32();
            materialTexture.Flags = (MaterialTextureFlags)Reader.ReadByte();
        }

        /// <summary>Reads the bone.</summary>
        /// <param name="bone">The bone.</param>
        /// <exception cref="System.InvalidOperationException">Invalid index for parent bone</exception>
        protected virtual void ReadBone(ref ModelBone bone)
        {
            // Read ModelBone index
            bone.Index = Reader.ReadInt32();

            // Read Parent Index
            int parentIndex = Reader.ReadInt32();
            if (parentIndex > Model.Bones.Count)
            {
                throw new InvalidOperationException("Invalid index for parent bone");
            }
            bone.Parent = parentIndex >= 0 ? Model.Bones[parentIndex] : null;

            // Transform
            Serialize(ref bone.Transform);

            // Name
            string boneName = null;
            Serialize(ref boneName, false, SerializeFlags.Nullable);
            bone.Name = boneName;

            // Indices
            List<int> indices = null;
            Serialize(ref indices, Serialize, SerializeFlags.Nullable);
            if (indices != null)
            {
                bone.Children = CreateModelBoneCollection(indices.Count);
                bone.Children.ChildIndices = indices;
            }
        }

        /// <summary>Reads the mesh.</summary>
        /// <param name="mesh">The mesh.</param>
        protected virtual void ReadMesh(ref ModelMesh mesh)
        {
            CurrentMesh = mesh;
            string meshName = null;
            Serialize(ref meshName, false, SerializeFlags.Nullable);
            mesh.Name = meshName;
            int parentBoneIndex = Reader.ReadInt32();
            if (parentBoneIndex >= 0) mesh.ParentBone = Model.Bones[parentBoneIndex];

            // Read the bounding sphere
            Serialize(ref mesh.BoundingSphere);

            ReadVertexBuffers(ref mesh.VertexBuffers);
            ReadIndexBuffers(ref mesh.IndexBuffers);
            ReadMeshParts(ref mesh.MeshParts);

            ReadProperties(ref mesh.Properties);
            CurrentMesh = null;
        }

        /// <summary>Reads the mesh part.</summary>
        /// <param name="meshPart">The mesh part.</param>
        protected virtual void ReadMeshPart(ref ModelMeshPart meshPart)
        {
            // Set the Parent mesh for the current ModelMeshPart.
            meshPart.ParentMesh = CurrentMesh;

            // Material
            int materialIndex = Reader.ReadInt32();
            meshPart.Material = Model.Materials[materialIndex];

            // IndexBuffer
            var indexBufferRange = default(ModelData.BufferRange);
            indexBufferRange.Serialize(this);
            meshPart.IndexBuffer = GetFromList(indexBufferRange, CurrentMesh.IndexBuffers);

            // VertexBuffer
            var vertexBufferRange = default(ModelData.BufferRange);
            vertexBufferRange.Serialize(this);
            meshPart.VertexBuffer = GetFromList(vertexBufferRange, CurrentMesh.VertexBuffers);

            // Properties
            ReadProperties(ref meshPart.Properties);
        }

        /// <summary>Reads the vertex buffer.</summary>
        /// <param name="vertexBufferBinding">The vertex buffer binding.</param>
        protected virtual void ReadVertexBuffer(ref VertexBufferBinding vertexBufferBinding)
        {
            // Read the number of vertices
            int count = Reader.ReadInt32();

            // Read vertex elements
            int vertexElementCount = Reader.ReadInt32();
            var elements = new VertexElement[vertexElementCount];
            for (int i = 0; i < vertexElementCount; i++)
            {
                elements[i].Serialize(this);
            }
            vertexBufferBinding.Layout = VertexInputLayout.New(0, elements);

            // Read Vertex Buffer
            int sizeInBytes = Reader.ReadInt32();
            SerializeMemoryRegion(SharedMemoryPointer, sizeInBytes);
            vertexBufferBinding.Buffer =  Buffer.New(GraphicsDevice, new DataPointer(SharedMemoryPointer, sizeInBytes), sizeInBytes / count, BufferFlags.VertexBuffer, ResourceUsage.Immutable);
        }

        /// <summary>Reads the index buffer.</summary>
        /// <returns>Buffer.</returns>
        protected virtual Buffer ReadIndexBuffer()
        {
            int indexCount = Reader.ReadInt32();
            int sizeInBytes = Reader.ReadInt32();
            SerializeMemoryRegion(SharedMemoryPointer, sizeInBytes);
            return Buffer.New(GraphicsDevice, new DataPointer(SharedMemoryPointer, sizeInBytes), sizeInBytes / indexCount, BufferFlags.IndexBuffer, ResourceUsage.Immutable);
        }

        /// <summary>The create list delegate delegate.</summary>
        /// <typeparam name="TLIST">The type of the tlist.</typeparam>
        /// <typeparam name="TITEM">The type of the titem.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>``0.</returns>
        protected delegate TLIST CreateListDelegate<out TLIST, TITEM>(int list) where TLIST : List<TITEM>;

        /// <summary>The create item delegate delegate.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>``0.</returns>
        protected delegate T CreateItemDelegate<out T>();

        /// <summary>The read item delegate delegate.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        protected delegate void ReadItemDelegate<T>(ref T item);

        /// <summary>Reads the list.</summary>
        /// <typeparam name="TLIST">The <see langword="Type" /> of the tlist.</typeparam>
        /// <typeparam name="TITEM">The <see langword="Type" /> of the titem.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="listCreate">The list create.</param>
        /// <param name="itemCreate">The item create.</param>
        /// <param name="itemReader">The item reader.</param>
        /// <returns>The TLIST.</returns>
        protected virtual TLIST ReadList<TLIST, TITEM>(ref TLIST list, CreateListDelegate<TLIST, TITEM> listCreate, CreateItemDelegate<TITEM> itemCreate, ReadItemDelegate<TITEM> itemReader) where TLIST : List<TITEM>
        {
            int count = Reader.ReadInt32();
            list = listCreate(count);
            for (int i = 0; i < count; i++)
            {
                var item = itemCreate();
                itemReader(ref item);
                list.Add(item);
            }
            return list;
        }

        /// <summary>Gets from list.</summary>
        /// <typeparam name="T">The <see langword="Type" /> of attribute.</typeparam>
        /// <param name="range">The range.</param>
        /// <param name="list">The list.</param>
        /// <returns>ModelBufferRange{``0}.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        private ModelBufferRange<T> GetFromList<T>(ModelData.BufferRange range, IList<T> list)
        {
            var index = range.Slot;
            if (index >= list.Count)
            {
                throw new InvalidOperationException(string.Format("Invalid slot [{0}] for {1} (Max: {2})", index, typeof(T).Name, list.Count));
            }
            return new ModelBufferRange<T> { Resource = list[index], Count = range.Count, Start = range.Start };
        }
    }
}