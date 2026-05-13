namespace HungNT
{
    /// <summary>
    /// Service quản lý và cung cấp truy cập vào các <see cref="IDataTable"/>.
    /// Register vào <see cref="ServiceLocator"/> qua <see cref="ServiceRegister"/>.
    /// <para>Tất cả ScriptableObject tables được load tự động từ <c>Resources/Database/</c>.</para>
    /// <code>
    /// var db = this.GetService&lt;IDatabaseService&gt;();
    /// var table = db.GetTable&lt;ItemTable&gt;();
    /// </code>
    /// </summary>
    public interface IDatabaseService : IService
    {
        /// <summary>Lấy table theo type. Throw nếu không tìm thấy.</summary>
        T GetTable<T>() where T : BaseDataTable;

        /// <summary>Thử lấy table. Trả về false nếu chưa load.</summary>
        bool TryGetTable<T>(out T table) where T : BaseDataTable;

        /// <summary>Kiểm tra table đã được load chưa.</summary>
        bool HasTable<T>() where T : BaseDataTable;
    }
}
