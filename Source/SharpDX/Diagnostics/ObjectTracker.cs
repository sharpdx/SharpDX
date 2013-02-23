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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using SharpDX.Collections;

namespace SharpDX.Diagnostics
{
    /// <summary>
    /// Event args for <see cref="ComObject"/> used by <see cref="ObjectTracker"/>.
    /// </summary>
    public class ComObjectEventArgs : EventArgs
    {
        /// <summary>
        /// The object being tracked/untracked.
        /// </summary>
        public ComObject Object;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComObjectEventArgs"/> class.
        /// </summary>
        /// <param name="o">The o.</param>
        public ComObjectEventArgs(ComObject o)
        {
            Object = o;
        }
    }

    /// <summary>
    /// List of <see cref="ObjectReference"/> tracked by <see cref="ObjectTracker"/>.
    /// </summary>
    public class ObjectReferenceCollection : List<ObjectReference>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectReferenceCollection"/> class.
        /// </summary>
        public ObjectReferenceCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1" /> class that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public ObjectReferenceCollection(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectReferenceCollection"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public ObjectReferenceCollection(IEnumerable<ObjectReference> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>The default instance.</value>
        public WeakReference DefaultInstance { get; internal set; }
    }

    /// <summary>
    /// Track all allocated objects.
    /// </summary>
    public static class ObjectTracker
    {
        private static readonly Dictionary<IntPtr, ObjectReferenceCollection> ObjectReferences = new Dictionary<IntPtr, ObjectReferenceCollection>(1024, EqualityComparer.DefaultIntPtr);

        /// <summary>
        /// Occurs when a ComObject is tracked.
        /// </summary>
        public static event EventHandler<ComObjectEventArgs> Tracked;

        /// <summary>
        /// Occurs when a ComObject is untracked.
        /// </summary>
        public static event EventHandler<ComObjectEventArgs> UnTracked;

#if !W8CORE
        /// <summary>
        /// Initializes the <see cref="ObjectTracker"/> class.
        /// </summary>
        static ObjectTracker()
        {
            AppDomain.CurrentDomain.DomainUnload += OnProcessExit;
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        /// <summary>
        /// Called when [process exit].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void OnProcessExit(object sender, EventArgs e)
        {
            if (Configuration.EnableObjectTracking)
            {
                var text = ReportActiveObjects();
                if (!string.IsNullOrEmpty(text))
                    Console.WriteLine(text);
            }
        }
#endif
        
        /// <summary>
        /// Tracks the specified COM object.
        /// </summary>
        /// <param name="comObject">The COM object.</param>
        public static void Track(ComObject comObject)
        {
            if (comObject == null || comObject.NativePointer == IntPtr.Zero)
                return;
            lock (ObjectReferences)
            {
                ObjectReferenceCollection referenceList;
                // Object is already tracked
                if (!ObjectReferences.TryGetValue(comObject.NativePointer, out referenceList))
                {
                    referenceList = new ObjectReferenceCollection();
                    ObjectReferences.Add(comObject.NativePointer, referenceList);
                }
#if W8CORE
                referenceList.Add(new ObjectReference(DateTime.Now, comObject));
#else
                referenceList.Add(new ObjectReference(DateTime.Now, comObject, new StackTrace(3, true)));
#endif
                // Fire Tracked event.
                OnTracked(comObject);
            }
        }

        /// <summary>
        /// Finds a list of object reference from a specified COM object pointer.
        /// </summary>
        /// <param name="comObjectPtr">The COM object pointer.</param>
        /// <returns>A list of object reference</returns>
        public static ObjectReferenceCollection Find(IntPtr comObjectPtr)
        {
            lock (ObjectReferences)
            {
                ObjectReferenceCollection referenceList;
                // Object is already tracked
                if (ObjectReferences.TryGetValue(comObjectPtr, out referenceList))
                    return new ObjectReferenceCollection(referenceList);
            }
            return new ObjectReferenceCollection();
        }

        internal static T FindOrCreateDefaultInstance<T>(IntPtr cppObjectPtr)
        {
            if (cppObjectPtr == IntPtr.Zero)
            {
                return default(T);
            }

            T valueInstance;
            lock (ObjectReferences)
            {
                ObjectReferenceCollection referenceList;

                // Object is already tracked
                if (!ObjectReferences.TryGetValue(cppObjectPtr, out referenceList))
                {
                    ObjectReferences.Add(cppObjectPtr, referenceList = new ObjectReferenceCollection());
                }
            
                // Create a new instance only if :
                // - There is no instance
                // - The current instance is not assignable to T (T has a higher instance than the stored instance)

                bool createNewInstance = false;
                object localRef = null;
                if (referenceList.DefaultInstance == null)
                {
                    createNewInstance = true;
                }
                else
                {
                    localRef = referenceList.DefaultInstance.Target;
                    if (localRef != null)
                    {
                        createNewInstance = (typeof(T) != localRef.GetType() && !Utilities.IsAssignableFrom(typeof(T), localRef.GetType()));
                    }
                }

                if (createNewInstance)
                {
                    valueInstance = (cppObjectPtr == IntPtr.Zero) ? default(T) : (T)Activator.CreateInstance(typeof(T), cppObjectPtr);
                    referenceList.DefaultInstance = new WeakReference(valueInstance);
                }
                else
                {
                    valueInstance = (T)localRef;
                }

            }
            return valueInstance;
        }

        internal static void MakeDefaultInstance<T>(IntPtr cppObjectPtr, T valueInstance) where T : CppObject
        {
            if (cppObjectPtr == IntPtr.Zero)
            {
                valueInstance.NativePointer = cppObjectPtr;
                return;
            }

            lock (ObjectReferences)
            {
                ObjectReferenceCollection referenceList;

                // Object is already tracked
                if (!ObjectReferences.TryGetValue(cppObjectPtr, out referenceList))
                {
                    ObjectReferences.Add(cppObjectPtr, referenceList = new ObjectReferenceCollection());
                }

                if (referenceList.DefaultInstance == null || !ReferenceEquals(referenceList.DefaultInstance.Target, valueInstance))
                {
                    referenceList.DefaultInstance = new WeakReference(valueInstance);
                }

                valueInstance.NativePointer = cppObjectPtr;
            }
        }

        internal static void ReleaseDefaultInstance(IntPtr cppObjectPtr, CppObject objectRef)
        {
            if (cppObjectPtr == IntPtr.Zero)
            {
                return;
            }

            lock (ObjectReferences)
            {
                ObjectReferenceCollection referenceList;

                // Object is tracked, reset the default instance
                if (ObjectReferences.TryGetValue(cppObjectPtr, out referenceList) && ReferenceEquals(referenceList.DefaultInstance.Target, objectRef))
                {
                    referenceList.DefaultInstance = null;
                }
            }
        }

        /// <summary>
        /// Finds the object reference for a specific COM object.
        /// </summary>
        /// <param name="comObject">The COM object.</param>
        /// <returns>An object reference</returns>
        public static ObjectReference Find(ComObject comObject)
        {
            lock (ObjectReferences)
            {
                ObjectReferenceCollection referenceList;
                // Object is already tracked
                if (ObjectReferences.TryGetValue(comObject.NativePointer, out referenceList))
                {
                    foreach (var objectReference in referenceList)
                    {
                        if (ReferenceEquals(objectReference.Object.Target, comObject))
                            return objectReference;
                    }
                }
            }            
            return null;
        }

        /// <summary>
        /// Untracks the specified COM object.
        /// </summary>
        /// <param name="comObject">The COM object.</param>
        public static void UnTrack(ComObject comObject)
        {
            if (comObject == null || comObject.NativePointer == IntPtr.Zero)
                return;

            lock (ObjectReferences)
            {
                ObjectReferenceCollection referenceList;
                // Object is already tracked
                if (ObjectReferences.TryGetValue(comObject.NativePointer, out referenceList))
                {
                    for (int i = referenceList.Count-1; i >=0; i--)
                    {
                        var objectReference = referenceList[i];
                        if (ReferenceEquals(objectReference.Object.Target, comObject))
                            referenceList.RemoveAt(i);
                        else if (!objectReference.IsAlive)
                            referenceList.RemoveAt(i);
                    }
                    // Remove empty list
                    if (referenceList.Count == 0)
                        ObjectReferences.Remove(comObject.NativePointer);

                    // Fire UnTracked event
                    OnUnTracked(comObject);
                }
            }
        }

        /// <summary>
        /// Reports all COM object that are active and not yet disposed.
        /// </summary>
        public static ObjectReferenceCollection FindActiveObjects()
        {
            var activeObjects = new ObjectReferenceCollection();
            lock (ObjectReferences)
            {
                foreach (var referenceList in ObjectReferences.Values)
                {
                    foreach (var objectReference in referenceList)
                    {
                        if (objectReference.IsAlive)
                            activeObjects.Add(objectReference);
                    }
                }
            }
            return activeObjects;
        }

        /// <summary>
        /// Reports all COM object that are active and not yet disposed.
        /// </summary>
        public static string ReportActiveObjects()
        {
            var text = new StringBuilder();
            foreach (var findActiveObject in FindActiveObjects())
            {
                var findActiveObjectStr = findActiveObject.ToString();
                if (!string.IsNullOrEmpty(findActiveObjectStr))
                    text.AppendLine(findActiveObjectStr);
            }
            return text.ToString();
        }

        private static void OnTracked(ComObject obj)
        {
            var handler = Tracked;
            if (handler != null)
            {
                handler(null, new ComObjectEventArgs(obj));
            }
        }

        private static void OnUnTracked(ComObject obj)
        {
            var handler = UnTracked;
            if (handler != null)
            {
                handler(null, new ComObjectEventArgs(obj));
            }
        }
   }
}