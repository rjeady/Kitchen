using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Kitchen.Forms
{
	public static class ComboBoxExtensions
	{
		/// <summary>
		/// Populate the ComboBox with the given IEnumerable{string} of items.
		/// </summary>
		/// <param name="comboBox">The ComboBox to populate.</param>
		/// <param name="items">The items to populate the ComboBox with.</param>
		/// <param name="refreshIfSameCount">Whether to refresh the ComboBox items collection if the existing number of items is the same as the number of items provided.</param>
		public static void SetItems(this ComboBox comboBox, IEnumerable<string> items, bool refreshIfSameCount)
		{
			var itemsArray = items.ToArray();

			if (refreshIfSameCount || comboBox.Items.Count != itemsArray.Length)
			{
				comboBox.Items.Clear();
				comboBox.Items.AddRange(itemsArray);
			}
		}
	}
}
