namespace HungNT.Database
{
    /// <summary>
    /// Interface cho một database table (ScriptableObject chứa config data).
    /// </summary>
    public interface IDataTable
    {
        /// <summary>Tên table (dùng cho debug / log).</summary>
        string TableName { get; }

        /// <summary>Gọi sau khi load — khởi tạo lookup, validate data, v.v.</summary>
        void Initialize();
    }
}