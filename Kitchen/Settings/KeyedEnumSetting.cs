using System;
using Kitchen.Enumerations;

namespace Kitchen.Settings
{
    public class KeyedEnumSetting<TEnum, TKey> : Setting<TEnum>
        where TEnum : KeyedEnum<TKey, TEnum>, IEquatable<TEnum>
    {
        public KeyedEnumSetting(TEnum defaultValue) : base(defaultValue) { }

        public override string Serialize()
        {
            return Value.Key.ToString();
        }

        public override void ParseAndSet(string value)
        {
            Value = (TEnum)(TKey)Convert.ChangeType(value, typeof(TKey));
        }
    }
}