using System;

namespace HungNT.Database
{
    /// <summary>
    /// Optional: class with <c>static void OnImportComplete()</c> invoked after a successful import batch.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ContentCallbacksAttribute : Attribute
    {
        public int Order { get; set; } = 100;
    }
}
