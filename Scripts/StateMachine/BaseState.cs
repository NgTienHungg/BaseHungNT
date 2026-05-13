using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Abstract base cho state dạng MonoBehaviour.
    /// Override OnEnter/OnExecute/OnExit để xử lý logic.
    /// </summary>
    public abstract class BaseState : MonoBehaviour, IState
    {
        protected IStateData _data;
        private StateMachine _machine;

        internal void SetMachine(StateMachine machine) => _machine = machine;

        public bool IsActive => _machine != null && _machine.IsCurrentState(GetType());

        public void SetData(IStateData data) => _data = data;

        public TData GetData<TData>() where TData : IStateData => (TData)_data;

        public virtual void OnInitialize() { }
        public virtual void OnEnter() { }
        public virtual void OnExecute() { }
        public virtual void OnExit() { }
        public virtual void OnReset() { }

#if UNITY_EDITOR
        [ContextMenu("Force Enter This State")]
        private void EditorForceEnter()
        {
            if (_machine != null)
                _machine.ChangeState(GetType(), null);
        }
#endif
    }
}
