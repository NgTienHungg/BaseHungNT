namespace HungNT
{
    /// <summary>
    /// Interface cho mỗi state trong StateMachine.
    /// </summary>
    public interface IState
    {
        bool IsActive { get; }

        void SetData(IStateData data);

        void OnInitialize();
        void OnEnter();
        void OnExecute();
        void OnExit();
        void OnReset();
    }
}
