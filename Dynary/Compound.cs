using System.Collections.Generic;
using Dynary.IO;

namespace Dynary
{
    /// <summary>
    /// Represents a compound, which can consist of multiple tokens.
    /// </summary>
    public class Compound : Token
    {
        /// <summary>
        /// Initializes an instance of the Compound class.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        public Compound(params Token[] tokens)
        {
            Tokens = new List<Token>();
            Tokens.AddRange(tokens);
        }

        /// <summary>
        /// Initializes an instance of the Compound class.
        /// </summary>
        public Compound()
        {
            Tokens = new List<Token>();
        }

        /// <summary>
        /// Writes this parseable to the underlying stream of the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Write(DynaryWriter writer)
        {
            writer.WriteType(Enums.TokenId.Compound);
            foreach (var token in Tokens)
            {
                token.Write(writer);
            }
            writer.EndCompound();
        }

        /// <summary>
        /// Reads this parseable from the underlying stream of the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override void Read(DynaryReader reader)
        {
            while (true)
            {
                var type = reader.ReadType();
                if (type == Enums.TokenId.Eoc)
                {
                    return;
                }

                var token = new Token(type, reader.ReadDynamic());
                Tokens.Add(token);
            }
        }

        /// <summary>
        /// Gets the tokens of this Compound.
        /// </summary>
        public List<Token> Tokens { get; }
    }
}
