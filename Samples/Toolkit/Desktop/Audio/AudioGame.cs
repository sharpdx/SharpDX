// Copyright (c) 2010-2013 SharpDX - SharpDX Team
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
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Audio;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audio
{
    class AudioGame :Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private KeyboardManager keyboardManager;
        private KeyboardState keyboardState;
        private MouseManager mouseManager;
        private MouseState mouseState;

        private SpriteBatch spriteBatch;
        private SpriteFont arial16BMFont;
        private PrimitiveBatch<VertexPositionColor> primitiveBatch;
        private BasicEffect primitiveBatchEffect;
        private BasicEffect geometryEffect;
        private GeometricPrimitive cube;
        private Texture2D listenerTexture;
        private Texture2D emitterTexture;
        private Random random;




        private AudioManager audioManager;
        private SoundEffect ergonWave;
        private SoundEffectInstance ergonWaveInstance;
        private WaveBank waveBank;
        //private WaveBank waveBankXbox; //does not play correctly

        private List<SoundTile> tiles;
        private bool play3D;

        public AudioGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            keyboardManager = new KeyboardManager(this);
            keyboardState = new KeyboardState();

            // Create the pointer manager
            mouseManager = new MouseManager(this);
            IsMouseVisible = true;

            random = new Random();           

            audioManager = new AudioManager(this);

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
            
        }

        protected override void Initialize()
        {
            Window.Title = "Audio Game";
            base.Initialize();
        }


        protected override void LoadContent()
        {     
            ergonWave = Content.Load<SoundEffect>("ergon.adpcm.wav");
            ergonWaveInstance = ergonWave.Create();
            ergonWaveInstance.IsLooped = true;
            waveBank = Content.Load<WaveBank>("TestBank.xwb");
            //waveBankXbox = Content.Load<WaveBank>("TestBankXbox.xwb"); //does not play correctly

            // SpriteFont supports the following font file format:
            // - DirectX Toolkit MakeSpriteFont or SharpDX Toolkit tkfont
            // - BMFont from Angelcode http://www.angelcode.com/products/bmfont/
            arial16BMFont = Content.Load<SpriteFont>("Arial16");

            // Instantiate a SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            primitiveBatch = new PrimitiveBatch<VertexPositionColor>(GraphicsDevice);
            primitiveBatchEffect = new BasicEffect(GraphicsDevice);
            primitiveBatchEffect.VertexColorEnabled = true;


            // setup tests
            tiles = new List<SoundTile>();           
            Rectangle border = new Rectangle();
            border.X = SoundTile.Padding.X;
            border.Y = SoundTile.Padding.Y;

            AddTile(ref border, "Click to play looped SoundEffectInstance of " + ergonWave.Name, () => ergonWaveInstance.Play());
            AddTile(ref border, "Click to play 'PewPew' wave bank entry", () => waveBank.Play("PewPew"));
            AddTile(ref border, "Click to play 'PewPew' wave bank entry with random pitch and pan", () => waveBank.Play("PewPew", 1,random.NextFloat(-1,1),random.NextFloat(-1,1)));
            AddTile(ref border, "Click to play 'PewPew' with 3D audio", () => PlayAudio3D());

            // setup 3D
            geometryEffect = ToDisposeContent(new BasicEffect(GraphicsDevice)
            {
                View = Matrix.LookAtRH(new Vector3(0, 10, 20), new Vector3(0, 0, 0), Vector3.UnitY),
                Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f),
                World = Matrix.Identity
            });

            cube = ToDisposeContent(GeometricPrimitive.Cube.New(GraphicsDevice));

            // Load the texture
            listenerTexture = Content.Load<Texture2D>("listen");
            emitterTexture = Content.Load<Texture2D>("speaker");            
            geometryEffect.TextureEnabled = true;

            base.LoadContent();

        }

        private void AddTile(ref Rectangle border, string label, Action playDelegate)
        {
            Vector2 labelSize;
            labelSize = arial16BMFont.MeasureString(label);
            border.Width = (int)(labelSize.X + SoundTile.Padding.X * 2);
            border.Height = (int)(labelSize.Y + SoundTile.Padding.Y * 2);
            tiles.Add(new SoundTile { Border = border, Label = label, PlayDelegate = playDelegate });
            border.Y = border.Bottom + SoundTile.Padding.Y; 
        }


        protected override void UnloadContent()
        {            
            Utilities.Dispose(ref spriteBatch);
            Utilities.Dispose(ref primitiveBatch);
            Utilities.Dispose(ref primitiveBatchEffect);
            base.UnloadContent();           
        }
        
        protected override void Update(GameTime gameTime)
        {
            var currentKey = keyboardManager.GetState();
            var currentMouse = mouseManager.GetState();           

            foreach (var tile in tiles)
            {
                if (tile.Border.Contains(new Vector2(currentMouse.X * GraphicsDevice.Viewport.Width,currentMouse.Y * GraphicsDevice.Viewport.Height)))
                {
                    if (currentMouse.Left == ButtonState.Pressed && mouseState.Left == ButtonState.Released)
                    {
                        tile.BorderColor = Color.Green;
                        tile.Play();
                    }
                    else
                    {
                        tile.BorderColor = Color.White;
                    }
                }
                else
                {
                    tile.BorderColor = Color.Gray;
                }
            }
           

            keyboardState = currentKey;
            mouseState = currentMouse;

            if (play3D)
                UpdateAudio3D(gameTime);

            base.Update(gameTime);
        }  
        
        protected override void Draw(GameTime gameTime)
        {
            // Clears the screen
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (play3D)
                DrawAudio3D(gameTime);

            DrawTiles();


            base.Draw(gameTime);
        }


        Matrix listener;
        Vector3 listenerVelocity;
        Matrix emitter;
        Vector3 emitterVelocity;
        SoundEffectInstance audio3DEffectInstance;

        private void PlayAudio3D()
        {
            if (play3D)
                return;

            listener = Matrix.LookAtRH(Vector3.Zero, new Vector3(0, 0, 4), Vector3.Up);
            listenerVelocity = Vector3.Zero;

            emitter = Matrix.LookAtRH(new Vector3(0,0,4), Vector3.Zero, Vector3.Up);
            emitterVelocity = Vector3.Zero;

            audio3DEffectInstance = waveBank.Create("PewPew");
            audio3DEffectInstance.IsLooped = true;
            audio3DEffectInstance.Apply3D(listener, listenerVelocity, emitter, emitterVelocity);
            audio3DEffectInstance.Play();
            play3D = true;
        }

        private void UpdateAudio3D(GameTime gameTime)
        {
            var rotation = (float)gameTime.TotalGameTime.TotalSeconds * 2.0f;

            emitter = Matrix.LookAtRH(new Vector3(0, 0, 4), Vector3.Zero, Vector3.Up) * Matrix.RotationY(rotation);
            audio3DEffectInstance.Apply3D(listener, listenerVelocity, emitter, emitterVelocity);
        }

        private void DrawAudio3D(GameTime gameTime)
        {
            geometryEffect.Texture = listenerTexture;
            geometryEffect.World = listener;
            cube.Draw(geometryEffect);

            geometryEffect.Texture = emitterTexture;
            geometryEffect.World = emitter;
            cube.Draw(geometryEffect);

        }

        private void DrawTiles()
        {
            primitiveBatchEffect.Projection = Matrix.OrthoOffCenterRH(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);
            primitiveBatchEffect.CurrentTechnique.Passes[0].Apply();
            primitiveBatch.Begin();
            foreach (var tile in tiles)
            {
                DrawTileBorder(tile);
            }
            primitiveBatch.End();

            spriteBatch.Begin();
            foreach (var tile in tiles)
            {
                spriteBatch.DrawString(arial16BMFont, tile.Label, (Vector2)tile.Border.TopLeft + SoundTile.Padding, tile.BorderColor);
            }
            spriteBatch.End();
        }

        private void DrawTileBorder(SoundTile tile)
        {
            var v1 = new VertexPositionColor { Color = tile.BorderColor };
            var v2 = new VertexPositionColor { Color = tile.BorderColor };

            v1.Position = new Vector3(tile.Border.TopLeft, 0);
            v2.Position = new Vector3(tile.Border.TopRight, 0);
            primitiveBatch.DrawLine(v1, v2);

            v1.Position = new Vector3(tile.Border.TopRight, 0);
            v2.Position = new Vector3(tile.Border.BottomRight, 0);
            primitiveBatch.DrawLine(v1, v2);

            v1.Position = new Vector3(tile.Border.BottomRight, 0);
            v2.Position = new Vector3(tile.Border.BottomLeft, 0);
            primitiveBatch.DrawLine(v1, v2);

            v1.Position = new Vector3(tile.Border.BottomLeft, 0);
            v2.Position = new Vector3(tile.Border.TopLeft, 0);
            primitiveBatch.DrawLine(v1, v2);
        }



        class SoundTile
        {
            public static Point Padding = new Point(4, 4);
            
            public Rectangle Border { get; set; }
            public Color BorderColor { get; set; }
            public string Label { get; set; }
            public Action PlayDelegate { get; set; }
            public Action StopDelegate { get; set; }

            public SoundTile()
            {
                BorderColor = Color.White;
            }

            public void Play()
            {
                if (PlayDelegate != null)
                    PlayDelegate();
            }
        }
    }
}
