using System;
using System.Windows.Forms;
using Kitchen.IO;

namespace Kitchen.Forms
{
	public static class IOErrorHandling
	{
		/// <summary>
		/// Attempt an IO action. If an IOException or an UnauthorizedAccessException occurs,
		/// the operation will be retried if the user chooses 'Retry' in a dialog with the specified text and caption.
		/// Returns a boolean value indicating whether or not the operation was successful.
		/// </summary>
		/// <param name="ioAction">The IO action to perform</param>
		/// <param name="shouldRetryDialogText">The text for the dialog asking the user if they want to retry the action.</param>
		/// <param name="shouldRetryDialogCaption">The caption for the dialog asking the user if they want to retry the action.</param>
		public static bool AttemptIOAction(Action ioAction, string shouldRetryDialogText, string shouldRetryDialogCaption)
		{
			return ErrorHandling.AttemptIOAction(ioAction, ex => MessageBox.Show(shouldRetryDialogText, shouldRetryDialogCaption, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry);
		}
	}
}
