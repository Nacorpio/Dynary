using Dynary.IO;

namespace Dynary
{
    public interface IParseable
    {
        /// <summary>
        /// Writes this parseable to the underlying stream of the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void Write(DynaryWriter writer);

        /// <summary>
        /// Reads this parseable from the underlying stream of the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        void Read(DynaryReader reader);
    }
}
