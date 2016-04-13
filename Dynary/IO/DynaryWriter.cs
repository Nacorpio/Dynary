using System.IO;
using System.Text;

namespace Dynary.IO
{
    /// <summary>
    /// Represents a writer, which writes in dynamic binary.
    /// </summary>
    public partial class DynaryWriter : BinaryWriter
    {
        /// <summary>
        /// Initializes an instance of the <see cref="DynaryWriter"/> class.
        /// </summary>
        /// <param name="output">The output stream.</param>
        /// <param name="encoding">The encoding.</param>
        public DynaryWriter(Stream output, Encoding encoding) : base(output, encoding)
        {}

        /// <summary>
        /// Writes the specified data with the specified token to the underlying stream.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="buffer">The data.</param>
        /// <param name="writeHeader">Whether to write a type header.</param>
        public void Write(Enums.TokenId token, byte[] buffer, bool writeHeader = true)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return;
            }

            if (writeHeader)
                WriteType(token);

            Write(buffer);
        }

        /// <summary>
        /// Writes the specified token to the underlying stream.
        /// </summary>
        /// <param name="token">The token.</param>
        public void WriteType(Enums.TokenId token)
        {
            OutStream.WriteByte((byte) token);
        }

        /// <summary>
        /// Writes the specified token to the underlying stream.
        /// </summary>s
        /// <param name="token">The token.</param>
        public void WriteToken(Token token)
        {
            token.Write(this);
        }
    }
}
