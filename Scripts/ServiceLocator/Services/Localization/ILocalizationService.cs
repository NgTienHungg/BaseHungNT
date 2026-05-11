namespace HungNT
{
    /// <summary>
    /// Interface cho Localization service.
    /// Implement bằng hệ thống localization cụ thể (I2 Localization, Unity Localization, ...).
    /// </summary>
    public interface ILocalizationService : IService
    {
        /// <summary>Ngôn ngữ hiện tại (ví dụ: "en", "vi", "ja").</summary>
        string CurrentLanguage { get; }

        /// <summary>Lấy chuỗi đã dịch theo key. Trả về key nếu không tìm thấy.</summary>
        string GetText(string key);

        /// <summary>Đổi ngôn ngữ hiện tại.</summary>
        void SetLanguage(string languageCode);

        /// <summary>Kiểm tra xem key có tồn tại trong bảng localization không.</summary>
        bool HasKey(string key);
    }
}
