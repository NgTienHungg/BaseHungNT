using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT.Demo
{
    /// <summary>
    /// Demo: Cách sử dụng TaskRunner.
    /// <para>1. Gắn TaskRunnerDemo + TaskRunner lên GameObject.</para>
    /// <para>2. Tạo children: LogTask_A, WaitTask, LogTask_B (theo thứ tự sibling).</para>
    /// <para>3. Play Mode → nhấn Run/Stop trong Inspector.</para>
    /// </summary>
    public class TaskRunnerDemo : MonoBehaviour
    {
        [SerializeField] private TaskRunner _runner;

        private void Awake()
        {
            if (_runner == null)
                _runner = GetComponent<TaskRunner>();
        }

        [Button("Run")]
        public void Run() => _runner.RunTask();

        [Button("Stop")]
        public void Stop() => _runner.StopTask();
    }

    // ── Demo Tasks ───────────────────────────────────────────────────────────

    /// <summary>Task log message khi bắt đầu, hoàn thành ngay.</summary>
    public class LogDemoTask : BaseTask
    {
        [SerializeField] private string _message = "Hello from LogDemoTask";

        protected override void OnBegin()
        {
            Debug.Log($"[TaskRunner Demo] {_message}");
            IsCompleted = true;
        }
    }

    /// <summary>Task chờ N giây rồi hoàn thành.</summary>
    public class WaitDemoTask : BaseTask
    {
        [SerializeField] private float _duration = 1f;
        private float _elapsed;

        protected override void OnBegin()
        {
            _elapsed = 0f;
        }

        protected override void OnRun()
        {
            _elapsed += Time.deltaTime;
            if (_elapsed >= _duration)
                IsCompleted = true;
        }
    }
}
