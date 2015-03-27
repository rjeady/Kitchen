using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Kitchen.IO;

namespace Kitchen.SettingsOld
{
	public class XmlSettingsRepository : ISettingsRepository
	{
		private readonly string filePath;

		public XmlSettingsRepository(string filePath)
		{
			this.filePath = filePath;
		}

		public void SaveSettingsGroup(SettingsGroupBase settingsGroup)
		{
			XElement settingsRoot;
			try
			{
				settingsRoot = XElement.Load(filePath);
			}
			catch (FileNotFoundException)
			{
				// the settings file doesn't exist, so we need to create it.
				settingsRoot = new XElement("Settings");
			}
			
			var t = settingsGroup.GetType();

			var groupElement = settingsRoot.GetOrAddElement(t.Name);

			foreach (var field in t.GetFields(BindingFlags.Public | BindingFlags.Instance))
			{
				var setting = field.GetValue(settingsGroup) as ISetting;
				if (setting != null)
				{
					groupElement.SetOrAddElement(field.Name, setting.Serialize());
				}
			}

			settingsRoot.Save(filePath);
		}

		public void LoadSettingsGroup(SettingsGroupBase settingsGroup, bool defaultOnError)
		{
			var settingsRoot = XElement.Load(filePath);

			var t = settingsGroup.GetType();

			var groupElement = settingsRoot.GetOrAddElement(t.Name);

			foreach (var field in t.GetFields(BindingFlags.Public | BindingFlags.Instance))
			{
				var setting = field.GetValue(settingsGroup) as ISetting;
				if (setting != null)
				{
					var newValue = (string)groupElement.Element(field.Name);
					if (newValue == null)
					{
						// setting could not be loaded.
						if (defaultOnError)
							setting.SetToDefault();
					}
					else
					{
						try
						{
							setting.ParseAndSet(newValue);
						}
						catch (Exception)
						{
							if (defaultOnError)
								setting.SetToDefault();
						}
					}
				}
			}
		}
	}
}
