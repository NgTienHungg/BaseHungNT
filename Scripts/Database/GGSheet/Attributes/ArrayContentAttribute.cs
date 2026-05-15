using System;

namespace HungNT.Database
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ArrayContentAttribute : Attribute
    {
        public string WorksheetName { get; set; }
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;

        public ArrayContentAttribute()
        {
        }

        public ArrayContentAttribute(string worksheetName)
        {
            WorksheetName = worksheetName;
        }

        public ArrayContentAttribute(string worksheetName, string from, string to)
            : this(worksheetName)
        {
            From = from;
            To = to;
        }
    }
}
