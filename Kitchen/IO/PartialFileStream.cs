using System.IO;

namespace Kitchen.IO
{
	/// <summary>
	/// A modified FileStream, where only a certain portion of the file will be read.
	/// </summary>
	class PartialFileStream : FileStream
	{
		public PartialFileStream(string path, FileMode mode, long readStartPosition, long readEndPosition)
			: base(path, mode)
		{
			base.Seek(readStartPosition, SeekOrigin.Begin);
			ReadEndPosition = readEndPosition;
		}

		/// <summary>
		/// The end position of the stream, at which the stream will no longer read.
		/// </summary>
		public long ReadEndPosition { get; set; }

		/// <summary>
		/// Reads a block of bytes from the current stream and writes the data to a buffer.
		/// The stream will not be read beyond the ReadEndPosition,
		/// but if seeking is performed it may be read from before the readStartPosition orginally specified.
		/// </summary>
		/// <returns>
		/// The total number of bytes written into the buffer.
		/// This can be less than the number of bytes requested if that number of bytes are not currently available,
		/// or zero if the end of the stream is reached before any bytes are read.
		/// </returns>
		/// <param name="buffer">When this method returns, contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the characters read from the current stream.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing data from the current stream.</param><param name="count">The maximum number of bytes to read. </param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative. </exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="offset"/> subtracted from the buffer length is less than <paramref name="count"/>. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current stream instance is closed. </exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (base.Position >= ReadEndPosition)
				return 0;

			if (base.Position + count > ReadEndPosition)
				count = (int)(ReadEndPosition - base.Position);

			return base.Read(buffer, offset, count);
		}
	}
}
