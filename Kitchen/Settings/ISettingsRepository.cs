
namespace Kitchen.Settings
{
	public interface ISettingsRepository
	{
		/// <summary>
		/// Save all of the settings from a particular settings group to the store.
		/// </summary>
		/// <param name="settingsGroup">The settings group.</param>
		void SaveSettingsGroup(SettingsGroupBase settingsGroup);

		/// <summary>
		/// Load all the settings for a particular settings group from the store.
		/// </summary>
		/// <param name="settingsGroup">The settings group.</param>
		/// <param name="defaultOnError">Whether to revert a setting to its default value if it cannot be loaded.</param>
		void LoadSettingsGroup(SettingsGroupBase settingsGroup, bool defaultOnError);
	}
}