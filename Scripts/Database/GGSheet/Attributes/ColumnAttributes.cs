using System;

namespace HungNT.Database
{
    /// <summary>Maps a field to a spreadsheet column (snake_case header). Inherits content binding from <see cref="ContentAttribute"/>.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnNameAttribute : ContentAttribute
    {
        public string ColumnName { get; set; }

        public ColumnNameAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }

    /// <summary>Legacy / alternate marker; treated like <see cref="ContentAttribute"/> for sheet binding.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnAttribute : ContentAttribute
    {
        public ColumnAttribute()
        {
        }

        public ColumnAttribute(string worksheetName)
            : base(worksheetName)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IgnoreColumnAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnNameFormatAttribute : Attribute
    {
        public string ColumnFormat { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PrimitiveArrayContent : Attribute
    {
        public readonly string CustomSeparator;

        public PrimitiveArrayContent(string customSeparator = "*")
        {
            CustomSeparator = customSeparator;
        }
    }
}
