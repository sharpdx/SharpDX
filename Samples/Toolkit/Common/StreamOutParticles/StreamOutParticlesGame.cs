using System;

using SharpDX;
using SharpDX.Toolkit;

namespace StreamOutParticles
{
    /// <summary>
    /// A simple application to demonstrate a GPU-based particle system.
    /// </summary>
    public class StreamOutParticlesGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private ParticleSystem particleSystem;
        private Random rnd = new Random();

        public StreamOutParticlesGame()
        {
            this.graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            // Create the particle system
            this.particleSystem = new ParticleSystem(this.GraphicsDevice, this.Content);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            // Dispose the particle system
            this.particleSystem.Dispose();

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // Spawn 128 new particles with a random velocity and color
            for (int i = 0; i < 128; i++)
            {
                var position = Vector3.Zero;
                var velocity = RandomDirection(Vector3.UnitY, MathUtil.TwoPi) * (float)rnd.NextDouble(1.0, 4.0);
                var color = new Color((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());
                var startSize = 0.02f;
                var endSize = 0.07f;
                var lifetime = 2f;

                this.particleSystem.Spawn(Vector3.Zero, velocity, color, startSize, endSize, lifetime, false, false);
            }

            // Update the particle system
            this.particleSystem.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Setup camera
            var cameraTarget = Vector3.Zero;
            var cameraPosition = Vector3.UnitZ * 10;

            var view = Matrix.LookAtRH(cameraPosition, cameraTarget, Vector3.UnitY);
            var ratio = (float)this.GraphicsDevice.BackBuffer.Width / this.GraphicsDevice.BackBuffer.Height;
            var proj = Matrix.PerspectiveFovRH(MathUtil.DegreesToRadians(45), ratio, 0.1f, 20f);

            var cameraForward = Vector3.Normalize(cameraTarget - cameraPosition);
            var up = Vector3.UnitY;
            var gravity = -2 * Vector3.UnitY;

            // Draw the particle system
            this.particleSystem.Draw(gravity, cameraForward, up, view, proj);

            base.Draw(gameTime);
        }

        private Vector3 RandomDirection(Vector3 direction, float max)
        {
            float r1 = ((float)rnd.NextDouble() * 2 - 1) * max;
            float r2 = ((float)rnd.NextDouble() * 2 - 1) * max;
            float r3 = ((float)rnd.NextDouble() * 2 - 1) * max;

            Quaternion q = Quaternion.RotationYawPitchRoll(r1, r2, r3);
            return Vector3.Transform(direction, q);
        }
    }
}