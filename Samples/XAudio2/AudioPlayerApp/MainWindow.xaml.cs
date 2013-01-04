// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;

using SharpDX;
using SharpDX.IO;
using SharpDX.MediaFoundation;
using SharpDX.XAudio2;

namespace AudioPlayerApp
{
    /// <summary>
    /// XAudio2 and MediaFoundation samples decoding a compressed audio file and playing it in real-time.
    /// </summary>
    public partial class MainWindow : Window
    {
        private XAudio2 xaudio2;
        private MasteringVoice masteringVoice;
        private Stream fileStream;
        private AudioPlayer audioPlayer;
        private DispatcherTimer timer;
        private object lockAudio = new object();
        private ToolTip volumeTooltip;

        public MainWindow()
        {
            InitializeComponent();

            InitializeXAudio2();

            volumeTooltip = new ToolTip();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += timer_Tick;
            timer.Start();
        }


        protected override void OnClosed(EventArgs e)
        {
            Utilities.Dispose(ref masteringVoice);
            Utilities.Dispose(ref xaudio2);
            base.OnClosed(e);
        }

        private void EjectButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Title = "Select an audio file (wma, mp3, ...etc.) or video file..." };
            if (dialog.ShowDialog() == true)
            {
                lock (lockAudio)
                {
                    if (audioPlayer != null)
                    {
                        audioPlayer.Close();
                        audioPlayer = null;
                    }

                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }

                    // Ask the user for a video or audio file to play
                    fileStream = new NativeFileStream(dialog.FileName, NativeFileMode.Open, NativeFileAccess.Read);

                    audioPlayer = new AudioPlayer(xaudio2, fileStream);

                    FilePathTextBox.Text = dialog.FileName;
                    SoundProgressBar.Maximum = audioPlayer.Duration.TotalSeconds;
                    SoundProgressBar.Value = 0;

                    // Auto-play
                    audioPlayer.Play();
                }
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            lock (lockAudio)
            {
                if (audioPlayer != null)
                {

                    if (audioPlayer.State == AudioPlayerState.Playing)
                    {
                        audioPlayer.Pause();
                    }
                    else
                    {
                        audioPlayer.Play();
                    }
                }
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            lock (lockAudio)
            {
                if (audioPlayer != null)
                {
                    audioPlayer.Stop();
                }
            }
        }

        private void InitializeXAudio2()
        {
            // This is mandatory when using any of SharpDX.MediaFoundation classes
            MediaManager.Startup();

            // Starts The XAudio2 engine
            xaudio2 = new XAudio2();
            xaudio2.StartEngine();
            masteringVoice = new MasteringVoice(xaudio2);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            lock (lockAudio)
            {
                if (audioPlayer != null)
                {
                    SoundProgressBar.Value = audioPlayer.Position.TotalSeconds;
                    TimeSpanTextBox.Text = FormatTimeSpan(audioPlayer.Position) + " / " + FormatTimeSpan(audioPlayer.Duration);
                }
            }
        }

        private static string FormatTimeSpan(TimeSpan time)
        {
            var timeStr = time.ToString("c");
            int index = timeStr.IndexOf('.');
            if (index > 0)
                timeStr = timeStr.Substring(0, index);
            return timeStr;
        }


        private bool isChangingPosition = false;

        private void SoundProgressBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isChangingPosition = true;
            SoundProgressBar_MouseMove(sender, e);
        }

        private Cursor previousCursor;
        private void SoundProgressBar_MouseEnter(object sender, MouseEventArgs e)
        {
            previousCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.IBeam;
        }

        private void SoundProgressBar_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = previousCursor;
        }

        private void SoundProgressBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isChangingPosition)
                return;

            lock (lockAudio)
            {
                if (audioPlayer != null)
                {
                    var pos = ((double)e.GetPosition(SoundProgressBar).X / SoundProgressBar.ActualWidth);
                    pos = Math.Min(1.0, Math.Max(0.0, pos));
                    audioPlayer.Position = new TimeSpan((long)(((SoundProgressBar.Maximum - SoundProgressBar.Minimum) * pos + SoundProgressBar.Minimum) * 1e7));
                }
            }
        }

        private void SoundProgressBar_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isChangingPosition = false;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lock (lockAudio)
            {
                if (audioPlayer != null)
                {
                    var volume = (float) Math.Min(1.0, Math.Max(0.0, e.NewValue));
                    audioPlayer.Volume = volume;
                    volumeTooltip.Content = string.Format("Volume {0}%", (int)(volume * 100));
                    volumeTooltip.IsOpen = true;
                    VolumeSlider.ToolTip = volumeTooltip;
                }
            }
        }

        private void VolumeSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (volumeTooltip.IsOpen)
                volumeTooltip.IsOpen = false;
        }
    }
}
