using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dynary.IO
{
    public partial class DynaryReader : BinaryReader
    {
        /// <summary>
        /// Initializes an instance of the DynaryReader class.
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <param name="encoding">The encoding.</param>
        public DynaryReader(Stream input, Encoding encoding) : base(input, encoding)
        {}

        /// <summary>
        /// Reads using the specified token identifier.
        /// </summary>
        /// <param name="token">The token identifier.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public dynamic Read(Enums.TokenId token, Encoding encoding)
        {
            dynamic value = null;
            switch (token)
            {
                case Enums.TokenId.Byte:
                    value = ReadByte();
                    break;
                case Enums.TokenId.Short:
                    value = ReadInt16();
                    break;
                case Enums.TokenId.Integer:
                    value = ReadInt32();
                    break;
                case Enums.TokenId.Long:
                    value = ReadInt64();
                    break;
                case Enums.TokenId.String:
                    value = ReadString(encoding);
                    break;
                case Enums.TokenId.Double:
                    value = ReadDouble();
                    break;
                case Enums.TokenId.Float:
                    value = ReadFloat();
                    break;
                case Enums.TokenId.Char:
                    value = ReadChar(encoding);
                    break;
                case Enums.TokenId.Pair:
                    value = ReadPair();
                    break;
            }
            return value;
        }

        /// <summary>
        /// Reads the next type from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public Enums.TokenId ReadType()
        {
            return (Enums.TokenId) ReadByte();
        }

        /// <summary>
        /// Reads the next token from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public Token ReadToken()
        {
            var t = new Token();
            t.Read(this);

            return t;
        }

        /// <summary>
        /// Reads all the tokens in the order they were written in from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Token> ReadToEnd()
        {
            return ReadToEnd(Encoding.Unicode);
        } 

        /// <summary>
        /// Reads all the tokens in the order they were written in from the underlying stream.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public IEnumerable<Token> ReadToEnd(Encoding encoding)
        {
            var tokens = new List<Token>();
            while (true)
            {
                try
                {
                    var type = ReadType();
                    if (type == Enums.TokenId.Eof || type == Enums.TokenId.Eoc)
                    {
                        return tokens;
                    }

                    dynamic value = ReadAny(type);
                    var token = new Token(type, value)
                    {
                        Id = tokens.Count
                    };

                    tokens.Add(token);
                }
                catch (EndOfStreamException e)
                {
                    Console.WriteLine("Couldn't find an end-of-file token.");
                    return null;
                }
            }
        }

        /// <summary>
        /// Reads any type of value using the specified token identifier.
        /// </summary>
        /// <param name="tokenId">The token identifier.</param>
        /// <returns></returns>
        private dynamic ReadAny(Enums.TokenId tokenId)
        {
            dynamic value = null;
            switch (tokenId)
            {
                case Enums.TokenId.Short:
                    value = ReadInt16();
                    break;
                case Enums.TokenId.Integer:
                    value = ReadInt32();
                    break;
                case Enums.TokenId.Long:
                    value = ReadInt64();
                    break;
                case Enums.TokenId.String:
                    value = ReadString();
                    break;
                case Enums.TokenId.Char:
                    value = ReadChar();
                    break;
                case Enums.TokenId.Byte:
                    value = ReadByte();
                    break;
                case Enums.TokenId.Double:
                    value = ReadDouble();
                    break;
                case Enums.TokenId.Float:
                    value = ReadFloat();
                    break;
                case Enums.TokenId.Pair:
                    value = ReadPair();
                    break;
                case Enums.TokenId.Compound:
                    value = ReadCompound();
                    break;
                case Enums.TokenId.List:
                    value = ReadEnumerable<dynamic>(Encoding.Unicode);
                    break;
            }
            return value;
        }

        /// <summary>
        /// Reads any type of value from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public dynamic ReadAny()
        {
            return ReadAny(Encoding.Unicode);
        }

        /// <summary>
        /// Reads any type of value from the underlying stream.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public dynamic ReadAny(Encoding encoding)
        {
            var type = ReadType();
            return ReadAny(type);
        }

        /// <summary>
        /// Reads any type of value from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public dynamic ReadDynamic()
        {
            return ReadAny(Encoding.Unicode);
        }
    }
}
