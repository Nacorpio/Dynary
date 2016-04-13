using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dynary.IO
{
    public static class ByteUtilities
    {
        /// <summary>
        /// Determines the token identifier of the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static Enums.TokenId ToToken(this object obj)
        {
            return ToToken(obj.GetType());
        }

        /// <summary>
        /// Determines the token identifier of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Enums.TokenId ToToken(this Type type)
        {
            if (type == null)
            {
                return Enums.TokenId.Eof;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Int16:
                    return Enums.TokenId.Short;
                case TypeCode.Int32:
                    return Enums.TokenId.Integer;
                case TypeCode.Int64:
                    return Enums.TokenId.Long;
                case TypeCode.Boolean:
                    return Enums.TokenId.Bool;
                case TypeCode.String:
                    return Enums.TokenId.String;
                case TypeCode.Char:
                    return Enums.TokenId.Char;
                case TypeCode.Double:
                    return Enums.TokenId.Double;
                case TypeCode.Single:
                    return Enums.TokenId.Float;
                case TypeCode.UInt16:
                    return Enums.TokenId.UShort;
                case TypeCode.UInt32:
                    return Enums.TokenId.UInt;
                case TypeCode.UInt64:
                    return Enums.TokenId.ULong;
                default:
                {
                    if (type == typeof(Pair))
                    {
                        return Enums.TokenId.Pair;
                    }

                    if (type == typeof(Compound))
                    {
                        return Enums.TokenId.Compound;
                    }

                    if (type == typeof(IList))
                    {
                        return Enums.TokenId.List;
                    }
                    throw new ArgumentException($"Unsupported type: {type.Name}");
                }
            }
        }

        /// <summary>
        /// Converts the specified enumerable of bytes to an object.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static dynamic ToObject<T>(this IEnumerable<byte> bytes)
        {
            return ToObject<T>(bytes, Encoding.Unicode);
        }

        /// <summary>
        /// Converts the specified enumerable of bytes to an object.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static dynamic ToObject<T>(this IEnumerable<byte> bytes, Encoding encoding)
        {
            switch (Type.GetTypeCode(typeof (T)))
            {
                case TypeCode.Int16:
                    return BitConverter.ToInt16(bytes.ToArray(), 0);
                case TypeCode.Int32:
                    return BitConverter.ToInt32(bytes.ToArray(), 0);
                case TypeCode.Int64:
                    return BitConverter.ToInt64(bytes.ToArray(), 0);
                case TypeCode.Char:
                    return encoding.GetChars(bytes.ToArray())[0];
                case TypeCode.Double:
                    return BitConverter.ToDouble(bytes.ToArray(), 0);
                case TypeCode.String:
                {
                    var buffer = bytes.ToList();
                    var size = (int) buffer.GetRange(0, 1)[0];

                    buffer.RemoveRange(0, 1);

                    var cSize = encoding.GetByteCount(new [] {'a'});

                    var result = string.Empty;
                    for (var i = 0; i < size * cSize; i += cSize)
                    {
                        var chunk = buffer.GetRange(i, cSize);
                        result += chunk.ToObject<char>(encoding);
                    }

                    return result;
                }
                default:
                    throw new ArgumentException($"Unsupported type: {Type.GetTypeCode(typeof(T))}");
            }
        }

        /// <summary>
        /// Converts the specified object to an array of bytes.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static IEnumerable<byte> ToBytes(this object obj)
        {
            return ToBytes(obj, Encoding.Unicode);
        } 

        /// <summary>
        /// Converts the specified object to an array of bytes.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static IEnumerable<byte> ToBytes(this object obj, Encoding encoding)
        {
            switch (Type.GetTypeCode(obj.GetType()))
            {
                case TypeCode.Int16:
                    return BitConverter.GetBytes((short) obj);
                case TypeCode.Int32:
                    return BitConverter.GetBytes((int) obj);
                case TypeCode.Int64:
                    return BitConverter.GetBytes((long) obj);
                case TypeCode.Boolean:
                    return BitConverter.GetBytes((bool) obj);
                case TypeCode.Byte:
                    return new [] {(byte) obj};
                case TypeCode.Char:
                    return BitConverter.GetBytes((char) obj);
                case TypeCode.Double:
                    return BitConverter.GetBytes((double) obj);
                case TypeCode.Single:
                    return BitConverter.GetBytes((float) obj);
                case TypeCode.String:
                {
                    var text = (string)obj;
                    var bytes = new List<byte>
                    {
                        (byte) text.Length
                    };

                    for (var i = 0; i < text.Length; i++)
                    {
                        var c = text.ToCharArray()[i];
                        bytes.AddRange(encoding.GetBytes(new [] {c}));
                    }
                    return bytes;
                }
                default:
                {
                    if (obj is IList)
                    {
                        var bytes = new List<byte>();
                        var list = (ArrayList) obj;

                        if (list.Count > byte.MaxValue)
                        {
                            throw new Exception("The list count exceeds the max value of a byte.");
                        }

                        bytes.Add((byte) list.Count);
                        foreach (var e in list)
                        {
                            bytes.AddRange(((int) e.ToToken()).ToBytes());
                            bytes.AddRange(e.ToBytes());
                        }

                        return bytes;
                    }

                    if (obj is Pair)
                    {
                        var bytes = new List<byte>();
                        var pair = (Pair) obj;

                        if (pair.Key == null || pair.Value == null)
                        {
                            throw new NullReferenceException("");
                        }

                        bytes.Add((byte) ToToken(pair.Key));
                        bytes.AddRange(ToBytes(pair.Key, encoding));

                        bytes.Add((byte)ToToken(pair.Value));
                        bytes.AddRange(ToBytes(pair.Value, encoding));

                        return bytes;
                    }
                    throw new ArgumentException($"Unsupported type: {obj.GetType().Name}");
                }
            }
        } 
    }
}
