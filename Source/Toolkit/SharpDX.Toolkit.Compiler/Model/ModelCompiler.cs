// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

using Assimp;
using Assimp.Unmanaged;

using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX.IO;
using SharpDX.Toolkit.Diagnostics;

namespace SharpDX.Toolkit.Graphics
{
    public sealed class ModelCompiler
    {
        private Assimp.Scene scene;
        private ModelData model;
        private List<ModelData.MeshPart>[] registeredMeshParts;
        private readonly Dictionary<Node, int> meshNodes = new Dictionary<Node, int>();
        private readonly Dictionary<Node, int> skinnedBones = new Dictionary<Node, int>();


        private readonly static List<string> AssimpMaterialDefaultNames = new List<string>()
                                                         {
                                                             "?mat.name,0,0",
                                                             "$mat.twosided,0,0",
                                                             "$mat.shadingm,0,0",
                                                             "$mat.wireframe,0,0",
                                                             "$mat.blend,0,0",
                                                             "$mat.opacity,0,0",
                                                             "$mat.bumpscaling,0,0",
                                                             "$mat.shininess,0,0",
                                                             "$mat.reflectivity,0,0",
                                                             "$mat.shinpercent,0,0",
                                                             "$clr.diffuse,0,0",
                                                             "$clr.ambient,0,0",
                                                             "$clr.specular,0,0",
                                                             "$clr.emissive,0,0",
                                                             "$clr.transparent,0,0",
                                                             "$clr.reflective,0,0",
                                                         };
        private ModelCompiler()
        {
        }

        public static ContentCompilerResult CompileAndSave(string fileName, string outputFile, ModelCompilerOptions compilerOptions)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (outputFile == null)
            {
                throw new ArgumentNullException("outputFile");
            }

            if (compilerOptions == null)
            {
                throw new ArgumentNullException("compilerOptions");
            }


            bool contentToUpdate = true;
            if (compilerOptions.DependencyFile != null)
            {
                if (!FileDependencyList.CheckForChanges(compilerOptions.DependencyFile))
                {
                    contentToUpdate = false;
                }
            }

            var result = new ContentCompilerResult { Logger = new Logger() };
            if (contentToUpdate)
            {
                try
                {
                    result = CompileFromFile(fileName, compilerOptions);

                    if (result.HasErrors)
                    {
                        return result;
                    }

                    var modelData = result.ModelData;

                    // Make sure that directory name doesn't collide with filename
                    var directoryName = Path.GetDirectoryName(outputFile + ".tmp");
                    if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    // Save the model
                    modelData.Save(outputFile);

                    if (compilerOptions.DependencyFile != null)
                    {
                        // Save the dependency
                        var dependencyList = new FileDependencyList();
                        dependencyList.AddDefaultDependencies();
                        dependencyList.AddDependencyPath(fileName);
                        dependencyList.Save(compilerOptions.DependencyFile);
                    }

                    result.IsContentGenerated = true;
                }
                catch (Exception ex)
                {
                    result.Logger.Error("Unexpected exception while converting {0} : {1}", fileName, ex.ToString());
                }
            }


            return result;
        }

        private Logger logger;

        private string modelFilePath;
        private string modelDirectory;

        public static ContentCompilerResult CompileFromFile(string fileName, ModelCompilerOptions compilerOptions)
        {
            var modelCompiler = new ModelCompiler();
            return modelCompiler.CompileFromFileInternal(fileName, compilerOptions);
        }

        private ContentCompilerResult CompileFromFileInternal(string fileName, ModelCompilerOptions compilerOptions)
        {
            logger = new Logger();
            var result = new ContentCompilerResult() { Logger = logger };
            modelFilePath = fileName;
            modelDirectory = Path.GetDirectoryName(modelFilePath);

            // Preload AssimpLibrary if not already loaded
            if (!AssimpLibrary.Instance.LibraryLoaded)
            {
                var rootPath = Path.GetDirectoryName(typeof(AssimpLibrary).Assembly.Location);
                AssimpLibrary.Instance.LoadLibrary(Path.Combine(rootPath, AssimpLibrary.Instance.DefaultLibraryPath32Bit), Path.Combine(rootPath, AssimpLibrary.Instance.DefaultLibraryPath64Bit));
            }

            var importer = new AssimpImporter();
            //importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));

            // Steps for Direct3D Right-Handed, should we make this configurable?
            var steps = PostProcessSteps.FlipUVs | PostProcessSteps.FlipWindingOrder;

            // Setup quality
            switch (compilerOptions.Quality)
            {
                case ModelRealTimeQuality.Low:
                    steps |= PostProcessPreset.TargetRealTimeFast;
                    break;
                case ModelRealTimeQuality.Maximum:
                    steps |= PostProcessPreset.TargetRealTimeMaximumQuality;
                    break;
                default:
                    steps |= PostProcessPreset.TargetRealTimeQuality;
                    break;
            }

            scene = importer.ImportFile(fileName, steps);
            model = new ModelData();
            ProcessScene();

            result.IsContentGenerated = true;
            result.ModelData = model;

            return result;
        }

        private void ProcessScene()
        {
            // Collect bones from mesh
            CollectSkinnedBones();

            registeredMeshParts = new List<ModelData.MeshPart>[scene.MeshCount];

            CollectEmbeddedTextures(scene.Textures);

            // Collect meshes and attached nodes
            CollectMeshNodes(scene.RootNode);
            
            // Collect nodes
            CollectNodes(scene.RootNode, null);

            // Process materials
            ProcessMaterials();
        }

        private void CollectEmbeddedTextures(Texture[] textures)
        {
            if (textures == null)
            {
                return;
            }

            for (int i = 0; i < textures.Length; i++)
            {
                if (textures[i].IsCompressed)
                {
                    var compressedTexture = (CompressedTexture)textures[i];
                    CheckTextureFormat(compressedTexture.FormatHint);
                    model.Textures.Add(compressedTexture.Data);
                }
                else
                {
                    logger.Error("Embedded texture non-compressed are not supported");
                }
            }
        }

        private void CheckTextureFormat(string format)
        {
            switch (format)
            {
                case "jpg":
                case "dds":
                case "tif":
                case "bmp":
                case "gif":
                    return;
            }
            logger.Warning(string.Format("Embedded texture with format [{0}] is not supported", format));
        }

        private void ProcessMaterials()
        {
            if (scene.Materials == null)
            {
                return;
            }

            foreach (var rawMaterial in scene.Materials)
            {
                model.Materials.Add(Process(rawMaterial));
            }
        }

        private ModelData.Material Process(Material rawMaterial)
        {
            var material = new ModelData.Material();
            var properties = material.Properties;

            // Setup all default properties for this material
            if (rawMaterial.HasBlendMode) properties.SetProperty(MaterialKeys.BlendMode, (MaterialBlendMode)rawMaterial.BlendMode);
            if (rawMaterial.HasBumpScaling) properties.SetProperty(MaterialKeys.BumpScaling, rawMaterial.BumpScaling);
            if (rawMaterial.HasColorAmbient) properties.SetProperty(MaterialKeys.ColorAmbient, ConvertColor(rawMaterial.ColorAmbient));
            if (rawMaterial.HasColorDiffuse) properties.SetProperty(MaterialKeys.ColorDiffuse, ConvertColor(rawMaterial.ColorDiffuse));
            if (rawMaterial.HasColorEmissive) properties.SetProperty(MaterialKeys.ColorEmissive, (Color3)ConvertColor(rawMaterial.ColorEmissive));
            if (rawMaterial.HasColorReflective) properties.SetProperty(MaterialKeys.ColorReflective, ConvertColor(rawMaterial.ColorReflective));
            if (rawMaterial.HasColorSpecular) properties.SetProperty(MaterialKeys.ColorSpecular, (Color3)ConvertColor(rawMaterial.ColorSpecular));
            if (rawMaterial.HasColorTransparent) properties.SetProperty(MaterialKeys.ColorTransparent, ConvertColor(rawMaterial.ColorTransparent));
            if (rawMaterial.HasName) properties.SetProperty(MaterialKeys.Name, rawMaterial.Name);
            if (rawMaterial.HasOpacity) properties.SetProperty(MaterialKeys.Opacity, rawMaterial.Opacity);
            if (rawMaterial.HasReflectivity) properties.SetProperty(MaterialKeys.Reflectivity, rawMaterial.Reflectivity);
            if (rawMaterial.HasShininess) properties.SetProperty(MaterialKeys.Shininess, rawMaterial.Shininess);
            if (rawMaterial.HasShininessStrength) properties.SetProperty(MaterialKeys.ShininessStrength, rawMaterial.ShininessStrength);
            if (rawMaterial.HasShadingMode) properties.SetProperty(MaterialKeys.ShadingMode, (MaterialShadingMode)rawMaterial.ShadingMode);
            if (rawMaterial.HasTwoSided) properties.SetProperty(MaterialKeys.TwoSided, rawMaterial.IsTwoSided);
            if (rawMaterial.HasWireFrame) properties.SetProperty(MaterialKeys.Wireframe, rawMaterial.IsWireFrameEnabled);

            // Iterate on other properties
            foreach (var rawProperty in rawMaterial.GetAllProperties())
            {
                var key = rawProperty.FullyQualifiedName;
                if (!properties.ContainsKey(key) && !AssimpMaterialDefaultNames.Contains(rawProperty.FullyQualifiedName))
                {
                    // Texture properties will be added after
                    if (!rawProperty.FullyQualifiedName.StartsWith("$tex"))
                    {
                        // Just use our own key for this material
                        if (key == "$mat.refracti,0,0") key = "Refraction";

                        const string matNamePrefix = "$mat.";
                        if (key.StartsWith(matNamePrefix) && key.Length > matNamePrefix.Length)
                        {
                            var newName = key.Substring(matNamePrefix.Length);
                            key = new StringBuilder().Append(char.ToUpperInvariant(newName[0])).Append(newName.Substring(1)).ToString();
                        }

                        if (properties.ContainsKey(key))
                        {
                            continue;
                        }

                        switch (rawProperty.PropertyType)
                        {
                            case PropertyType.String:
                                properties.Add(key, rawProperty.AsString());
                                break;
                            case PropertyType.Float:
                                switch (rawProperty.ByteCount / 4)
                                {
                                    case 1:
                                        properties.Add(key, rawProperty.AsFloat());
                                        break;
                                    case 2:
                                        properties.Add(key, new Vector2(rawProperty.AsFloatArray()));
                                        break;
                                    case 3:
                                        properties.Add(key, new Vector3(rawProperty.AsFloatArray()));
                                        break;
                                    case 4:
                                        properties.Add(key, new Vector4(rawProperty.AsFloatArray()));
                                        break;
                                    case 16:
                                        properties.Add(key, new Matrix(rawProperty.AsFloatArray()));
                                        break;
                                }
                                break;
                            case PropertyType.Integer:
                                switch (rawProperty.ByteCount / 4)
                                {
                                    case 1:
                                        properties.Add(key, rawProperty.AsInteger());
                                        break;
                                    default:
                                        properties.Add(key, rawProperty.AsIntegerArray());
                                        break;
                                }
                                break;
                            case PropertyType.Buffer:
                                properties.Add(key, rawProperty.RawData);
                                break;
                        }
                    }
                }
            }

            // Process textures
            foreach (TextureType textureType in Enum.GetValues(typeof(TextureType)))
            {
                if (textureType != TextureType.None)
                {
                    var textures = rawMaterial.GetTextures(textureType);
                    if (textures != null)
                    {
                        var materialTextures = new List<ModelData.MaterialTexture>();
                        material.Textures.Add(string.Format("{0}Texture", textureType), materialTextures);

                        foreach (var textureSlot in textures)
                        {
                            var textureFilePath = textureSlot.FilePath.Replace(@"/", @"\");
                            var textureWinFilePath = textureFilePath;

                            // Remove any .\
                            while (textureFilePath.StartsWith(@".\"))
                            {
                                textureFilePath = textureFilePath.Substring(2);
                            }

                            var newTextureSlot = new ModelData.MaterialTexture()
                                                     {
                                                         FilePath = textureFilePath,
                                                         BlendFactor = textureSlot.BlendFactor,
                                                         Operation = (MaterialTextureOperator)textureSlot.Operation,
                                                         Index = (int)textureSlot.TextureIndex,
                                                         UVIndex = (int)textureSlot.UVIndex,
                                                         WrapMode = ConvertWrapMode(textureSlot.WrapMode),
                                                         Flags = (MaterialTextureFlags)textureSlot.Flags
                                                     };

                            var pathToFullTexture = Path.Combine(modelDirectory, textureWinFilePath);
                            if (!File.Exists(pathToFullTexture))
                            {
                                logger.Warning(string.Format("Texture [{0}] not found from path [{1}] for model [{2}]. Skip it", textureSlot.FilePath, pathToFullTexture, modelFilePath));
                            }

                            materialTextures.Add(newTextureSlot);
                        }
                    }
                }
            }

            return material;
        }

        private void CollectSkinnedBones()
        {
            foreach (var mesh in scene.Meshes)
            {
                if (mesh.HasBones)
                {
                    foreach (var bone in mesh.Bones)
                    {
                        RegisterNode(scene.RootNode.FindNode(bone.Name), skinnedBones);
                    }
                }
            }
        }

        private void CollectMeshNodes(Node node)
        {
            if (node.HasMeshes)
            {
                RegisterNode(node, meshNodes);
            }

            if (node.HasChildren)
            {
                foreach (var child in node.Children)
                {
                    CollectMeshNodes(child);
                }
            }
        }

        private bool IsModelNode(Node node)
        {
            // Disable Skinned bones for this version
            //return meshNodes.ContainsKey(node) || skinnedBones.ContainsKey(node);
            return meshNodes.ContainsKey(node);
        }

        private void RegisterNode(Node node, Dictionary<Node,int> nodeMap)
        {
            while (node != null)
            {
                if (!nodeMap.ContainsKey(node))
                {
                    nodeMap.Add(node, 0);
                }
                else
                {
                    break;
                }

                node = node.Parent;
            }
        }
        
        private void CollectNodes(Node node, ModelData.Bone targetParent)
        {
            bool isModelNode = IsModelNode(node);

            if (!isModelNode)
            {
                return;
            }

            var parent = new ModelData.Bone
                            {
                                Index = model.Bones.Count,
                                Name = node.Name,
                                ParentIndex = targetParent == null ? -1 : targetParent.Index,
                                Transform = ConvertMatrix(node.Transform),
                                Children = new List<int>()
                            };

            if (targetParent!= null && targetParent.Children != null)
            {
                targetParent.Children.Add(parent.Index);
            }

            // Associate created bones with local index
            meshNodes[node] = model.Bones.Count;
            model.Bones.Add(parent);

            // if node has meshes, create a new scene object for it
            if( node.MeshCount > 0)
            {
                var mesh = new ModelData.Mesh
                               {
                                   Name = parent.Name, 
                                   ParentBoneIndex = parent.Index, 
                                   MeshParts = new List<ModelData.MeshPart>()
                               };
                model.Meshes.Add(mesh);

                // Precalculate the number of vertices for bounding sphere calculation
                boundingPointCount = 0;
                for (int i = 0; i < node.MeshCount; i++)
                {
                    var meshIndex = node.MeshIndices[i];
                    var meshPart = scene.Meshes[meshIndex];
                    boundingPointCount += meshPart.VertexCount;
                }

                // Reallocate the buffer if needed
                if (boundingPoints == null || boundingPoints.Length < boundingPointCount)
                {
                    boundingPoints = new Vector3[boundingPointCount];
                }

                currentBoundingPointIndex = 0;
                for (int i = 0; i < node.MeshCount; i++)
                {
                    var meshIndex = node.MeshIndices[i];
                    var meshPart = Process(mesh, scene.Meshes[meshIndex]);

                    var meshToPartList = registeredMeshParts[meshIndex];
                    if (meshToPartList == null)
                    {
                        meshToPartList = new List<ModelData.MeshPart>();
                        registeredMeshParts[meshIndex] = meshToPartList;
                    }

                    meshToPartList.Add(meshPart);
                    mesh.MeshParts.Add(meshPart);
                }

                // Calculate the bounding sphere.
                BoundingSphere.FromPoints(boundingPoints, 0, boundingPointCount, out mesh.BoundingSphere);
            } 

            // continue for all child nodes
            if (node.HasChildren)
            {
                foreach (var subNode in node.Children)
                {
                    CollectNodes(subNode, parent);
                }
            }
        }

        private Vector3[] boundingPoints;
        private int currentBoundingPointIndex;
        private int boundingPointCount;



        private ModelData.MeshPart Process(ModelData.Mesh mesh, Assimp.Mesh assimpMesh)
        {
            var meshPart = new ModelData.MeshPart()
                               {
                                   MaterialIndex = assimpMesh.MaterialIndex,
                                   VertexBufferRange = new ModelData.BufferRange() { Slot = mesh.VertexBuffers.Count },
                                   IndexBufferRange = new ModelData.BufferRange() { Slot = mesh.IndexBuffers.Count }
                               };

            var vertexBuffer = new ModelData.VertexBuffer()
            {
                Layout = new List<VertexElement>()
            };
            mesh.VertexBuffers.Add(vertexBuffer);

            var indexBuffer = new ModelData.IndexBuffer();
            mesh.IndexBuffers.Add(indexBuffer);

            var layout = vertexBuffer.Layout;

            int vertexBufferElementSize = 0;

            // Add position
            layout.Add(VertexElement.PositionTransformed(Format.R32G32B32_Float, 0));
            vertexBufferElementSize += Utilities.SizeOf<SharpDX.Vector3>();

            // Add normals
            if (assimpMesh.HasNormals)
            {
                layout.Add(VertexElement.Normal(0, Format.R32G32B32_Float, vertexBufferElementSize));
                vertexBufferElementSize += Utilities.SizeOf<SharpDX.Vector3>();
            }

            // Add colors
            if (assimpMesh.VertexColorChannelCount > 0)
            {
                for (int localIndex = 0, i = 0; i < assimpMesh.VertexColorChannelCount; i++)
                {
                    if (assimpMesh.HasVertexColors(i))
                    {
                        layout.Add(VertexElement.Color(localIndex, Format.R32G32B32A32_Float, vertexBufferElementSize));
                        vertexBufferElementSize += Utilities.SizeOf<SharpDX.Color4>();
                        localIndex++;
                    }
                }
            }

            // Add textures
            if (assimpMesh.TextureCoordsChannelCount > 0)
            {
                for (int localIndex = 0, i = 0; i < assimpMesh.TextureCoordsChannelCount; i++)
                {
                    if (assimpMesh.HasTextureCoords(i))
                    {
                        var uvCount = assimpMesh.GetUVComponentCount(i);

                        if (uvCount == 2)
                        {
                            layout.Add(VertexElement.TextureCoordinate(localIndex, Format.R32G32_Float, vertexBufferElementSize));
                            vertexBufferElementSize += Utilities.SizeOf<SharpDX.Vector2>();
                        }
                        else if (uvCount == 3)
                        {
                            layout.Add(VertexElement.TextureCoordinate(localIndex, Format.R32G32B32_Float, vertexBufferElementSize));
                            vertexBufferElementSize += Utilities.SizeOf<SharpDX.Vector3>();
                        }
                        else
                        {
                            throw new InvalidOperationException("Unexpected uv count");
                        }

                        localIndex++;
                    }
                }
            }

            // Add tangent / bitangent
            if (assimpMesh.HasTangentBasis)
            {
                layout.Add(VertexElement.Tangent(Format.R32G32B32_Float, vertexBufferElementSize));
                vertexBufferElementSize += Utilities.SizeOf<SharpDX.Vector3>();

                layout.Add(VertexElement.BiTangent(Format.R32G32B32_Float, vertexBufferElementSize));
                vertexBufferElementSize += Utilities.SizeOf<SharpDX.Vector3>();
            }

            // Extract Skinning Indices / Weights
            bool hasWeights = false;
            var skinningCount   = new int[assimpMesh.VertexCount];
            var skinningIndices = new Int4[assimpMesh.VertexCount];
            var skinningWeights = new Vector4[assimpMesh.VertexCount];

            if (assimpMesh.HasBones)
            {
                meshPart.BoneOffsetMatrices = new Matrix[assimpMesh.BoneCount];
                for (int i = 0; i < assimpMesh.Bones.Length; i++)
                {
                    var bone = assimpMesh.Bones[i];
                    meshPart.BoneOffsetMatrices[i] = ConvertMatrix(bone.OffsetMatrix);
                    if (bone.HasVertexWeights)
                    {
                        var boneNode = scene.RootNode.FindNode(bone.Name);
                        var boneIndex = skinnedBones[boneNode];
                        for (int j = 0; j < bone.VertexWeightCount; j++)
                        {
                            var weights = bone.VertexWeights[j];
                            var vertexSkinningCount = skinningCount[weights.VertexID];

                            skinningIndices[weights.VertexID][vertexSkinningCount] = boneIndex;

                            skinningWeights[weights.VertexID][vertexSkinningCount] = weights.Weight;

                            skinningCount[weights.VertexID] = ++vertexSkinningCount;
                        }

                        hasWeights = true;
                    }
                }

                if (hasWeights)
                {
                    layout.Add(VertexElement.BlendIndices(Format.R16G16B16A16_SInt, vertexBufferElementSize));
                    vertexBufferElementSize += 8;

                    layout.Add(VertexElement.BlendWeights(Format.R32G32B32A32_Float, vertexBufferElementSize));
                    vertexBufferElementSize += Utilities.SizeOf<SharpDX.Vector4>();
                }
            }

            // Write all vertices
            meshPart.VertexBufferRange.Count = assimpMesh.VertexCount;
            vertexBuffer.Count = assimpMesh.VertexCount;
            vertexBuffer.Buffer = new byte[vertexBufferElementSize * assimpMesh.VertexCount];

            // Update the MaximumBufferSizeInBytes needed to load this model
            if (vertexBuffer.Buffer.Length > model.MaximumBufferSizeInBytes)
            {
                model.MaximumBufferSizeInBytes = vertexBuffer.Buffer.Length;
            }

            var vertexStream = DataStream.Create(vertexBuffer.Buffer, true, true);
            for (int i = 0; i < assimpMesh.VertexCount; i++)
            {
                var position = assimpMesh.Vertices[i];
                vertexStream.Write(position);

                // Store bounding points for BoundingSphere pre-calculation
                boundingPoints[currentBoundingPointIndex++] = new Vector3(position.X, position.Y, position.Z);

                // Add normals
                if (assimpMesh.HasNormals)
                {
                    vertexStream.Write(assimpMesh.Normals[i]);
                }

                // Add colors
                if (assimpMesh.VertexColorChannelCount > 0)
                {
                    for (int j = 0; j < assimpMesh.VertexColorChannelCount; j++)
                    {
                        if (assimpMesh.HasVertexColors(j))
                        {
                            vertexStream.Write(assimpMesh.GetVertexColors(j)[i]);
                        }
                    }
                }

                // Add textures
                if (assimpMesh.TextureCoordsChannelCount > 0)
                {
                    for (int j = 0; j < assimpMesh.TextureCoordsChannelCount; j++)
                    {
                        if (assimpMesh.HasTextureCoords(j))
                        {
                            var uvCount = assimpMesh.GetUVComponentCount(j);

                            var uv = assimpMesh.GetTextureCoords(j)[i];

                            if (uvCount == 2)
                            {
                                vertexStream.Write(new Vector2(uv.X, uv.Y));
                            }
                            else
                            {
                                vertexStream.Write(uv);
                            }
                        }
                    }
                }

                // Add tangent / bitangent
                if (assimpMesh.HasTangentBasis)
                {
                    vertexStream.Write(assimpMesh.Tangents[i]);
                    vertexStream.Write(assimpMesh.BiTangents[i]);
                }

                // Add Skinning Indices/Weights
                if (assimpMesh.HasBones && hasWeights)
                {
                    var vertexSkinndingIndices = skinningIndices[i];
                    vertexStream.Write((short)vertexSkinndingIndices.X);
                    vertexStream.Write((short)vertexSkinndingIndices.Y);
                    vertexStream.Write((short)vertexSkinndingIndices.Z);
                    vertexStream.Write((short)vertexSkinndingIndices.W);
                    vertexStream.Write(skinningWeights[i]);
                }
            }
            vertexStream.Dispose();

            // Write all indices
            var indices = assimpMesh.GetIntIndices();
            indexBuffer.Count = indices.Length;
            meshPart.IndexBufferRange.Count = indices.Length;
            if (meshPart.VertexBufferRange.Count < 65536)
            {
                // Write only short indices if count is less than the size of a short
                indexBuffer.Buffer = new byte[indices.Length * 2];
                using (var indexStream = DataStream.Create(indexBuffer.Buffer, true, true))
                    foreach (int index in indices) indexStream.Write((ushort)index);
            }
            else
            {
                // Otherwise, use full 32-bit precision to store indices
                indexBuffer.Buffer = new byte[indices.Length * 4];
                using (var indexStream = DataStream.Create(indexBuffer.Buffer, true, true))
                    indexStream.WriteRange(indices);
            }

            // Update the MaximumBufferSizeInBytes needed to load this model
            if (indexBuffer.Buffer.Length > model.MaximumBufferSizeInBytes)
            {
                model.MaximumBufferSizeInBytes = indexBuffer.Buffer.Length;
            }

            return meshPart;
        }

        private unsafe static SharpDX.Vector2 ConvertVector(Vector2D value)
        {
            return *(SharpDX.Vector2*)&value;
        }

        private unsafe static SharpDX.Vector3 ConvertVector(Vector3D value)
        {
            return *(SharpDX.Vector3*)&value;
        }

        private unsafe static SharpDX.Color4 ConvertColor(Color4D value)
        {
            return *(SharpDX.Color4*)&value;
        }

        private unsafe static Matrix ConvertMatrix(Matrix4x4 sourceMatrix)
        {
            // Assimp uses column vectors
            return Matrix.Transpose(*(Matrix*)&sourceMatrix);
        }

        private static SharpDX.Quaternion ConvertQuaternion(Assimp.Quaternion value)
        {
            // Assimp quaternions are stored in wxyz order
            return new SharpDX.Quaternion(value.X, value.Y, value.Z, value.W);
        }

        private TextureAddressMode ConvertWrapMode(TextureWrapMode wrapMode)
        {
            switch (wrapMode)
            {
                case TextureWrapMode.Clamp:
                    return TextureAddressMode.Clamp;
                case TextureWrapMode.Wrap:
                    return TextureAddressMode.Wrap;
                case TextureWrapMode.Mirror:
                    return TextureAddressMode.Mirror;
                default:
                    return TextureAddressMode.Border;
            }
        }
    }
}