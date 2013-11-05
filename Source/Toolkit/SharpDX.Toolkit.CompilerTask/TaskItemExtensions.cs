namespace SharpDX.Toolkit
{
    using System;

    using Microsoft.Build.Framework;

    /// <summary>The task item extensions class.</summary>
    public static class TaskItemExtensions
    {
        /// <summary>Gets the metadata.</summary>
        /// <typeparam name="T">The <see langword="Type" /> of attribute.</typeparam>
        /// <param name="taskItem">The task item.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The T.</returns>
        public static T GetMetadata<T>(this ITaskItem taskItem, string name, T defaultValue)
        {
            var outputValue = taskItem.GetMetadata(name);
            T value = defaultValue;
            if (!string.IsNullOrEmpty(outputValue))
            {
                try
                {
                    value = (T)Convert.ChangeType(outputValue, typeof(T));
                }
                catch (Exception)
                {
                }
            }
            return value;
        }
    }
}