using System;

namespace HungNT.Database
{
    public enum ImportType
    {
        /// <summary>
        /// Automatic binding via <see cref="ContentAttribute"/> / <see cref="ColumnNameAttribute"/> on fields.
        /// </summary>
        Automatic,

        /// <summary>
        /// Custom import via static <c>Import(IImportUtility)</c> on the asset type.
        /// </summary>
        Manual,

        Both
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ContentAssetAttribute : Attribute
    {
        public ImportType ImportType { get; }
        public string PrimaryKey { get; }

        public ContentAssetAttribute(ImportType importType = ImportType.Automatic, string primaryKey = "key")
        {
            ImportType = importType;
            PrimaryKey = primaryKey;
        }
    }
}
