using System;

namespace HungNT.Database
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ContentAttribute : Attribute
    {
        public string Alias { get; }
        public string Delimiter { get; } = string.Empty;
        public string WorksheetName { get; } = string.Empty;

        public ContentAttribute()
        {
            Alias = string.Empty;
        }

        public ContentAttribute(string alias, string delimiter)
        {
            Alias = alias;
            Delimiter = delimiter;
        }

        public ContentAttribute(string alias, string delimiter, string worksheetName)
        {
            Alias = alias;
            Delimiter = delimiter;
            WorksheetName = worksheetName;
        }

        public ContentAttribute(string worksheetName)
        {
            WorksheetName = worksheetName;
        }
    }
}
