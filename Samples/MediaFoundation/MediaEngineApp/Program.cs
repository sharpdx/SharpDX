// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Threading;
using System.Windows.Forms;

using SharpDX.MediaFoundation;

namespace MediaEngineApp
{
    /// <summary>
    /// Demonstrates simple usage of MediaEngine on Windows by playing a music selected by a file dialog.
    /// Note that this sample is not "Dispose" safe and is not releasing any COM resources.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The event raised when MediaEngine is ready to play the music.
        /// </summary>
        private static readonly ManualResetEvent eventReadyToPlay = new ManualResetEvent(false);

        /// <summary>
        /// Set when the music is stopped.
        /// </summary>
        private static bool isMusicStopped;

        /// <summary>
        /// The instance of MediaEngineEx
        /// </summary>
        private static MediaEngineEx mediaEngineEx;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The args.</param>
        [STAThread]
        static void Main(string[] args)
        {
            // Select a File to play
            var openFileDialog = new OpenFileDialog { Title = "Select a music file", Filter = "Music Files(*.WMA;*.MP3;*.WAV)|*.WMA;*.MP3;*.WAV" };
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            // Initialize MediaFoundation
            MediaManager.Startup();

            // Creates the MediaEngineClassFactory
            var mediaEngineFactory = new MediaEngineClassFactory();

            // Creates MediaEngine for AudioOnly 
            var mediaEngine = new MediaEngine(mediaEngineFactory, null, MediaEngineCreateFlags.AudioOnly);

            // Register our PlayBackEvent
            mediaEngine.PlaybackEvent += OnPlaybackCallback;

            // Query for MediaEngineEx interface
            mediaEngineEx = mediaEngine.QueryInterface<MediaEngineEx>();

            // Opens the file
            var fileStream = openFileDialog.OpenFile();
            
            // Create a ByteStream object from it
            var stream = new ByteStream(fileStream);

            // Creates an URL to the file
            var url = new Uri(openFileDialog.FileName, UriKind.RelativeOrAbsolute);

            // Set the source stream
            mediaEngineEx.SetSourceFromByteStream(stream, url.AbsoluteUri);

            // Wait for MediaEngine to be ready
            if (!eventReadyToPlay.WaitOne(1000))
            {
                Console.WriteLine("Unexpected error: Unable to play this file");
            }

            // Play the music
            mediaEngineEx.Play();

            // Wait until music is stopped.
            while (!isMusicStopped)
            {
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Called when [playback callback].
        /// </summary>
        /// <param name="playEvent">The play event.</param>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        private static void OnPlaybackCallback(MediaEngineEvent playEvent, long param1, int param2)
        {
            Console.Write("PlayBack Event received: {0}", playEvent);
            switch (playEvent)
            {
                case MediaEngineEvent.CanPlay:
                    eventReadyToPlay.Set();
                    break;
                case MediaEngineEvent.TimeUpdate:
                    Console.Write(" {0}", TimeSpan.FromSeconds(mediaEngineEx.CurrentTime));
                    break;
                case MediaEngineEvent.Error:
                case MediaEngineEvent.Abort:
                case MediaEngineEvent.Ended:
                    isMusicStopped = true;
                    break;
            }

            Console.WriteLine();
        }
    }
}
