using System;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Chạy chuỗi <see cref="BaseTask"/> theo thứ tự hierarchy.
    /// <para>Setup: Tạo children là các BaseTask — sibling = sequential, child = parallel.</para>
    /// <para>Runtime: Gọi <see cref="RunTask"/> để bắt đầu, <see cref="StopTask"/> để dừng.</para>
    /// </summary>
    public class TaskRunner : MonoBehaviour
    {
        [SerializeField] private bool _autoStart;

        private BaseTask _startingTask;
        private BaseTask _currentTask;
        private bool _isRunning;
        private BaseTask[] _tasks;
        private IStopHandler[] _stopHandlers;

        public bool IsRunning => _isRunning;
        public BaseTask CurrentTask => _currentTask;

        public event Action OnComplete;

        private void Awake()
        {
            _tasks = GetComponentsInChildren<BaseTask>();
            _stopHandlers = GetComponentsInChildren<IStopHandler>();

            if (_tasks.Length > 0)
            {
                foreach (var task in _tasks)
                    task.SetupRelationships();

                _startingTask = _tasks[0];
            }
        }

        private void OnEnable()
        {
            if (_autoStart && _startingTask != null)
                RunTask();
        }

        private void OnDisable()
        {
            if (_isRunning)
                StopTask();
        }

        // ── Public API ───────────────────────────────────────────────────────

        /// <summary>Bắt đầu chạy chuỗi task từ đầu.</summary>
        public void RunTask()
        {
            if (_isRunning || _startingTask == null) return;

            foreach (var task in _tasks)
            {
                task.IsCompleted = false;
                task.IsRunning = false;
                task.ForceInterrupt = false;
            }

            _currentTask = _startingTask;
            _isRunning = true;

            foreach (var task in _currentTask.ParallelTasks)
            {
                if (!task.IsIgnore)
                    task.Begin();
            }

            TaskRunnerManager.Instance.Subscribe(this);
        }

        /// <summary>Dừng toàn bộ runner và tất cả task.</summary>
        public void StopTask()
        {
            if (!_isRunning) return;

            _isRunning = false;
            _currentTask = null;

            foreach (var task in _tasks)
            {
                if (task.IsRunning)
                    task.End();
                task.IsCompleted = false;
                task.IsRunning = false;
                task.ForceInterrupt = false;
            }

            if (_stopHandlers != null)
            {
                foreach (var stop in _stopHandlers)
                    stop.OnStop();
            }

            if (TaskRunnerManager.Instance != null)
                TaskRunnerManager.Instance.Unsubscribe(this);

            OnComplete?.Invoke();
        }

        // ── Internal (called by TaskRunnerManager) ───────────────────────────

        internal void Tick()
        {
            if (!_isRunning || _currentTask == null) return;

            if (_currentTask.IsCompleted)
            {
                foreach (var task in _currentTask.ParallelTasks)
                {
                    if (!task.IsIgnore)
                        task.End();
                }

                if (_currentTask == null)
                {
                    StopTask();
                    return;
                }

                if (_currentTask.NextTask != null)
                {
                    _currentTask = _currentTask.NextTask;
                    foreach (var task in _currentTask.ParallelTasks)
                    {
                        if (!task.IsIgnore)
                            task.Begin();
                    }
                }
                else
                {
                    StopTask();
                }
            }
            else
            {
                foreach (var task in _currentTask.ParallelTasks)
                {
                    if (task.IsIgnore) continue;
                    task.Run();
                    if (task.ForceInterrupt)
                    {
                        StopTask();
                        return;
                    }
                }
            }
        }
    }
}
