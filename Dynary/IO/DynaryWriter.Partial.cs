using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dynary.IO
{
    public partial class DynaryWriter
    {
        /// <summary>
        /// Writes the specified enumerable to the underlying stream.
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="list">The list.</param>
        public void WriteEnumerable<T>(IEnumerable<T> list)
        {
            WriteEnumerable(list, Encoding.Unicode);
        }

        /// <summary>
        /// Writes the specified enumerable to the underlying stream.
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="encoding">The encoding.</param>
        public void WriteEnumerable<T>(IEnumerable<T> list, Encoding encoding)
        {
            var enumerable = list as T[] ?? list.ToArray();
            if (list == null || !enumerable.Any() || enumerable.Length > byte.MaxValue)
            {
                return;
            }

            WriteType(Enums.TokenId.List);
            WriteSingleByte((byte) enumerable.Length, false);
            WriteSingleByte((byte) typeof(T).ToToken(), false);

            foreach (var obj in enumerable)
            {
                Write(obj.ToBytes(encoding).ToArray());
            }
        }

        /// <summary>
        /// Writes the specified compound to the underlying stream.
        /// </summary>
        /// <param name="compound">The compound.</param>
        public void WriteCompound(Compound compound)
        {
            compound?.Write(this);
        }   

        /// <summary>
        /// Writes a pair with the specified parameters to the underlying stream.
        /// </summary>
        /// <typeparam name="T">The key type.</typeparam>
        /// <typeparam name="TE">The value type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="encoding">The encoding.</param>
        public void WritePair<T, TE>(T key, TE value, Encoding encoding)
        {
            if (key == null || value == null)
            {
                return;
            }

            WriteType(Enums.TokenId.Pair);

            Write(key.ToToken(), key.ToBytes(encoding).ToArray());
            Write(value.ToToken(), value.ToBytes(encoding).ToArray());
        }

        /// <summary>
        /// Writes a pair with the specified parameters to the underlying stream.
        /// </summary>
        /// <typeparam name="T">The key type.</typeparam>
        /// <typeparam name="TE">The value type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void WritePair<T, TE>(T key, TE value)
        {
            WritePair(key, value, Encoding.Unicode);
        }

        /// <summary>
        /// Writes the specified byte to the underlying stream.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="header">Whether to write a header.</param>
        public void WriteSingleByte(byte value, bool header = true)
        {
            Write(Enums.TokenId.Byte, new[] { value }, header);
        }

        /// <summary>
        /// Writes the specified 64-bit integer to the underlying stream.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteLong(long value)
        {
            Write(Enums.TokenId.Long, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes the specified 32-bit integer to the underlying stream.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteInteger(int value)
        {
            Write(Enums.TokenId.Integer, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes the specified 16-bit integer to the underlying stream.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteShort(short value)
        {
            Write(Enums.TokenId.Short, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes the specified character to the underlying stream.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="encoding">The encoding.</param>
        public void WriteChar(char character, Encoding encoding)
        {
            Write(Enums.TokenId.Char, encoding.GetBytes(new[] { character }));
        }

        /// <summary>
        /// Writes the specified float to the underlying stream.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteFloat(float value)
        {
            Write(Enums.TokenId.Float, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes the specified double to the underlying stream.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteDouble(double value)
        {
            Write(Enums.TokenId.Double, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes the specified boolean to the underlying stream.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteBoolean(bool value)
        {
            Write(Enums.TokenId.Bool, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes the specified character to the underlying stream.
        /// </summary>
        /// <param name="character">The character.</param>
        public void WriteChar(char character)
        {
            WriteChar(character, Encoding.Unicode);
        }

        /// <summary>
        /// Writes the end of a compound of the underlying stream.
        /// </summary>
        public void EndCompound()
        {
            WriteType(Enums.TokenId.Eoc);
        }

        /// <summary>
        /// Writes the end of the underlying stream.
        /// </summary>
        public void EndStream()
        {
            WriteType(Enums.TokenId.Eof);
        }

        /// <summary>
        /// Writes the specified string to the underlying stream.
        /// </summary>
        /// <param name="text">The text.</param>
        public void WriteString(string text)
        {
            WriteString(text, Encoding.Unicode);
        }

        /// <summary>
        /// Writes the specified string to the underlying stream.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="encoding">The encoding.</param>
        public void WriteString(string text, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length > byte.MaxValue)
            {
                return;
            }

            WriteType(Enums.TokenId.String);
            OutStream.WriteByte((byte)text.Length);

            for (var i = 0; i < text.Length; i++)
            {
                var c = text.ToCharArray()[i];
                Write(encoding.GetBytes(new[] { c }));
            }
        }
    }
}
