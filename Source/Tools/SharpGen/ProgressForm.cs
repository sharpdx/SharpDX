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
using System.Windows.Forms;
using SharpCore.Logging;

namespace SharpGen
{
    /// <summary>
    /// Progress form used to show the progress of the code generation.
    /// </summary>
    public partial class ProgressForm : Form, IProgressReport
    {
        private bool _isAborted = false;
        private bool _isClosedOk = false;

        public ProgressForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Notify progress.
        /// </summary>
        /// <param name="level">The level 0-100.</param>
        /// <param name="message">The message to notify.</param>
        /// <returns>
        /// true if the process is aborted; false otherwise
        /// </returns>
        public bool ProgressStatus(int level, string message)
        {
            if (level < progressBar1.Minimum)
                level = progressBar1.Minimum;
            if (level > progressBar1.Maximum)
                level = progressBar1.Maximum;

            MethodInvoker invoker = delegate
                              {
                                  progressBar1.Value = level;
                                  if (!_isAborted)
                                    statusLabel.Text = message;
                                  statusStrip1.Refresh();
                              };

            Invoke(invoker);
            return _isAborted;
        }

        public void FatalExit(string message)
        {
            MessageBox.Show(this, message, "SharpGen Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Abort();
        }

        /// <summary>
        /// Handles the Click event of the buttonAbort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            Abort();
            statusLabel.Text = "Aborting, please wait for shutdown...";
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Abort();
            e.Cancel = !_isClosedOk;
        }

        /// <summary>
        /// Aborts this instance.
        /// </summary>
        private void Abort()
        {
            _isAborted = true;
            statusLabel.Text = "Aborting, please wait for shutdown...";
        }

        /// <summary>
        /// Shutdowns this instance.
        /// </summary>
        public void Shutdown()
        {
            _isClosedOk = true;
            Close();
        }
    }
}
