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
    /// A node in the imported model hierarchy.
    /// </summary>
    internal sealed class Node {
        private String _name;
        private Matrix4x4 _transform;
        private Node _parent;
        private Node[] _children;
        private int[] _meshes;

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        public String Name {
            get {
                return _name;
            }
        }

        /// <summary>
        /// Gets the transformation of the node relative to its parent.
        /// </summary>
        public Matrix4x4 Transform {
            get {
                return _transform;
            }
        }

        /// <summary>
        /// Gets the node's parent, if it exists. 
        /// </summary>
        public Node Parent {
            get {
                return _parent;
            }
        }

        /// <summary>
        /// Gets the number of children that is owned by this node.
        /// </summary>
        public int ChildCount {
            get {
                return (_children == null) ? 0 : _children.Length;
            }
        }

        /// <summary>
        /// Gets if the node contains children.
        /// </summary>
        public bool HasChildren {
            get {
                return _children != null;
            }
        }

        /// <summary>
        /// Gets the node's children.
        /// </summary>
        public Node[] Children {
            get {
                return _children;
            }
        }

        /// <summary>
        /// Gets the number of meshes referenced by this node.
        /// </summary>
        public int MeshCount {
            get {
                return (_meshes == null) ? 0 : _meshes.Length;
            }
        }

        /// <summary>
        /// Gets if the node contains mesh references.
        /// </summary>
        public bool HasMeshes {
            get {
                return _meshes != null;
            }
        }

        /// <summary>
        /// Gets the indices of the meshes referenced by this node. Meshes can be
        /// shared between nodes, so there is a mesh collection owned by the scene
        /// that each node can reference.
        /// </summary>
        public int[] MeshIndices {
            get {
                return _meshes;
            }
        }

        /// <summary>
        /// Constructs a new Node.
        /// </summary>
        /// <param name="aiNode">Unmanaged AiNode structure</param>
        /// <param name="parent">Parent of this node or null</param>
        internal Node(AiNode aiNode, Node parent) {
            _name = aiNode.Name.GetString();
            _transform = aiNode.Transformation;
            _parent = parent;

            if(aiNode.NumChildren > 0 && aiNode.Children != IntPtr.Zero) {
                AiNode[] childNodes = MemoryHelper.MarshalArray<AiNode>(aiNode.Children, (int) aiNode.NumChildren, true);
                _children = new Node[childNodes.Length];
                for(int i = 0; i < _children.Length; i++) {
                    _children[i] = new Node(childNodes[i], this);
                }
            }

            if(aiNode.NumMeshes > 0 && aiNode.Meshes != IntPtr.Zero) {
                _meshes = MemoryHelper.MarshalArray<int>(aiNode.Meshes, (int) aiNode.NumMeshes);
            }
        }

        /// <summary>
        /// Finds a node with the specific name, which may be this node
        /// or any children or children's children, and so on, if it exists.
        /// </summary>
        /// <param name="name">Node name</param>
        /// <returns>The node or null if it does not exist</returns>
        public Node FindNode(String name) {
            if(name.Equals(_name)) {
                return this;
            }
            if(HasChildren) {
                foreach(Node child in _children) {
                    Node found = child.FindNode(name);
                    if(found != null) {
                        return found;
                    }
                }
            }
            //No child found
            return null;
        }
    }
}
