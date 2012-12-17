/*
* Copyright (c) 2012 Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using Assimp.Unmanaged;

namespace Assimp {
    /// <summary>
    /// Represents a completely imported model or scene. Everything that was imported from the given file can be
    /// accessed from here. Once the scene is loaded from unmanaged memory, it resides solely in managed memory
    /// and Assimp's read only copy is released.
    /// </summary>
    internal sealed class Scene {
        private SceneFlags _flags;
        private Node _rootNode;
        private Mesh[] _meshes;
        private Light[] _lights;
        private Camera[] _cameras;
        private Texture[] _textures;
        private Animation[] _animations;
        private Material[] _materials;

        /// <summary>
        /// Gets the state of the imported scene. By default no flags are set, but
        /// issues can arise if the flag is set to incomplete.
        /// </summary>
        public SceneFlags SceneFlags {
            get {
                return _flags;
            }
        }

        /// <summary>
        /// Gets the root node of the scene graph. There will always be at least the root node
        /// if the import was successful and no special flags have been set. Presence of further nodes
        /// depends on the format and content of the imported file.
        /// </summary>
        public Node RootNode {
            get {
                return _rootNode;
            }
        }

        /// <summary>
        /// Gets the number of meshes in the scene.
        /// </summary>
        public int MeshCount {
            get {
                return (_meshes == null) ? 0 : _meshes.Length;
            }
        }

        /// <summary>
        /// Checks if the scene contains meshes. Unless if no special scene flags are set
        /// this should always be true.
        /// </summary>
        public bool HasMeshes {
            get {
                return _meshes != null;
            }
        }

        /// <summary>
        /// Gets the meshes contained in the scene, if any.
        /// </summary>
        public Mesh[] Meshes {
            get {
                return _meshes;
            }
        }

        /// <summary>
        /// Gets the number of lights in the scene.
        /// </summary>
        public int LightCount {
            get {
                return (_lights == null) ? 0 : _lights.Length;
            }
        }

        /// <summary>
        /// Checks if the scene contains any lights.
        /// </summary>
        public bool HasLights {
            get {
                return _lights != null;
            }
        }

        /// <summary>
        /// Gets the lights in the scene, if any.
        /// </summary>
        public Light[] Lights {
            get {
                return _lights;
            }
        }

        /// <summary>
        /// Gets the number of cameras in the scene.
        /// </summary>
        public int CameraCount {
            get {
                return (_cameras == null) ? 0 : _cameras.Length;
            }
        }

        /// <summary>
        /// Checks if the scene contains any cameras.
        /// </summary>
        public bool HasCameras {
            get {
                return _cameras != null;
            }
        }

        /// <summary>
        /// Gets the cameras in the scene, if any.
        /// </summary>
        public Camera[] Cameras {
            get {
                return _cameras;
            }
        }

        /// <summary>
        /// Gets the number of embedded textures in the scene.
        /// </summary>
        public int TextureCount {
            get {
                return (_textures == null) ? 0 : _textures.Length;
            }
        }

        /// <summary>
        /// Checks if the scene contains embedded textures.
        /// </summary>
        public bool HasTextures {
            get {
                return _textures != null;
            }
        }

        /// <summary>
        /// Gets the embedded textures in the scene, if any.
        /// </summary>
        public Texture[] Textures {
            get {
                return _textures;
            }
        }

        /// <summary>
        /// Gets the number of animations in the scene.
        /// </summary>
        public int AnimationCount {
            get {
                return (_animations == null) ? 0 : _animations.Length;
            }
        }

        /// <summary>
        /// Checks if the scene contains any animations.
        /// </summary>
        public bool HasAnimations {
            get {
                return _animations != null;
            }
        }

        /// <summary>
        /// Gets the animations in the scene, if any.
        /// </summary>
        public Animation[] Animations {
            get {
                return _animations;
            }
        }

        /// <summary>
        /// Gets the number of materials in the scene. There should always be at least the
        /// default Assimp material if no materials were loaded.
        /// </summary>
        public int MaterialCount {
            get {
                return (_materials == null) ? 0 : _materials.Length;
            }
        }

        /// <summary>
        /// Checks if the scene contains any materials. There should always be at least the
        /// default Assimp material if no materials were loaded.
        /// </summary>
        public bool HasMaterials {
            get {
                return _materials != null;
            }
        }

        /// <summary>
        /// Gets the materials in the scene.
        /// </summary>
        public Material[] Materials {
            get {
                return _materials;
            }
        }

        /// <summary>
        /// Constructs a new Scene.
        /// </summary>
        /// <param name="scene">Unmanaged AiScene struct.</param>
        internal Scene(AiScene scene) {
            _flags = scene.Flags;

            //Read materials
            if(scene.NumMaterials > 0 && scene.Materials != IntPtr.Zero) {
                AiMaterial[] materials = MemoryHelper.MarshalArray<AiMaterial>(scene.Materials, (int) scene.NumMaterials, true);
                _materials = new Material[materials.Length];
                for(int i = 0; i < _materials.Length; i++) {
                    _materials[i] = new Material(materials[i]);
                }
            }

            //Read scenegraph
            if(scene.RootNode != IntPtr.Zero) {
                _rootNode = new Node(MemoryHelper.MarshalStructure<AiNode>(scene.RootNode), null);
            }

            //Read meshes
            if(scene.NumMeshes > 0 && scene.Meshes != IntPtr.Zero) {
                AiMesh[] meshes = MemoryHelper.MarshalArray<AiMesh>(scene.Meshes, (int) scene.NumMeshes, true);
                _meshes = new Mesh[meshes.Length];
                for(int i = 0; i < _meshes.Length; i++) {
                    _meshes[i] = new Mesh(meshes[i]);
                }
            }

            //Read lights
            if(scene.NumLights > 0 && scene.Lights != IntPtr.Zero) {
                AiLight[] lights = MemoryHelper.MarshalArray<AiLight>(scene.Lights, (int) scene.NumLights, true);
                _lights = new Light[lights.Length];
                for(int i = 0; i < _lights.Length; i++) {
                    _lights[i] = new Light(lights[i]);
                }
            }

            //Read cameras
            if(scene.NumCameras > 0 && scene.Cameras != IntPtr.Zero) {
                AiCamera[] cameras = MemoryHelper.MarshalArray<AiCamera>(scene.Cameras, (int) scene.NumCameras, true);
                _cameras = new Camera[cameras.Length];
                for(int i = 0; i < _cameras.Length; i++) {
                    _cameras[i] = new Camera(cameras[i]);
                }
            }

            //Read Textures
            if(scene.NumTextures > 0 && scene.Textures != IntPtr.Zero) {
                AiTexture[] textures = MemoryHelper.MarshalArray<AiTexture>(scene.Textures, (int) scene.NumTextures, true);
                _textures = new Texture[textures.Length];
                for(int i = 0; i < _textures.Length; i++) {
                    _textures[i] = Texture.CreateTexture(textures[i]);
                }
            }

            //Read animations
            if(scene.NumAnimations > 0 && scene.Animations != IntPtr.Zero) {
                AiAnimation[] animations = MemoryHelper.MarshalArray<AiAnimation>(scene.Animations, (int) scene.NumAnimations, true);
                _animations = new Animation[animations.Length];
                for(int i = 0; i < _animations.Length; i++) {
                    _animations[i] = new Animation(animations[i]);
                }
            }
        }
    }
}
