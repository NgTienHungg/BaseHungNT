namespace HungNT
{
    /// <summary>
    /// Interface cho save/load backend. Mặc định dùng <see cref="PlayerPrefsDataSave"/>,
    /// có thể thay bằng BayatGames, EasySave, hoặc custom backend.
    /// </summary>
    public interface IDataSave
    {
        /// <summary>Lưu object dưới key.</summary>
        void Save<T>(string key, T data);

        /// <summary>Load object từ key, trả về defaultValue nếu chưa có.</summary>
        T Load<T>(string key, T defaultValue = default);

        /// <summary>Xóa dữ liệu theo key.</summary>
        void Delete(string key);

        /// <summary>Kiểm tra key có tồn tại.</summary>
        bool HasKey(string key);
    }
}
