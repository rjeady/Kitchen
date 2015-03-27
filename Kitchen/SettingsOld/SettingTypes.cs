using System;

namespace Kitchen.SettingsOld
{
	public class StringSetting : Setting<string>
	{
		public StringSetting(string defaultValue) : base(defaultValue) { }
		
		public override void ParseAndSet(string value)
		{
			Value = value;
		}
	}

	public class ValueSetting<T> : Setting<T> where T : struct, IConvertible, IEquatable<T>
	{
		public ValueSetting(T defaultValue) : base(defaultValue) { }

		public override void ParseAndSet(string value)
		{
			Value = (T)Convert.ChangeType(value, typeof(T));
		}
	}
}
