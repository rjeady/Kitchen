using System;
using System.Windows.Forms;
using Kitchen.Events;

namespace Kitchen.Forms
{
	/// <summary>
	/// A textbox adapter that applies input validation as a user enters text.
	/// The TextChangeValidated event should be handled to detect when the TextBox input text is changed.
	/// </summary>
	public class TextBoxValidator
	{
		private readonly TextBox _textBox;
		private readonly Predicate<string> _isValid;
		private string _lastValidText;
		private readonly bool _removeInvalidInputs;

		/// <summary>
		/// Occurs when the TextBox text change is committed and this change is validated.
		/// </summary>
		public event EventHandler<TextChangedEventArgs> TextChanged;

		/// <summary>
		/// Initializes a new instance of the TextBoxValidator class.
		/// </summary>
		/// <param name="textBox">The TextBox which will be validated</param>
		/// <param name="validationPredicate">A predicate to test the TextBox text against, to determine its validity</param>
		/// <param name="validateImmediately">Whether to immediately validate input upon user input, or wait until focus moves from the text box.</param>
		/// <param name="removeInvalidInputs">Whether to immediately remove invalid inputs from the text box</param>
		public TextBoxValidator(TextBox textBox, Predicate<string> validationPredicate, bool validateImmediately, bool removeInvalidInputs)
		{
			if (textBox == null)
				throw new ArgumentNullException("textBox");
			if (validationPredicate == null)
				throw new ArgumentNullException("validationPredicate");

			_textBox = textBox;
			_removeInvalidInputs = removeInvalidInputs;
			if (!removeInvalidInputs)
			{
				_textBox.Text = _lastValidText;
				_isValid = validationPredicate;
			}

			if (validateImmediately)
			{
				_textBox.TextChanged += TextBoxOnTextChanged;
			}
			else
			{
				_textBox.Leave += TextBoxOnTextChanged;
			}
		}

		private void TextBoxOnTextChanged(object sender, EventArgs e)
		{
			if (_isValid(_textBox.Text))
			{
				if (_removeInvalidInputs)
					_lastValidText = _textBox.Text;
				TextChanged.Raise(this, new TextChangedEventArgs(_textBox.Text, true));
			}
			else
			{
				if (_removeInvalidInputs)
				{
					int selection = _textBox.SelectionStart;
					int selectionChange = _textBox.Text.Length - _lastValidText.Length;
					_textBox.Text = _lastValidText;
					_textBox.SelectionStart = selection - selectionChange;
				}
				else
				{
					TextChanged.Raise(this, new TextChangedEventArgs(_textBox.Text, false));
				}
			}
		}
	}

	public class TextChangedEventArgs : EventArgs
	{
		public TextChangedEventArgs(string text, bool isValid)
		{
			Text = text;
			IsValid = isValid;
		}

		public string Text { get; private set; }
		public bool IsValid { get; private set; }
	}
}