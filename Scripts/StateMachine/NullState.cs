namespace HungNT
{
    /// <summary>
    /// Empty/Null state — dùng làm trạng thái mặc định khi chưa có state nào.
    /// </summary>
    public sealed class NullState : IState
    {
        public static readonly NullState Instance = new();

        public bool IsActive => false;

        public void SetData(IStateData data) { }
        public void OnInitialize() { }
        public void OnEnter() { }
        public void OnExecute() { }
        public void OnExit() { }
        public void OnReset() { }
    }
}
