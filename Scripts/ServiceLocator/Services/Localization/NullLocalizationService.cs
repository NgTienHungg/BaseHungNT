namespace HungNT
{
    /// <summary>
    /// Null Object implementation của ILocalizationService.
    /// Trả về key gốc thay vì bản dịch — dùng trong Editor hoặc khi chưa setup localization.
    /// </summary>
    public class NullLocalizationService : ILocalizationService
    {
        public string CurrentLanguage { get; private set; } = "en";

        public string GetText(string key)
        {
            // Trả về key gốc để dễ phát hiện missing localization
            return key;
        }

        public void SetLanguage(string languageCode)
        {
            CurrentLanguage = languageCode;
        }

        public bool HasKey(string key)
        {
            return false;
        }
    }
}
