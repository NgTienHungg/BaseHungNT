using System;

namespace HungNT
{
    /// <summary>
    /// Abstract base cho mỗi "khối" dữ liệu người chơi cần lưu.
    /// <para>Kế thừa, thêm các field [Serializable], gọi service để save/load.</para>
    /// <code>
    /// [Serializable]
    /// public class CoinSave : BaseUserData
    /// {
    ///     public long Amount = 0;
    /// }
    /// </code>
    /// </summary>
    [Serializable]
    public abstract class BaseUserData
    {
        /// <summary>Key dùng để save/load với ES3. Mặc định = tên class.</summary>
        public string Key { get; private set; }

        protected BaseUserData()
        {
            Key = GetType().Name;
        }

        protected BaseUserData(string customKey)
        {
            Key = customKey;
        }

        /// <summary>
        /// Gọi ngay sau khi load từ disk.
        /// Override để migration / fix data cũ.
        /// </summary>
        public virtual void OnAfterLoad() { }
    }
}
