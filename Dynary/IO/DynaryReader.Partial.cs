using System;
using System.Collections.Generic;
using System.Text;

namespace Dynary.IO
{
    public partial class DynaryReader
    {
        /// <summary>
        /// Reads a enumerable from the underlying stream.
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public IEnumerable<T> ReadEnumerable<T>(Encoding encoding)
        {
            var size = (int) ReadByte();
            if (size <= 0)
            {
                throw new ArgumentException("The amount of elements can't be zero or below.");
            }

            var result = new List<T>(size);

            var elementType = ReadType();
            for (var i = 0; i < size; i++)
            {
                var value = Read(elementType, encoding);
                result.Add((T) value);
            }

            return result;
        } 

        /// <summary>
        /// Reads the next compound from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public Compound ReadCompound()
        {
            var compound = new Compound();
            if (ReadType() != Enums.TokenId.Compound)
            {
                return null;
            }

            while (true)
            {
                var type = ReadType();
                if (type == Enums.TokenId.Eoc || type == Enums.TokenId.Eof)
                {
                    return compound;
                }
                compound.Tokens.Add(new Token(type, ReadDynamic())
                {
                    Parent = compound
                });
            }
        }

        /// <summary>
        /// Reads the next pair from the underlying stream.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public Pair ReadPair(Encoding encoding)
        {
            dynamic key = ReadAny(encoding);
            dynamic value = ReadAny(encoding);

            return new Pair(key, value);
        }

        /// <summary>
        /// Reads the next pair from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public Pair ReadPair()
        {
            return ReadPair(Encoding.Unicode);
        }

        /// <summary>
        /// Reads the next generic pair from the underlying stream.
        /// </summary>
        /// <typeparam name="T">The key type.</typeparam>
        /// <typeparam name="TE">The value type.</typeparam>
        /// <returns></returns>
        public Pair<T, TE> ReadPair<T, TE>()
        {
            return ReadPair<T, TE>(Encoding.Unicode);
        }

        /// <summary>
        /// Reads the next generic pair from the underlying stream.
        /// </summary>
        /// <typeparam name="T">The key type.</typeparam>
        /// <typeparam name="TE">The value type.</typeparam>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public Pair<T, TE> ReadPair<T, TE>(Encoding encoding)
        {
            T key = ReadAny(encoding);
            TE value = ReadAny(encoding);

            return new Pair<T, TE>(key, value);
        }

        /// <summary>
        /// Reads the next string from the underlying stream.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public string ReadString(Encoding encoding)
        {
            var buffer = new byte[1];
            Read(buffer, 0, 1);

            var size = (int)buffer[0];

            var result = string.Empty;
            for (var i = 0; i < size; i++)
            {
                result += ReadChar(encoding);
            }

            return result;
        }

        /// <summary>
        /// Reads the next string from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public new string ReadString()
        {
            return ReadString(Encoding.Unicode);
        }

        /// <summary>
        /// Reads the next float from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public float ReadFloat()
        {
            var buffer = new byte[sizeof(float)];
            Read(buffer, 0, sizeof(float));

            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Reads the next double from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public new double ReadDouble()
        {
            var buffer = new byte[sizeof(double)];
            Read(buffer, 0, sizeof(double));

            return BitConverter.ToDouble(buffer, 0);
        }

        /// <summary>
        /// Reads the next boolean from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public new bool ReadBoolean()
        {
            var buffer = new byte[sizeof(bool)];
            Read(buffer, 0, sizeof(bool));

            return BitConverter.ToBoolean(buffer, 0);
        }

        /// <summary>
        /// Reads the next 16-bit integer from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public new short ReadInt16()
        {
            var buffer = new byte[sizeof(short)];
            Read(buffer, 0, sizeof(short));

            return BitConverter.ToInt16(buffer, 0);
        }

        /// <summary>
        /// Reads the next character from the underlying stream.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public char ReadChar(Encoding encoding)
        {
            var buffer = new byte[sizeof(char)];
            Read(buffer, 0, sizeof(char));

            return encoding.GetChars(buffer)[0];
        }

        /// <summary>
        /// Reads the next character from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public new char ReadChar()
        {
            return ReadChar(Encoding.Unicode);
        }

        /// <summary>
        /// Reads the next 32-bit integer from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public new int ReadInt32()
        {
            var buffer = new byte[sizeof(int)];
            Read(buffer, 0, sizeof(int));

            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Reads the next 64-bit integer from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public new long ReadInt64()
        {
            var buffer = new byte[sizeof(long)];
            Read(buffer, 0, sizeof(long));

            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
