using System;
using System.Collections;
using System.Collections.Generic;
using Dynary.IO;

namespace Dynary
{
    /// <summary>
    /// Represents a token without a dynamic value.
    /// </summary>
    public class Token : IParseable
    {
        public static Dictionary<Enums.TokenId, int> TokenSizes = new Dictionary<Enums.TokenId, int>
        {
            {Enums.TokenId.Integer, sizeof(int)},
            {Enums.TokenId.Short, sizeof(short)},
            {Enums.TokenId.Long, sizeof(long)},
            {Enums.TokenId.Char, sizeof(char)}
        }; 

        /// <summary>
        /// Initializes an instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        public Token(Enums.TokenId type, dynamic value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Token"/> class.
        /// </summary>
        public Token() : this(Enums.TokenId.Dynamic, null)
        {}

        /// <summary>
        /// Writes this <see cref="Token"/> to the underlying stream of the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public virtual void Write(DynaryWriter writer)
        {
            writer.WriteType(Type);
            switch (Type)
            {
                case Enums.TokenId.Short:
                    writer.WriteShort((short) Value);
                    break;
                case Enums.TokenId.Integer:
                    writer.WriteInteger((int) Value);
                    break;
                case Enums.TokenId.Long:
                    writer.WriteLong((long) Value);
                    break;
                case Enums.TokenId.Char:
                    writer.WriteChar((char) Value);
                    break;
                case Enums.TokenId.Byte:
                    writer.WriteSingleByte((byte) Value);
                    break;
                case Enums.TokenId.Float:
                    writer.WriteFloat((float) Value);
                    break;
                case Enums.TokenId.Double:
                    writer.WriteDouble((double) Value);
                    break;
                case Enums.TokenId.Decimal:
                    break;
                case Enums.TokenId.Bool:
                    writer.WriteBoolean((bool) Value);
                    break;
                case Enums.TokenId.SByte:
                    break;
                case Enums.TokenId.String:
                    writer.WriteString((string) Value);
                    break;
                case Enums.TokenId.UShort:
                    break;
                case Enums.TokenId.ULong:
                    break;
                case Enums.TokenId.UInt:
                    break;
                case Enums.TokenId.Struct:
                    break;
                case Enums.TokenId.Class:
                    break;
                case Enums.TokenId.Collection:
                    break;
                case Enums.TokenId.Dynamic:
                    break;
                case Enums.TokenId.Pair:
                    var pair = (Pair) Value;
                    writer.WritePair(pair.Value, pair.Key);
                    break;
                case Enums.TokenId.Compound:
                    var compound = (Compound) Value;
                    compound.Write(writer);
                    break;
                case Enums.TokenId.Eoc:
                    break;
                case Enums.TokenId.Eof:
                    break;
                default:
                    throw new ArgumentException($"Unsupported type: {Type}");
            }
        }

        /// <summary>
        /// Reads this <see cref="Token"/> from the underlying stream of the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public virtual void Read(DynaryReader reader)
        {
            Type = reader.ReadType();
            Value = reader.ReadAny();
        }

        /// <summary>
        /// Gets or sets the identifier of this Token.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the parent of this <see cref="Token"/>.
        /// </summary>
        public Token Parent { get; set; }

        /// <summary>
        /// Gets the type of this <see cref="Token"/>.
        /// </summary>
        public Enums.TokenId Type { get; private set; }

        /// <summary>
        /// Gets the value of this <see cref="Token"/>.
        /// </summary>
        public dynamic Value { get; private set; }

        /// <summary>
        /// Gets the path of this Token.
        /// </summary>
        /// <returns></returns>
        public string ToPath()
        {
            return $"{Id}->{Type}";
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return $"{Type}";
        }
    }

    /// <summary>
    /// Represents a token that can be either read or written to a stream.
    /// </summary>
    public class Token<T> : Token
    {
        /// <summary>
        /// Initializes an instance of the <see cref="Token{T}"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        public Token(Enums.TokenId type, T value) : base(type, value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the value of this <see cref="Token{T}"/>.
        /// </summary>
        public new T Value { get; }
    }
}
