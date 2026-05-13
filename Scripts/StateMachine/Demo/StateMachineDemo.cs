using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT.Demo
{
    /// <summary>
    /// Demo: Cách sử dụng StateMachine.
    /// <para>1. Gắn StateMachine + StateMachineDemo lên GameObject.</para>
    /// <para>2. Tạo children với các state (IdleDemoState, PatrolDemoState, AttackDemoState).</para>
    /// <para>3. Play Mode → nhấn các nút trong Inspector.</para>
    /// </summary>
    public class StateMachineDemo : MonoBehaviour
    {
        [SerializeField] private StateMachine _fsm;

        private void Awake()
        {
            if (_fsm == null)
                _fsm = GetComponent<StateMachine>();
        }

        private void Start()
        {
            _fsm.Initialize<IdleDemoState>();
        }

        private void Update()
        {
            _fsm.OnUpdate();
        }

        [Button("Change to Idle")]
        public void GoIdle() => _fsm.ChangeState<IdleDemoState>();

        [Button("Change to Patrol")]
        public void GoPatrol() => _fsm.ChangeState<PatrolDemoState>();

        [Button("Change to Attack (with data)")]
        public void GoAttack()
            => _fsm.ChangeState<AttackDemoState>(new StateData<string>("Enemy_01"));

        [Button("Back to Initial")]
        public void BackToInitial() => _fsm.BackToInitialState();

        [Button("Reset Machine")]
        public void ResetMachine() => _fsm.ResetMachine();
    }

    // ── Demo States ──────────────────────────────────────────────────────────

    public class IdleDemoState : BaseState
    {
        public override void OnEnter() => Debug.Log("[FSM Demo] Enter Idle");
        public override void OnExit() => Debug.Log("[FSM Demo] Exit Idle");
    }

    public class PatrolDemoState : BaseState
    {
        public override void OnEnter() => Debug.Log("[FSM Demo] Enter Patrol");
        public override void OnExecute() { /* patrol logic */ }
        public override void OnExit() => Debug.Log("[FSM Demo] Exit Patrol");
    }

    public class AttackDemoState : BaseState
    {
        public override void OnEnter()
        {
            var targetName = GetData<StateData<string>>()?.Data ?? "unknown";
            Debug.Log($"[FSM Demo] Enter Attack — target: {targetName}");
        }

        public override void OnExit() => Debug.Log("[FSM Demo] Exit Attack");
    }
}
