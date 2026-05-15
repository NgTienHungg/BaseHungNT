using System.Collections.Generic;
using Sirenix.OdinInspector;
using HungNT;

namespace HungNT
{
    /// <summary>
    /// Singleton batch-update tất cả <see cref="TaskRunner"/> đang chạy.
    /// Thay vì mỗi runner tự Update, chỉ có 1 Update duy nhất ở đây.
    /// </summary>
    public class TaskRunnerManager : MonoSingleton<TaskRunnerManager>
    {
        [ShowInInspector, ReadOnly]
        private readonly List<TaskRunner> _runners = new();

        public void Subscribe(TaskRunner runner)
        {
            if (!_runners.Contains(runner))
                _runners.Add(runner);
        }

        public void Unsubscribe(TaskRunner runner)
        {
            _runners.Remove(runner);
        }

        private void Update()
        {
            for (int i = _runners.Count - 1; i >= 0; i--)
            {
                var runner = _runners[i];
                if (runner != null)
                    runner.Tick();
            }
        }
    }
}
