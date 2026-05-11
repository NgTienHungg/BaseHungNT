namespace HungNT
{
    /// <summary>
    /// Base interface cho tất cả các game service.
    /// <para><see cref="Initialize"/>: Khởi tạo nội bộ — setup riêng của service, không phụ thuộc service khác.</para>
    /// <para><see cref="LateInitialize"/>: Gọi sau khi TẤT CẢ services đã Initialize xong — an toàn để gọi chéo service khác.</para>
    /// </summary>
    public interface IService
    {
        void Initialize();

        void LateInitialize();
    }
}