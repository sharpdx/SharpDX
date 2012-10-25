// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using System.Collections.Generic;
using System.Reflection; // THIS NAMESPACE MUST BE USED FOR 4.5 CORE PROFILE

namespace SharpDX.Toolkit
{
    /// <summary>
    /// Main service provider for <see cref="Game"/>.
    /// </summary>
    public class GameServiceRegistry : IServiceRegistry
    {
        private readonly Dictionary<Type, object> registeredService = new Dictionary<Type, object>();

        #region IServiceRegistry Members

        /// <summary>
        /// Gets the instance service providing a specified service.
        /// </summary>
        /// <param name="type">The type of service.</param>
        /// <returns>The registered instance of this service.</returns>
        /// <exception cref="System.ArgumentNullException">type</exception>
        public object GetService(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            lock (registeredService)
            {
                if (registeredService.ContainsKey(type))
                    return registeredService[type];
            }

            return null;
        }

        public event EventHandler<ServiceEventArgs> ServiceAdded;

        public event EventHandler<ServiceEventArgs> ServiceRemoved;

        /// <summary>
        /// Adds a service to this <see cref="GameServiceRegistry"/>.
        /// </summary>
        /// <param name="type">The type of service to add.</param>
        /// <param name="provider">The service provider to add.</param>
        /// <exception cref="System.ArgumentNullException">type;Service type cannot be null</exception>
        /// <exception cref="System.ArgumentException">Service is already registered;type</exception>
        public void AddService(Type type, object provider)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (provider == null)
                throw new ArgumentNullException("provider");

#if WIN8METRO
            if (!type.GetTypeInfo().IsAssignableFrom(provider.GetType().GetTypeInfo()))
                throw new ArgumentException(string.Format("Service [{0}] must be assignable to [{1}]", provider.GetType().FullName, type.GetType().FullName));
#else
            if (!type.IsAssignableFrom(provider.GetType()))
                throw new ArgumentException(string.Format("Service [{0}] must be assignable to [{1}]", provider.GetType().FullName, type.GetType().FullName));
#endif

            lock (registeredService)
            {
                if (registeredService.ContainsKey(type))
                    throw new ArgumentException("Service is already registered", "type");
                registeredService.Add(type, provider);
            }
            OnServiceAdded(new ServiceEventArgs(type, provider));
        }

        /// <summary>Removes the object providing a specified service.</summary>
        /// <param name="type">The type of service.</param>
        public void RemoveService(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            object oldService = null;
            lock (registeredService)
            {
                if (registeredService.TryGetValue(type, out oldService))
                    registeredService.Remove(type);
            }
            if (oldService != null)
                OnServiceRemoved(new ServiceEventArgs(type, oldService));
        }

        #endregion

        private void OnServiceAdded(ServiceEventArgs e)
        {
            EventHandler<ServiceEventArgs> handler = ServiceAdded;
            if (handler != null) handler(this, e);
        }

        private void OnServiceRemoved(ServiceEventArgs e)
        {
            EventHandler<ServiceEventArgs> handler = ServiceRemoved;
            if (handler != null) handler(this, e);
        }
    }
}