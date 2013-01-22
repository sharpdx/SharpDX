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
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace SharpDocPak
{
    /// <summary>
    /// A simple WebServer
    /// </summary>
    internal class HttpServer
    {
        private readonly HttpListener _httpListener;
        private Thread _daemon;

        private const int startingPort = 8000;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="urlPrefix">The URL prefix.</param>
        public HttpServer()
        {
            // If HtppListener is not supported, exit with an errors
            if (!HttpListener.IsSupported)
            {
                MessageBox.Show("Windows XP SP2 or Server 2003 is required to use the HttpListener class", "HttpServer Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            var port = FindAvailablePort();
            Url = "http://localhost:" + port + "/";

            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(Url);
        }

        /// <summary>
        /// Gets the URL of this server.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; private set; }

        /// <summary>
        /// Finds an available HTTP port.
        /// </summary>
        /// <exception cref="InvalidOperationException">If unable to find an available port</exception>
        /// <returns>A port</returns>
        public static int FindAvailablePort()
        {
            var localhost = Dns.GetHostEntry("localhost").AddressList[0];
            for (int i = startingPort; i < 65535; i++)
            {
                try
                {
                    var listener = new TcpListener(localhost, i);
                    listener.Start();
                    listener.Stop();
                    return i;
                }
                catch (SocketException ex)
                {
                }
            }
            throw new InvalidOperationException("Unable to find a HTTP port");
        }

        /// <summary>
        /// Occurs when [process request].
        /// </summary>
        public event EventHandler<HttpRequestEventArgs> ProcessRequest;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _httpListener.Start();
            _daemon = new Thread(() =>
                           {
                               try
                               {
                                   while (true)
                                   {
                                       var request = _httpListener.GetContext();
                                       if (ProcessRequest != null)
                                           ProcessRequest(this, new HttpRequestEventArgs(request));
                                   }
                               }
                               catch (Exception ex)
                               {
                                   Console.WriteLine(ex);
                               }
                           }
                ) {IsBackground = true};
            _daemon.Start();
        }

        private void StopListening()
        {
            _httpListener.GetType().InvokeMember("RemoveAll", BindingFlags.Instance, null,  _httpListener, new object[] {false});
            _httpListener.Close();
            // pendingRequestQueue.Clear(); //this is something we use but just in case you have some requests clear them
        }

    }
}