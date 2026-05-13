using System;

namespace HungNT
{
    /// <summary>
    /// Abstract base cho mỗi "khối" dữ liệu người dùng cần lưu.
    /// <para>Kế thừa và thêm các field cần lưu (phải [Serializable]).</para>
    /// <code>
    /// [Serializable]
    /// public class CurrencySave : BaseUserData
    /// {
    ///     public int Gold;
    ///     public int Gem;
    /// }
    /// </code>
    /// </summary>
    [Serializable]
    public abstract class BaseUserData
    {
        /// <summary>Key dùng để save/load. Mặc định = tên class.</summary>
        public string Key { get; private set; }

        protected BaseUserData()
        {
            Key = GetType().Name;
        }

        protected BaseUserData(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Gọi sau khi load — override để sửa data cũ / migration.
        /// </summary>
        public virtual void OnAfterLoad() { }
    }
}
