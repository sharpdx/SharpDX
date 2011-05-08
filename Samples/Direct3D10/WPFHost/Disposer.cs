using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFHost
{
    public static class Disposer
    {
        /// <summary>
        /// Dispose an object instance and set the reference to null
        /// </summary>
        /// <typeparam name="TypeName">The type of object to dispose</typeparam>
        /// <param name="resource">A reference to the instance for disposal</param>
        /// <remarks>This method hides any thrown exceptions that might occur during disposal of the object (by design)</remarks>
        public static void SafeDispose<TypeName>(ref TypeName resource) where TypeName : class
        {
            if (resource == null)
                return;

            IDisposable disposer = resource as IDisposable;
            if (disposer != null)
            {
                try
                {
                    disposer.Dispose();
                }
                catch
                {
                }
            }

            resource = null;
        }
    }
}
