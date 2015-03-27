
namespace Kitchen.SettingsOld
{
    public interface ISetting
    {
        string Serialize();
        void ParseAndSet(string value);
        void SetToDefault();
    }
}