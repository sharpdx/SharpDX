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
    /// Track all allocated objects.
    /// </summary>
    public static class ObjectTracker
    {
        private static readonly Dictionary<IntPtr, List<ObjectReference>> ObjectReferences = new Dictionary<IntPtr, List<ObjectReference>>(EqualityComparer.DefaultIntPtr);

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
                List<ObjectReference> referenceList;
                // Object is already tracked
                if (!ObjectReferences.TryGetValue(comObject.NativePointer, out referenceList))
                {
                    referenceList = new List<ObjectReference>();
                    ObjectReferences.Add(comObject.NativePointer, referenceList);
                }
#if W8CORE
                referenceList.Add(new ObjectReference(DateTime.Now, comObject));
#else
                referenceList.Add(new ObjectReference(DateTime.Now, comObject, new StackTrace(3, true)));
#endif
            }
        }

        /// <summary>
        /// Finds a list of object reference from a specified COM object pointer.
        /// </summary>
        /// <param name="comObjectPtr">The COM object pointer.</param>
        /// <returns>A list of object reference</returns>
        public static List<ObjectReference> Find(IntPtr comObjectPtr)
        {
            lock (ObjectReferences)
            {
                List<ObjectReference> referenceList;
                // Object is already tracked
                if (ObjectReferences.TryGetValue(comObjectPtr, out referenceList))
                    return new List<ObjectReference>(referenceList);
            }
            return new List<ObjectReference>();
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
                List<ObjectReference> referenceList;
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
                List<ObjectReference> referenceList;
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
                }
            }
        }

        /// <summary>
        /// Reports all COM object that are active and not yet disposed.
        /// </summary>
        public static List<ObjectReference> FindActiveObjects()
        {
            var activeObjects = new List<ObjectReference>();
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
    }
}