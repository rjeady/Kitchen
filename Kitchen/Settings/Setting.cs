
namespace Kitchen.Settings
{
    public interface ISetting
    {
        string Serialize();
        void ParseAndSet(string value);
        void SetToDefault();
    }
}