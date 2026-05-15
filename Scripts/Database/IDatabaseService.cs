namespace HungNT.Database
{
    /// <summary>
    /// Service quản lý và cung cấp truy cập vào các <see cref="IDataTable"/>.
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
