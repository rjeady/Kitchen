using System;
using System.Xml.Linq;
using Kitchen.Enumerations;

namespace Kitchen.IO
{
	public static class XElementExtensions
	{
		/// <summary>
		/// Converts the value in the XElement to a member of the specified KeyedEnum, which has a string Key.
		/// </summary>
		/// <typeparam name="T">The type, derived from KeyEnum and with a string Key, to return</typeparam>
		/// <typeparam name="TKey">The key type.</typeparam>
		/// <param name="element">The XElement to get the value from.</param>
		/// <param name="defaultValue">The value to return if a corresponding element is not found.</param>
		/// <returns>
		/// The value from the specified element, converted to a NamedEnum member of type T,
		/// or the specified default value if a corresponding element is not found.
		/// </returns>	
		public static T ParseToKeyedEnum<TKey, T>(this XElement element, T defaultValue)
			where TKey : IConvertible
			where T : KeyedEnum<TKey, T>
		{
			var valueString = (string)element;
			T result;

			if (valueString != null && KeyedEnum<TKey, T>.TryParse((TKey)Convert.ChangeType(valueString, typeof(TKey)), out result))
			{
				return result;
			}
			return defaultValue;
		}

		/// <summary>
		/// Converts the value in the XElement to a member of the specified KeyedEnum, which has a string Key.
		/// </summary>
		/// <typeparam name="T">The type, derived from KeyEnum and with a string Key, to return</typeparam>
		/// <param name="element">The XElement to get the value from.</param>
		/// <param name="defaultValue">The value to return if a corresponding element is not found.</param>
		/// <returns>
		/// The value from the specified element, converted to a NamedEnum member of type T,
		/// or the specified default value if a corresponding element is not found.
		/// </returns>	
		public static T ParseToStringKeyedEnum<T>(this XElement element, T defaultValue) where T : KeyedEnum<string, T>
		{
			var valueString = (string)element;
			T result;

			if (valueString != null && KeyedEnum<string, T>.TryParse(valueString, out result))
			{
				return result;
			}
			return defaultValue;
		}

		/// <summary>
		/// Converts the value in the XElement to a member of the specified NamedEnum.
		/// </summary>
		/// <typeparam name="T">The type, derived from NamedEnum, to return</typeparam>
		/// <param name="element">The XElement to get the value from.</param>
		/// <param name="defaultValue">The value to return if a corresponding element is not found.</param>
		/// <returns>
		/// The value from the specified element, converted to a NamedEnum member of type T,
		/// or the specified default value if a corresponding element is not found.
		/// </returns>
		public static T ParseToNamedEnum<T>(this XElement element, T defaultValue) where T : Enum<T>
		{
			var valueString = (string)element;
			T result;

			if (valueString != null && Enum<T>.TryParse(valueString, out result))
			{
				return result;
			}
			return defaultValue;
		}

		/// <summary>
		/// Gets the value in an XElement as a specified type, using the specified conversion.
		/// </summary>
		/// <typeparam name="T">The type to return.</typeparam>
		/// <param name="element">The XElement to get the value from.</param>
		/// <param name="defaultValue">The value to return if the element is not found, or the conversion raises an exception.</param>
		/// <param name="conversion">A conversion function, which takes a string parameter to convert, and returns a value of type T.</param>
		/// <returns>The value from the specified element, converted to type T, or the specified default value if the conversion fails.</returns>
		public static T ParseToType<T>(this XElement element, Converter<string, T> conversion, T defaultValue = default(T))
		{
			var valueString = (string)element;

			if (valueString == null)
			{
				return defaultValue;
			}

			try
			{
				return conversion(valueString);
			}
			catch
			{
				return defaultValue;
			}
		}


		public static void SetOrAddElement(this XElement parentElement, string elementName, string value)
		{
			var element = parentElement.Element(elementName);
			if (element == null)
			{
				parentElement.Add(new XElement(elementName, value));
			}
			else
			{
				element.Value = value;
			}
		}

		public static XElement GetOrAddElement(this XElement parentElement, string elementName)
		{
			var element = parentElement.Element(elementName);
			if (element == null)
			{
				element = new XElement(elementName);
				parentElement.Add(element);
			}
			return element;
		}
	}
}