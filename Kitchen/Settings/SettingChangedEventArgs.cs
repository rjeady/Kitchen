using System;

namespace Kitchen.Settings
{
    public class SettingChangedEventArgs<T> : EventArgs where T : IEquatable<T>
    {
        public SettingChangedEventArgs(Setting<T> setting, T oldValue)
        {
            Setting = setting;
            OldValue = oldValue;
        }

        public T OldValue { get; private set; }

        public ISetting Setting { get; private set; }
    }
}