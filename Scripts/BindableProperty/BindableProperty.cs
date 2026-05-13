using System;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Generic reactive property — tự notify listener khi value thay đổi.
    /// <code>
    /// var hp = new BindableProperty&lt;int&gt;(100);
    /// hp.Register(val => Debug.Log($"HP = {val}")).UnRegisterOnDestroy(gameObject);
    /// hp.Value = 80; // → log "HP = 80"
    /// </code>
    /// </summary>
    [Serializable]
    public class BindableProperty<T>
    {
        [SerializeField] private T _value;
        private Action<T> _onValueChanged;

        public Func<T, T, bool> Comparer { get; set; } = DefaultComparer;

        public BindableProperty(T defaultValue = default)
        {
            _value = defaultValue;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (value == null && _value == null) return;
                if (value != null && Comparer(value, _value)) return;

                _value = value;
                _onValueChanged?.Invoke(value);
            }
        }

        /// <summary>Set value mà không fire event.</summary>
        public void SetValueSilently(T newValue)
        {
            _value = newValue;
        }

        // ── Register / Unregister ────────────────────────────────────────────

        /// <summary>Đăng ký lắng nghe thay đổi. Trả về token để hủy.</summary>
        public IUnRegister Register(Action<T> listener)
        {
            _onValueChanged += listener;
            return new BindablePropertyUnRegister(this, listener);
        }

        /// <summary>Đăng ký + gọi ngay với giá trị hiện tại.</summary>
        public IUnRegister RegisterWithInitValue(Action<T> listener)
        {
            listener?.Invoke(_value);
            return Register(listener);
        }

        /// <summary>Hủy đăng ký listener.</summary>
        public void UnRegister(Action<T> listener)
        {
            _onValueChanged -= listener;
        }

        // ── Implicit ────────────────────────────────────────────────────────

        public static implicit operator T(BindableProperty<T> property) => property._value;

        public override string ToString() => _value?.ToString() ?? "null";

        // ── Private ──────────────────────────────────────────────────────────

        private static bool DefaultComparer(T a, T b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            return a.Equals(b);
        }

        private class BindablePropertyUnRegister : IUnRegister
        {
            private BindableProperty<T> _property;
            private Action<T> _listener;

            public BindablePropertyUnRegister(BindableProperty<T> property, Action<T> listener)
            {
                _property = property;
                _listener = listener;
            }

            public void UnRegister()
            {
                _property?.UnRegister(_listener);
                _property = null;
                _listener = null;
            }
        }
    }
}
