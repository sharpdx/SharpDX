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
    /// Describes a light source in the scene. Assimp supports multiple light sources
    /// including spot, point, and directional lights. All are defined by a single structure
    /// and distinguished by their parameters. Lights have corresponding nodes in the scenegraph.
    /// <para>Some file formats such as 3DS and ASE export a "target point", e.g. the point
    /// a spot light is looking at (it can even be animated). Assimp writes the target point as a subnode
    /// of a spotlight's main node called "spotName.Target". However, this is just additional information
    /// then, the transform tracks of the main node make the spot light already point in the right direction.</para>
    /// </summary>
    internal sealed class Light {
        private String _name;
        private LightSourceType _lightType;
        private float _angleInnerCone;
        private float _angleOuterCone;
        private float _attConstant;
        private float _attLinear;
        private float _attQuadratic;
        private Vector3D _position;
        private Vector3D _direction;
        private Color3D _diffuse;
        private Color3D _specular;
        private Color3D _ambient;

        /// <summary>
        /// Gets the name of the light source. This corresponds to a node present in the scenegraph.
        /// </summary>
        public String Name {
            get {
                return _name;
            }
        }

        /// <summary>
        /// Gets the type of light source. This should never be undefined.
        /// </summary>
        public LightSourceType LightType {
            get {
                return _lightType;
            }
        }

        /// <summary>
        /// Gets the inner angle of a spot light's light cone. The spot light has
        /// maximum influence on objects inside this angle. The angle is given in radians, it
        /// is 2PI for point lights and defined for directional lights.
        /// </summary>
        public float AngleInnerCone {
            get {
                return _angleInnerCone;
            }
        }

        /// <summary>
        /// Gets the outer angle of a spot light's light cone. The spot light does not affect objects outside
        /// this angle. The angle is given in radians. It is 2PI for point lights and undefined for
        /// directional lights. The outer angle must be greater than or equal to the inner angle.
        /// </summary>
        public float AngleOuterCone {
            get {
                return _angleOuterCone;
            }
        }

        /// <summary>
        /// Gets the constant light attenuation factor. The intensity of the light source
        /// at a given distance 'd' from the light position is <code>Atten = 1 / (att0 + att1 * d + att2 * d*d)</code>.
        /// <para>This member corresponds to the att0 variable in the equation and is undefined for directional lights.</para>
        /// </summary>
        public float AttenuationConstant {
            get {
                return _attConstant;
            }
        }

        /// <summary>
        /// Gets the linear light attenuation factor. The intensity of the light source
        /// at a given distance 'd' from the light position is <code>Atten = 1 / (att0 + att1 * d + att2 * d*d)</code>
        /// <para>This member corresponds to the att1 variable in the equation and is undefined for directional lights.</para>
        /// </summary>
        public float AttenuationLinear {
            get {
                return _attLinear;
            }
        }

        /// <summary>
        /// Gets the quadratic light attenuation factor. The intensity of the light source
        /// at a given distance 'd' from the light position is <code>Atten = 1 / (att0 + att1 * d + att2 * d*d)</code>.
        /// <para>This member corresponds to the att2 variable in the equation and is undefined for directional lights.</para>
        /// </summary>
        public float AttenuationQuadratic {
            get {
                return _attQuadratic;
            }
        }

        /// <summary>
        /// Gets the position of the light source in space, relative to the
        /// transformation of the node corresponding to the light. This is undefined for
        /// directional lights.
        /// </summary>
        public Vector3D Position {
            get {
                return _position;
            }
        }

        /// <summary>
        /// Gets the direction of the light source in space, relative to the transformation
        /// of the node corresponding to the light. This is undefined for point lights.
        /// </summary>
        public Vector3D Direction {
            get {
                return _direction;
            }
        }

        /// <summary>
        /// Gets the diffuse color of the light source.  The diffuse light color is multiplied with
        /// the diffuse material color to obtain the final color that contributes to the diffuse shading term.
        /// </summary>
        public Color3D ColorDiffuse {
            get {
                return _diffuse;
            }
        }

        /// <summary>
        /// Gets the specular color of the light source. The specular light color is multiplied with the
        /// specular material color to obtain the final color that contributes to the specular shading term.
        /// </summary>
        public Color3D ColorSpecular {
            get {
                return _specular;
            }
        }

        /// <summary>
        /// Gets the ambient color of the light source. The ambient light color is multiplied with the ambient
        /// material color to obtain the final color that contributes to the ambient shading term.
        /// </summary>
        public Color3D ColorAmbient {
            get {
                return _ambient;
            }
        }

        /// <summary>
        /// Constructs a new Light.
        /// </summary>
        /// <param name="light">Unmanaged AiLight struct</param>
        internal Light(AiLight light) {
            _name = light.Name.GetString();
            _lightType = light.Type;
            _angleInnerCone = light.AngleInnerCone;
            _angleOuterCone = light.AngleOuterCone;
            _attConstant = light.AttenuationConstant;
            _attLinear = light.AttenuationLinear;
            _attQuadratic = light.AttenuationQuadratic;
            _position = light.Position;
            _direction = light.Direction;
            _diffuse = light.ColorDiffuse;
            _specular = light.ColorSpecular;
            _ambient = light.ColorAmbient;
        }
    }
}
