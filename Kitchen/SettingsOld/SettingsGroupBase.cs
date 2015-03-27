namespace Kitchen.SettingsOld
{
    public abstract class SettingsGroupBase { }

    public class AudioSettingsGroup : SettingsGroupBase
    {
        public static ValueSetting<int> Volume = new ValueSetting<int>(0);

    }
}
