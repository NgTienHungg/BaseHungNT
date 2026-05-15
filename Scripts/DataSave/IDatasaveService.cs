using HungNT;

namespace HungNT
{
    /// <summary>
    /// Service lưu/load dữ liệu người chơi.
    /// Register vào <see cref="T:HungNT.ServiceLocator"/> qua <see cref="T:HungNT.ServiceRegister"/>.
    /// <code>
    /// var save = this.GetService&lt;IDatasaveService&gt;();
    /// var coin = save.GetData&lt;CoinSave&gt;();
    /// coin.Amount += 100;
    /// save.Save(coin);
    /// </code>
    /// </summary>
    public interface IDatasaveService : IService
    {
        /// <summary>
        /// Lấy data (load từ disk nếu chưa cache, tạo mới nếu chưa tồn tại).
        /// </summary>
        T GetData<T>() where T : BaseUserData, new();

        /// <summary>Lưu data xuống disk.</summary>
        void Save<T>(T data) where T : BaseUserData;

        /// <summary>Lưu data đang cache theo type.</summary>
        void Save<T>() where T : BaseUserData, new();

        /// <summary>Lưu toàn bộ data đang cache.</summary>
        void SaveAll();

        /// <summary>Xóa data theo type khỏi disk và cache.</summary>
        void Delete<T>() where T : BaseUserData, new();

        /// <summary>Xóa toàn bộ dữ liệu đã lưu (dùng khi reset game).</summary>
        void DeleteAll();
    }
}
