using System;
using Kitchen.Events;

namespace Kitchen.SettingsOld
{
    public abstract class Setting<T> : ISetting
        where T : IEquatable<T>
    {
        private T _value;
        private readonly T _defaultValue;

        protected Setting(T defaultValue)
        {
            _defaultValue = _value = defaultValue;
        }

        public event EventHandler<SettingChangedEventArgs<T>> Changed;

        public T Value
        {
            get { return _value; }
            set
            {
                if (!_value.Equals(value))
                {
                    var oldValue = _value;
                    _value = value;
                    Changed.Raise(this, new SettingChangedEventArgs<T>(this, oldValue));
                }
            }
        }

        public virtual string Serialize()
        {
            return Value.ToString();
        }

        public virtual void SetToDefault()
        {
            Value = _defaultValue;
        }

        public abstract void ParseAndSet(string value);
    }
}