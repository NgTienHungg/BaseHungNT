using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Đơn vị công việc trong chuỗi TaskRunner.
    /// <para>Hierarchy quy ước:</para>
    /// <list type="bullet">
    ///   <item>Sibling tiếp theo = NextTask (chạy tuần tự).</item>
    ///   <item>Children = ParallelTasks (chạy song song với task cha).</item>
    /// </list>
    /// <para>Override <see cref="OnBegin"/>, <see cref="OnRun"/>, <see cref="OnEnd"/> để xử lý logic.</para>
    /// <para>Set <see cref="IsCompleted"/> = true khi task hoàn thành.</para>
    /// </summary>
    public class BaseTask : MonoBehaviour
    {
        [Tooltip("Bỏ qua task này khi runner chạy.")]
        public bool IsIgnore;

        [SerializeField] private bool _completeWhenAllChildrenDone;

        private BaseTask _nextTask;
        private BaseTask[] _parallelTasks;

        public BaseTask NextTask
        {
            get => _nextTask;
            set => _nextTask = value;
        }

        public BaseTask[] ParallelTasks
        {
            get => _parallelTasks;
            protected set => _parallelTasks = value;
        }

        public bool IsRunning { get; set; }
        public bool IsCompleted { get; set; }
        public bool ForceInterrupt { get; set; }

        // ── Lifecycle (override these) ──────────────────────────────────────

        /// <summary>Gọi khi bắt đầu task.</summary>
        protected virtual void OnBegin() { }

        /// <summary>Gọi mỗi tick khi task đang chạy.</summary>
        protected virtual void OnRun() { }

        /// <summary>Gọi khi kết thúc task.</summary>
        protected virtual void OnEnd() { }

        // ── Internal API (called by TaskRunner) ──────────────────────────────

        internal void Begin()
        {
            IsCompleted = false;
            IsRunning = true;
            OnBegin();
        }

        internal void Run()
        {
            if (IsCompleted || !IsRunning) return;

            if (_completeWhenAllChildrenDone)
            {
                bool allDone = true;
                foreach (var task in _parallelTasks)
                {
                    if (task != this && !task.IsCompleted)
                    {
                        allDone = false;
                        break;
                    }
                }
                if (allDone) IsCompleted = true;
            }

            OnRun();
        }

        internal void End()
        {
            IsCompleted = true;
            IsRunning = false;
            OnEnd();
        }

        // ── Hierarchy wiring (called by TaskRunner.Awake) ────────────────────

        internal void SetupRelationships()
        {
            int siblingIndex = transform.GetSiblingIndex();

            if (siblingIndex < transform.parent.childCount - 1)
                _nextTask = transform.parent.GetChild(siblingIndex + 1).GetComponent<BaseTask>();

            _parallelTasks = GetComponentsInChildren<BaseTask>();
        }
    }
}
