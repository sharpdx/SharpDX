namespace ShaderLinking
{
    using SharpDX;
    using SharpDX.Toolkit;
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Main game class
    /// </summary>
    internal sealed class ShaderCompilerGame : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private readonly ShaderData _data; // holds shader source, parameters and dirty state
        private readonly ShaderBuilder _builder; // builds shader, contains all linking code

        private GeometricPrimitive<Vertex> _cubeGeometry; // cube geometrc primitive, contains vertex and index buffers
        private Effect _effect; // the applied effect, rebuild by ShaderBuilder class

        // transformation matrices
        private Matrix _view;
        private Matrix _projection;
        private Matrix _world;

        public ShaderCompilerGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            _data = new ShaderData();
            _builder = new ShaderBuilder(_data);
        }

        public ShaderData Data { get { return _data; } }

        protected override void LoadContent()
        {
            base.LoadContent();

            // mark data as dirty at loading
            _data.IsDirty = true;

            // initialize transformation matrices
            _view = Matrix.LookAtRH(new Vector3(0, 1.2f, 3f), new Vector3(0, 0, 0), new Vector3(0f, 1f, 0f));
            _projection = Matrix.PerspectiveFovRH(35f * MathUtil.Pi / 180f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 10.0f);
            _world = Matrix.RotationY(45f * MathUtil.Pi / 180f);

            // load geometry
            LoadGeometry();
        }

        private void LoadGeometry()
        {
            // constants for easy manipulation
            const float f = 0.5f;
            const float c = 0.75f;

            var vertices = new[]
                           {
                               new Vertex(new Vector3(f, f, f), new Vector3(c, 0f, 0f)),
                               new Vertex(new Vector3(f, f, -f), new Vector3(0, c, 0f)),
                               new Vertex(new Vector3(f, -f, f), new Vector3(0f, 0f, c)),
                               new Vertex(new Vector3(f, -f, -f), new Vector3(c, c, 0f)),

                               new Vertex(new Vector3(-f, f, f), new Vector3(c, 0f, c)),
                               new Vertex(new Vector3(-f, f, -f), new Vector3(0f, c, c)),
                               new Vertex(new Vector3(-f, -f, f), new Vector3(0f, 0f, 0f)),
                               new Vertex(new Vector3(-f, -f, -f), new Vector3(c, c, c))
                           };

            var indices = new short[]
                          {
                              0,2,1, 1,2,3,
                              1,3,7, 7,5,1,

                              5,7,6, 5,6,4,
                              0,6,2, 0,4,6,

                              0,1,5, 0,5,4,
                              3,7,6, 3,6,2
                          };

            _cubeGeometry = ToDisposeContent(new GeometricPrimitive<Vertex>(GraphicsDevice, vertices, indices, false));
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // check if data needs to be recreated
            if (_data.IsDirty)
            {
                // dispose old effect
                Utilities.Dispose(ref _effect);

                // rebuild the effect
                var effectData = _builder.Rebuild();
                if (effectData != null)
                    // instantiate new effect from the built data
                    _effect = ToDispose(new Effect(GraphicsDevice, effectData));
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            // if the effect is loaded...
            if (_effect != null)
            {
                // update cube rotation
                var rot = Matrix.RotationY((float)gameTime.TotalGameTime.TotalSeconds);

                // set the parameters
                _effect.Parameters[0].SetValue(rot * _world * _view * _projection);

                // draw cube
                _cubeGeometry.Draw(_effect);
            }

            base.Draw(gameTime);
        }
    }
}