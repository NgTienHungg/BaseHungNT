using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT.Demo
{
    /// <summary>
    /// Demo: Cách sử dụng BindableProperty.
    /// <para>Gắn lên GameObject, Play Mode → nhấn các nút trong Inspector.</para>
    /// </summary>
    public class BindablePropertyDemo : MonoBehaviour
    {
        [ShowInInspector]
        private readonly BindableProperty<int> _hp = new(100);

        [ShowInInspector]
        private readonly BindableProperty<string> _playerName = new("Hero");

        private void Start()
        {
            _hp.RegisterWithInitValue(OnHpChanged).UnRegisterOnDestroy(this);
            _playerName.Register(OnNameChanged).UnRegisterOnDestroy(this);
        }

        private void OnHpChanged(int value) => Debug.Log($"[Bindable Demo] HP = {value}");
        private void OnNameChanged(string value) => Debug.Log($"[Bindable Demo] Name = {value}");

        [Button("Take 10 Damage")]
        public void TakeDamage()
        {
            _hp.Value -= 10;
        }

        [Button("Heal 20")]
        public void Heal()
        {
            _hp.Value += 20;
        }

        [Button("Change Name")]
        public void ChangeName()
        {
            _playerName.Value = "Warrior_" + Random.Range(1, 100);
        }

        [Button("Set HP Silently (no event)")]
        public void SetSilently()
        {
            _hp.SetValueSilently(999);
            Debug.Log($"[Bindable Demo] HP set silently to {_hp.Value}");
        }
    }
}
