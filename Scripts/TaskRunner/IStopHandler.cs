namespace HungNT
{
    /// <summary>
    /// Implement trên MonoBehaviour con của TaskRunner để nhận callback khi runner dừng.
    /// </summary>
    public interface IStopHandler
    {
        void OnStop();
    }
}
