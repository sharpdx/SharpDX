using System;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using Buffer = SharpDX.Toolkit.Graphics.Buffer;

namespace StreamOutParticles
{
    /// <summary>
    /// Particle System that uses Geometry Shader and the Stream-Out stage to do
    /// as much work as possible on the GPU.
    /// Based on Microsoft's "ParticleGS" sample.
    /// </summary>
    internal class ParticleSystem : IDisposable
    {
        private const int MAX_PARTICLES = 1024 * 16;
        private const int MAX_NEW = 128;

        public const uint FLAG_CONSTRAINED = 1;
        public const uint FLAG_FAST_FADE = 2;

        private GraphicsDevice device;

        private Buffer<ParticleVertex> particleStart;
        private Buffer<ParticleVertex> particleDrawFrom;
        private Buffer<ParticleVertex> particleStreamTo;
        private VertexInputLayout layout;
        private Texture2D texture;

        private Effect effect;
        private EffectParameter viewParameter;
        private EffectParameter projParameter;
        private EffectParameter lookAtMatrixParameter;
        private EffectParameter elapsedSecondsParameter;
        private EffectParameter gravityParameter;
        private EffectParameter camDirParameter;
        private EffectParameter textureParameter;
        private EffectParameter samplerParameter;
        private EffectPass updatePass;
        private EffectPass renderPass;
        
        private ParticleVertex[] newParticles = new ParticleVertex[MAX_NEW];
        private int numNew = 0;

        private float elapsed;

        public ParticleSystem(GraphicsDevice device, ContentManager content)
        {
            this.device = device;

            // Create vertex buffer used to spawn new particles
            this.particleStart = Buffer.Vertex.New<ParticleVertex>(device, MAX_NEW);

            // Create vertex buffers to use for updating and drawing the particles alternatively
            var vbFlags = BufferFlags.VertexBuffer | BufferFlags.StreamOutput;
            this.particleDrawFrom = Buffer.New<ParticleVertex>(device, MAX_PARTICLES, vbFlags);
            this.particleStreamTo = Buffer.New<ParticleVertex>(device, MAX_PARTICLES, vbFlags);

            this.layout = VertexInputLayout.FromBuffer(0, this.particleStreamTo);
            this.effect = content.Load<Effect>("ParticleEffect");
            this.texture = content.Load<Texture2D>("Dot");

            this.viewParameter = effect.Parameters["_view"];
            this.projParameter = effect.Parameters["_proj"];
            this.lookAtMatrixParameter = effect.Parameters["_lookAtMatrix"];
            this.elapsedSecondsParameter = effect.Parameters["_elapsedSeconds"];
            this.camDirParameter = effect.Parameters["_camDir"];
            this.gravityParameter = effect.Parameters["_gravity"];
            this.textureParameter = effect.Parameters["_texture"];
            this.samplerParameter = effect.Parameters["_sampler"];
            this.updatePass = effect.Techniques["UpdateTeq"].Passes[0];
            this.renderPass = effect.Techniques["RenderTeq"].Passes[0];
        }

        public void Dispose()
        {
            this.particleStart.Dispose();
            this.particleDrawFrom.Dispose();
            this.particleStreamTo.Dispose();
        }

        public void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(Vector3 gravity, Vector3 cameraForward, Vector3 up, Matrix view, Matrix proj)
        {
            // Normalize vector and create a billboard matrix that is used in the geometry shader
            // to expand a single particle vertex into four to form a quad.
            cameraForward = Vector3.Normalize(cameraForward);
            var lookAtMatrix = Matrix.Billboard(Vector3.Zero, -cameraForward, up, cameraForward);

            this.viewParameter.SetValue(view);
            this.projParameter.SetValue(proj);
            this.lookAtMatrixParameter.SetValue(lookAtMatrix);
            this.elapsedSecondsParameter.SetValue(elapsed);
            this.camDirParameter.SetValue(cameraForward);
            this.gravityParameter.SetValue(gravity);
            this.textureParameter.SetResource(this.texture);
            this.samplerParameter.SetResource(this.device.SamplerStates.LinearClamp);
            this.elapsed = 0;

            this.Advance();
            this.Render();
        }

        private void Advance()
        {
            // Use particleDrawFrom as input and stream output to particleStreamTo
            this.device.SetVertexBuffer(this.particleDrawFrom);
            this.device.SetVertexInputLayout(this.layout);
            this.device.SetStreamOutputTarget(this.particleStreamTo, 0);

            // Update particles on the gpu
            this.updatePass.Apply();
            this.device.DrawAuto(PrimitiveType.PointList);

            // See if we have any newly-spawned particles waiting
            if (numNew > 0)
            {
                // Copy new particles to vertex buffer and draw them to append them to stream output target
                this.particleStart.SetData(this.newParticles, 0, this.numNew);
                this.device.SetVertexBuffer(this.particleStart);
                this.device.Draw(PrimitiveType.PointList, numNew);
                numNew = 0;
            }

            this.device.ResetStreamOutputTargets();

            // Swap vertex buffers
            Utilities.Swap(ref this.particleDrawFrom, ref this.particleStreamTo);
        }

        private void Render()
        {
            // Since we just swapped the vertex buffers, particleDrawFrom contains the current state
            this.device.SetVertexBuffer(this.particleDrawFrom);
            this.device.SetVertexInputLayout(this.layout);
            this.device.SetBlendState(this.device.BlendStates.Additive);
            this.device.SetDepthStencilState(this.device.DepthStencilStates.None);

            // Actually draw the particles
            this.renderPass.Apply();
            this.device.DrawAuto(PrimitiveType.PointList);
        }

        /// <summary>
        /// Creates a new particle.
        /// </summary>
        /// <param name="position">The position to spawn the particle at.</param>
        /// <param name="velocity">The initial velocity of the particle.</param>
        /// <param name="color">A color to multiply the particle's texture by.</param>
        /// <param name="sizeStart">The initial size of the particle.</param>
        /// <param name="sizeEnd">The size of the particle just before dying.</param>
        /// <param name="lifetime">The total lifetime of the particle in seconds.</param>
        /// <param name="constrained">Set to true to constrain one of the axes used to create the quad to the particle's velocity.</param>
        /// <param name="fastFade">Set to true to fade the particle in and out more quickly.</param>
        public void Spawn(Vector3 position, Vector3 velocity, Color color, float sizeStart, float sizeEnd, float lifetime, bool constrained, bool fastFade)
        {
            // discard particle if buffer is full
            if (numNew >= MAX_NEW)
                return;

            // create particle struct
            ParticleVertex v = new ParticleVertex();
            v.Position = position;
            v.Velocity = velocity;
            v.TimerLifetime = new Vector2(0, lifetime);
            v.SizeStartEnd = new Vector2(sizeStart, sizeEnd);
            v.Color = color.ToVector4();

            // set the particle's flags
            if (constrained) v.Flags |= FLAG_CONSTRAINED;
            if (fastFade) v.Flags |= FLAG_FAST_FADE;

            // append to buffer
            this.newParticles[numNew++] = v;
        }
    }
}