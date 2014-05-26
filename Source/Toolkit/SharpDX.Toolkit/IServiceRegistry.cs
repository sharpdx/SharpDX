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

namespace SharpDX
{
    /// <summary>
    /// A service registry is a <see cref="IServiceProvider"/> that provides methods to register and unregister services.
    /// </summary>
    public interface IServiceRegistry : IServiceProvider
    {
        /// <summary>
        /// Occurs when a new service is added.
        /// </summary>
        event EventHandler<ServiceEventArgs> ServiceAdded;

        /// <summary>
        /// Occurs when when a service is removed.
        /// </summary>
        event EventHandler<ServiceEventArgs> ServiceRemoved;

        /// <summary>
        /// Gets the service object of specified type. The service must be registered with the <typeparamref name="T"/> type key.
        /// </summary>
        /// <remarks>This method will thrown an exception if the service is not registered, it null value can be accepted - use the <see cref="IServiceProvider.GetService"/> method.</remarks>
        /// <typeparam name="T">The type of the service to get.</typeparam>
        /// <returns>The service instance.</returns>
        /// <exception cref="ArgumentException">Is thrown when the corresponding service is not registered.</exception>
        T GetService<T>();

        /// <summary>
        /// Adds a service to this service provider.
        /// </summary>
        /// <param name="type">The type of service to add.</param>
        /// <param name="provider">The instance of the service provider to add.</param>
        /// <exception cref="System.ArgumentNullException">Service type cannot be null</exception>
        /// <exception cref="System.ArgumentException">Service is already registered</exception>
        void AddService(Type type, object provider);

        /// <summary>
        /// Adds a service to this service provider.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="provider">The instance of the service provider to add.</param>
        /// <exception cref="System.ArgumentNullException">Service type cannot be null</exception>
        /// <exception cref="System.ArgumentException">Service is already registered</exception>
        void AddService<T>(T provider);

        /// <summary>
        /// Removes the object providing a specified service.
        /// </summary>
        /// <param name="type">The type of service.</param>
        void RemoveService(Type type);
    }
}