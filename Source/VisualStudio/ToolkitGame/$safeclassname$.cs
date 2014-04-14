using System;
using System.Text;
using SharpDX;
$if$ ($sharpdx_platform_wp8$ == true)using SharpDX.Direct3D;$endif$

namespace $safeprojectname$
{
    // Use these namespaces here to override SharpDX.Direct3D11
    using SharpDX.Toolkit;
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    /// <summary>
    /// Simple $safeclassname$ game using SharpDX.Toolkit.
    /// </summary>
    public class $safeclassname$ : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
$if$ ($sharpdx_feature_spritebatch$ == true)        private SpriteBatch spriteBatch;
$endif$$if$ ($sharpdx_feature_spritetexture$ == true)        private Texture2D ballsTexture;
$endif$$if$ ($sharpdx_feature_spritefont$ == true)        private SpriteFont arial16Font;
$endif$$if$ ($sharpdx_feature_3d$ == true)
        private Matrix view;
        private Matrix projection;
$endif$$if$ ($sharpdx_feature_model3d$ == true)
        private Model model;
$endif$$if$ ($sharpdx_feature_bloomeffect$ == true)
        private Effect bloomEffect;
        private RenderTarget2D renderTargetOffScreen;
        private RenderTarget2D[] renderTargetDownScales;
        private RenderTarget2D renderTargetBlurTemp;
$endif$$if$ ($sharpdx_feature_primitive3d$ == true)
        private BasicEffect basicEffect;
        private GeometricPrimitive primitive;
$endif$$if$ ($sharpdx_feature_keyboard$ == true)
        private KeyboardManager keyboard;
        private KeyboardState keyboardState;
$endif$$if$ ($sharpdx_feature_mouse$ == true)
        private MouseManager mouse;
        private MouseState mouseState;
$endif$$if$ ($sharpdx_feature_pointer$ == true)
        private PointerManager pointer;
        private PointerState pointerState;
$endif$
        /// <summary>
        /// Initializes a new instance of the <see cref="$safeclassname$" /> class.
        /// </summary>
        public $safeclassname$()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);
$if$ ($sharpdx_platform_wp8$ == true)            graphicsDeviceManager.PreferredGraphicsProfile = new[] { FeatureLevel.Level_9_3 };
$endif$
            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
$if$ ($sharpdx_feature_keyboard$ == true)
            // Initialize input keyboard system
            keyboard = new KeyboardManager(this);
$endif$$if$ ($sharpdx_feature_mouse$ == true)
            // Initialize input mouse system
            mouse = new MouseManager(this);
$endif$$if$ ($sharpdx_feature_pointer$ == true)
            // Initialize input pointer system
            pointer = new PointerManager(this);
$endif$        }

        protected override void Initialize()
        {
            // Modify the title of the window
            Window.Title = "$safeclassname$";

            base.Initialize();
        }

        protected override void LoadContent()
        {
$if$ ($sharpdx_feature_spritebatch$ == true)            // Instantiate a SpriteBatch
            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));
$endif$$if$ ($sharpdx_feature_spritetexture$ == true)
            // Loads the balls texture (32 textures (32x32) stored vertically => 32 x 1024 ).
            // The [Balls.dds] file is defined with the build action [ToolkitTexture] in the project
            ballsTexture = Content.Load<Texture2D>("Balls");
$endif$$if$ ($sharpdx_feature_spritefont$ == true)
            // Loads a sprite font
            // The [Arial16.xml] file is defined with the build action [ToolkitFont] in the project
            arial16Font = Content.Load<SpriteFont>("Arial16");
$endif$$if$ ($sharpdx_feature_model3d$ == true)
            // Load a 3D model
            // The [Ship.fbx] file is defined with the build action [ToolkitModel] in the project
            model = Content.Load<Model>("Ship");
                
            // Enable default lighting on model.
            BasicEffect.EnableDefaultLighting(model, true);
$endif$$if$ ($sharpdx_feature_bloomeffect$ == true)
            // Bloom Effect
            // The [Bloom.fx] file is defined with the build action [ToolkitFxc] in the project
            bloomEffect = Content.Load<Effect>("Bloom");

            // Creates render targets for bloom effect
            renderTargetDownScales = new RenderTarget2D[5];
            var backDesc = GraphicsDevice.BackBuffer.Description;
            renderTargetOffScreen = ToDisposeContent(RenderTarget2D.New(GraphicsDevice, backDesc.Width, backDesc.Height, 1, backDesc.Format));
            for (int i = 0; i < renderTargetDownScales.Length; i++)
            {
                renderTargetDownScales[i] = ToDisposeContent(RenderTarget2D.New(GraphicsDevice, backDesc.Width >> i, backDesc.Height >> i, 1, backDesc.Format));
            }
            renderTargetBlurTemp = ToDisposeContent((RenderTarget2D)renderTargetDownScales[renderTargetDownScales.Length - 1].Clone());
$endif$$if$ ($sharpdx_feature_primitive3d$ == true)
            // Creates a basic effect
            basicEffect = ToDisposeContent(new BasicEffect(GraphicsDevice));
            basicEffect.PreferPerPixelLighting = true;
            basicEffect.EnableDefaultLighting();

            // Creates torus primitive
            primitive = ToDisposeContent(GeometricPrimitive.Torus.New(GraphicsDevice));
$endif$
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
$if$ ($sharpdx_feature_3d$ == true)
            // Calculates the world and the view based on the model size
            view = Matrix.LookAtRH(new Vector3(0.0f, 0.0f, 7.0f), new Vector3(0, 0.0f, 0), Vector3.UnitY);
            projection = Matrix.PerspectiveFovRH(0.9f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
$endif$
$if$ ($sharpdx_feature_primitive3d$ == true)            // Update basic effect for rendering the Primitive
            basicEffect.View = view;
            basicEffect.Projection = projection;
$endif$$if$ ($sharpdx_feature_keyboard$ == true)
            // Get the current state of the keyboard
            keyboardState = keyboard.GetState();
$endif$$if$ ($sharpdx_feature_mouse$ == true)
            // Get the current state of the mouse
            mouseState = mouse.GetState();
$endif$$if$ ($sharpdx_feature_pointer$ == true)
            // Get the current state of the pointer
            pointerState = pointer.GetState();
$endif$        }

        protected override void Draw(GameTime gameTime)
        {
            // Use time in seconds directly
            var time = (float)gameTime.TotalGameTime.TotalSeconds;
$if$ ($sharpdx_feature_bloomeffect$ == true)
            // Make offline rendering
            GraphicsDevice.SetRenderTargets(GraphicsDevice.DepthStencilBuffer, renderTargetOffScreen);
$endif$
            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);
$if$ ($sharpdx_feature_3d$ == true)
            // Constant used to translate 3d models
            float translateX = 0.0f;
$endif$$if$ ($sharpdx_feature_model3d$ == true)            
            // ------------------------------------------------------------------------
            // Draw the 3d model
            // ------------------------------------------------------------------------
            var world = Matrix.Scaling(0.003f) *
                        Matrix.RotationY(time) *
                        Matrix.Translation(0, -1.5f, 2.0f);
            model.Draw(GraphicsDevice, world, view, projection);
            translateX += 3.5f;
$endif$$if$ ($sharpdx_feature_primitive3d$ == true)
            // ------------------------------------------------------------------------
            // Draw the 3d primitive using BasicEffect
            // ------------------------------------------------------------------------
            basicEffect.World = Matrix.Scaling(2.0f, 2.0f, 2.0f) *
                                Matrix.RotationX(0.8f * (float)Math.Sin(time * 1.45)) *
                                Matrix.RotationY(time * 2.0f) *
                                Matrix.RotationZ(0) *
                                Matrix.Translation(translateX, -1.0f, 0);
            primitive.Draw(basicEffect);
$endif$$if$ ($sharpdx_feature_spritefont$ == true)
            // ------------------------------------------------------------------------
            // Draw the some 2d text
            // ------------------------------------------------------------------------
            spriteBatch.Begin();
            var text = new StringBuilder("This text is displayed with SpriteBatch").AppendLine();
$endif$$if$ ($sharpdx_feature_keyboard$ == true)
            // Display pressed keys
            var pressedKeys = keyboardState.GetPressedKeys();
            text.Append("Key Pressed: [");
            foreach (var key in pressedKeys)
            {
                text.Append(key.ToString());
                text.Append(" ");
            }
            text.Append("]").AppendLine();
$endif$$if$ ($sharpdx_feature_mouse$ == true)
            // Display mouse coordinates and mouse button status
            text.AppendFormat("Mouse ({0},{1}) Left: {2}, Right {3}", mouseState.X, mouseState.Y, mouseState.Left, mouseState.Right).AppendLine();
$endif$$if$ ($sharpdx_feature_pointer$ == true)
            var points = pointerState.Points;
            if (points.Count > 0)
            {
                foreach (var point in points)
                {
                    text.AppendFormat("Pointer event: [{0}] {1} {2} ({3}, {4})", point.PointerId, point.DeviceType, point.EventType, point.Position.X, point.Position.Y).AppendLine();
                }
            }
$endif$$if$ ($sharpdx_feature_spritefont$ == true)
            spriteBatch.DrawString(arial16Font, text.ToString(), new Vector2(16, 16), Color.White);
            spriteBatch.End();
$endif$$if$ ($sharpdx_feature_spritetexture$ == true)
            // ------------------------------------------------------------------------
            // Use SpriteBatch to draw some balls on the screen using NonPremultiplied mode
            // as the sprite texture used is not premultiplied
            // ------------------------------------------------------------------------
            spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);
            for (int i = 0; i < 40; i++)
            {
                var posX = (float)Math.Cos(time * 4.5f + i * 0.1f) * 60.0f + 136.0f;
                var posY = GraphicsDevice.BackBuffer.Height * 2.0f / 3.0f + 100.0f * (float)Math.Sin(time * 10.0f + i * 0.4f);

                spriteBatch.Draw(
                    ballsTexture,
                    new Vector2(posX, posY),
                    new Rectangle(0, 0, 32, 32),
                    Color.White,
                    0.0f,
                    new Vector2(16, 16),
                    Vector2.One,
                    SpriteEffects.None,
                    0f);
            }
            spriteBatch.End();
$endif$$if$ ($sharpdx_feature_bloomeffect$ == true)
            // ------------------------------------------------------------------------
            // Cheap bloom post effect
            // Blur applied only on latest downscale render target
            // ------------------------------------------------------------------------

            // Setup states for posteffect
            GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.Default);
            GraphicsDevice.SetBlendState(GraphicsDevice.BlendStates.Default);
            GraphicsDevice.SetDepthStencilState(GraphicsDevice.DepthStencilStates.None);

            // Apply BrightPass
            const float brightPassThreshold = 0.5f;
            GraphicsDevice.SetRenderTargets(renderTargetDownScales[0]);
            bloomEffect.CurrentTechnique = bloomEffect.Techniques["BrightPassTechnique"];
            bloomEffect.Parameters["Texture"].SetResource(renderTargetOffScreen);
            bloomEffect.Parameters["PointSampler"].SetResource(GraphicsDevice.SamplerStates.PointClamp);
            bloomEffect.Parameters["BrightPassThreshold"].SetValue(brightPassThreshold);
            GraphicsDevice.DrawQuad(bloomEffect.CurrentTechnique.Passes[0]);

            // Down scale passes
            for (int i = 1; i < renderTargetDownScales.Length; i++)
            {
                GraphicsDevice.SetRenderTargets(renderTargetDownScales[i]);
                GraphicsDevice.DrawQuad(renderTargetDownScales[0]);
            }

            // Horizontal blur pass
            var renderTargetBlur = renderTargetDownScales[renderTargetDownScales.Length - 1];
            GraphicsDevice.SetRenderTargets(renderTargetBlurTemp);
            bloomEffect.CurrentTechnique = bloomEffect.Techniques["BlurPassTechnique"];
            bloomEffect.Parameters["Texture"].SetResource(renderTargetBlur);
            bloomEffect.Parameters["LinearSampler"].SetResource(GraphicsDevice.SamplerStates.LinearClamp);
            bloomEffect.Parameters["TextureTexelSize"].SetValue(new Vector2(1.0f / renderTargetBlurTemp.Width, 1.0f / renderTargetBlurTemp.Height));
            GraphicsDevice.DrawQuad(bloomEffect.CurrentTechnique.Passes[0]);

            // Vertical blur pass
            GraphicsDevice.SetRenderTargets(renderTargetBlur);
            bloomEffect.Parameters["Texture"].SetResource(renderTargetBlurTemp);
            GraphicsDevice.DrawQuad(bloomEffect.CurrentTechnique.Passes[1]);

            // Render to screen
            GraphicsDevice.SetRenderTargets(GraphicsDevice.BackBuffer);
            GraphicsDevice.DrawQuad(renderTargetOffScreen);

            // Add bloom on top of it
            GraphicsDevice.SetBlendState(GraphicsDevice.BlendStates.Additive);
            GraphicsDevice.DrawQuad(renderTargetBlur);
            GraphicsDevice.SetBlendState(GraphicsDevice.BlendStates.Default);
$endif$
            base.Draw(gameTime);
        }
    }
}
