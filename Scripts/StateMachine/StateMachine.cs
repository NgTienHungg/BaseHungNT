using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Finite State Machine — quản lý các state dạng MonoBehaviour trên children.
    /// <para>Gắn lên GameObject, thêm các <see cref="BaseState"/> vào children.</para>
    /// <para>Gọi <see cref="Initialize"/> một lần, sau đó <see cref="OnUpdate"/> mỗi frame.</para>
    /// </summary>
    public class StateMachine : MonoBehaviour
    {
        [SerializeField, Tooltip("Transform chứa các state children. Nếu null sẽ dùng chính transform này.")]
        private Transform _stateHolder;

#if UNITY_EDITOR
        [ShowInInspector, ReadOnly] private string _currentStateName;
        [ShowInInspector, ReadOnly] private string _previousStateName;
#endif

        private IState _currentState;
        private IState _previousState;
        private IState _nextState;
        private IState _initialState;

        private bool _pendingEnter;
        private bool _pendingExit;

        [ShowInInspector, ReadOnly]
        private readonly Dictionary<Type, IState> _states = new();

        public bool IsInitialized { get; private set; }
        public IState CurrentState => _currentState;
        public IState PreviousState => _previousState;

        // ── Initialize ───────────────────────────────────────────────────────

        /// <summary>
        /// Khởi tạo FSM: tìm tất cả BaseState trên children, đăng ký, và set initial state.
        /// </summary>
        /// <param name="startingStateType">
        /// Type của state khởi đầu. Nếu null sẽ dùng NullState.
        /// </param>
        public void Initialize(Type startingStateType = null)
        {
            var holder = _stateHolder != null ? _stateHolder : transform;
            var allStates = holder.GetComponentsInChildren<BaseState>(includeInactive: true);

            _states.Clear();
            foreach (var state in allStates)
            {
                var type = state.GetType();
                if (_states.ContainsKey(type))
                {
                    this.LogWarning($"Duplicate state {type.Name} — skipped.");
                    continue;
                }

                state.SetMachine(this);
                _states[type] = state;
            }

            _states[typeof(NullState)] = NullState.Instance;

            foreach (var kvp in _states)
                kvp.Value.OnInitialize();

            _initialState = startingStateType != null && _states.ContainsKey(startingStateType)
                ? _states[startingStateType]
                : NullState.Instance;

            _previousState = _initialState;
            _currentState = _initialState;
            _pendingEnter = true;
            _pendingExit = false;
            IsInitialized = true;
        }

        /// <summary>Khởi tạo FSM với state khởi đầu là <typeparamref name="TStart"/>.</summary>
        public void Initialize<TStart>() where TStart : BaseState
        {
            Initialize(typeof(TStart));
        }

        // ── Update ───────────────────────────────────────────────────────────

        /// <summary>Gọi mỗi frame (thường từ Update hoặc từ owner tick).</summary>
        public void OnUpdate()
        {
            if (!IsInitialized) return;

            if (_pendingExit)
            {
                _currentState.OnExit();
                _previousState = _currentState;
                _currentState = _nextState;
                _nextState = null;
                _pendingEnter = true;
                _pendingExit = false;
            }

            if (_pendingEnter)
            {
#if UNITY_EDITOR
                _previousStateName = _previousState?.GetType().Name;
                _currentStateName = _currentState?.GetType().Name;
#endif
                _currentState.OnEnter();
                _pendingEnter = false;
            }

            _currentState?.OnExecute();
        }

        // ── Change State ─────────────────────────────────────────────────────

        /// <summary>Chuyển sang state <typeparamref name="T"/> với data tùy chọn.</summary>
        public void ChangeState<T>(IStateData data = null) where T : IState
        {
            ChangeState(typeof(T), data);
        }

        /// <summary>Chuyển sang state theo Type với data tùy chọn.</summary>
        public void ChangeState(Type stateType, IStateData data)
        {
            if (!_states.TryGetValue(stateType, out var state))
            {
                this.LogError($"State {stateType.Name} not found in machine {gameObject.name}.");
                return;
            }

            if (_currentState?.GetType() == stateType) return;

            state.SetData(data);
            _nextState = state;
            _pendingExit = true;
        }

        /// <summary>Quay về state ban đầu.</summary>
        public void BackToInitialState()
        {
            if (_initialState != null)
                ChangeState(_initialState.GetType(), null);
        }

        /// <summary>Quay về state trước đó.</summary>
        public void BackToPreviousState()
        {
            if (_previousState != null)
                ChangeState(_previousState.GetType(), null);
        }

        /// <summary>Chuyển sang NullState (idle / empty).</summary>
        public void ChangeToEmptyState()
        {
            _nextState = NullState.Instance;
            _pendingExit = true;
        }

        // ── Query ────────────────────────────────────────────────────────────

        public bool IsCurrentState<T>() where T : IState => _currentState?.GetType() == typeof(T);
        public bool IsCurrentState(Type type) => _currentState?.GetType() == type;
        public bool IsInEmptyState() => _currentState is NullState;

        public bool IsPreviousState<T>() where T : IState => _previousState?.GetType() == typeof(T);

        public T GetState<T>() where T : IState
        {
            return _states.TryGetValue(typeof(T), out var state) ? (T)state : default;
        }

        public bool HasState<T>() where T : IState => _states.ContainsKey(typeof(T));
        public bool HasState(Type type) => _states.ContainsKey(type);

        // ── Reset ────────────────────────────────────────────────────────────

        /// <summary>Reset FSM về trạng thái ban đầu, gọi OnReset trên tất cả state.</summary>
        public void ResetMachine()
        {
            _previousState = _initialState;
            _currentState = _initialState;
            _pendingEnter = true;
            _pendingExit = false;

            foreach (var kvp in _states)
                kvp.Value.OnReset();
        }

        // ── Debug ────────────────────────────────────────────────────────────

        /// <summary>Snapshot tất cả state đang đăng ký — dùng bởi Editor.</summary>
        public IReadOnlyDictionary<Type, IState> GetDebugSnapshot() => _states;
    }
}
