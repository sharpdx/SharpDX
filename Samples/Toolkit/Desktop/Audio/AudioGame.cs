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
        private Color background;
        private AudioManager audioManager;
        private SoundEffect effect;
        private SoundEffectInstance effectInstance;
        private SoundEffect effectFromFile;
        private WaveBank waveBank;
        private WaveBank waveBank2;

        public AudioGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            keyboardManager = new KeyboardManager(this);
            keyboardState = new KeyboardState();
            background = Color.CornflowerBlue;

            audioManager = new AudioManager(this);
            GameSystems.Add(audioManager);
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
            base.LoadContent();

            effect = Content.Load<SoundEffect>("ergon.wav");
            effectInstance = effect.Create();
            waveBank = Content.Load<WaveBank>("TestBank.xwb");
            waveBank2 = Content.Load<WaveBank>("TestBankXbox.xwb");
            effectFromFile = SoundEffect.FromFile(audioManager,@"Content\ergon.adpcm.wav");
        }


        protected override void UnloadContent()
        {
            Utilities.Dispose(ref effectFromFile);
            base.UnloadContent();            
        }

        int bankIndex;
        protected override void Update(GameTime gameTime)
        {
            var current = keyboardManager.GetState();

            if (current.IsKeyDown(Keys.Space) && keyboardState.IsKeyUp(Keys.Space))
            {
                background = background == Color.CornflowerBlue ? Color.Red : Color.CornflowerBlue;

                if (effectInstance != null)
                {
                    if(effectInstance.State != SoundState.Playing)
                        effectInstance.Play();
                    else if (effectInstance.State == SoundState.Playing)
                        effectInstance.Stop();

                }
            }

            if (current.IsKeyDown(Keys.Enter) && keyboardState.IsKeyUp(Keys.Enter))
            {
                if (bankIndex >= waveBank.Count)
                    bankIndex = 0;

                if (waveBank != null)
                    waveBank.Play(bankIndex ++);

            }

            if (current.IsKeyDown(Keys.LeftControl) && keyboardState.IsKeyUp(Keys.LeftControl))
            {
                if (bankIndex >= waveBank2.Count)
                    bankIndex = 0;

                if (waveBank2 != null)
                    waveBank2.Play(bankIndex++);

            }

            keyboardState = current;

            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            // Clears the screen
            GraphicsDevice.Clear(background);

            base.Draw(gameTime);
        }


    }
}
