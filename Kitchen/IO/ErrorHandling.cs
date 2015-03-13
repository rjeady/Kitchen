using System;
using System.IO;

namespace Kitchen.IO
{
	public static class ErrorHandling
	{
		/// <summary>
		/// Attempt an IO action. If an IOException or an UnauthorizedAccessException occurs,
		/// the operation will be retried if the shouldRetry function returns true; otherwise the operation will be abandoned.
		/// Returns a boolean value indicating whether or not the operation was successful.
		/// </summary>
		/// <param name="ioAction">The IO action to perform</param>
		/// <param name="shouldRetry">
		/// The function called if an IOException or UnauthorizedAccessException occurs.
		/// Takes the exception as a paramater, and returns a boolean indicating whether or not to retry the operation.
		/// </param>
		public static bool AttemptIOAction(Action ioAction, Func<Exception, bool> shouldRetry)
		{
			bool retry;
			do
			{
				try
				{
					ioAction();
					// success.
					return true;
				}
				catch (IOException ex)
				{
					retry = shouldRetry(ex);
				}
				catch (UnauthorizedAccessException ex)
				{
					retry = shouldRetry(ex);
				}
			} while (retry);
			return false;
		}
	}
}
