namespace Dynary
{
    public sealed class Enums
    {
        /// <summary>
        /// Represents a token identifier.
        /// </summary>
        public enum TokenId : byte
        {
            Byte       = 0x00,        // 0 "byte"
            Char       = 0x01,        // 1 "char"
            Short      = 0x02,        // 2 "short"
            Integer    = 0x03,        // 3 "int"
            Long       = 0x04,        // 4 "long"
            Float      = 0x05,        // 5 "float"
            Double     = 0x06,        // 6 "double"
            Decimal    = 0x07,        // 7 "decimal"
            Bool       = 0x08,        // 8 "bool"
            SByte      = 0x09,        // 9 "sbyte"
            String     = 0x0A,        // 10 "string"
            UShort     = 0x0B,        // 11 "ushort"
            ULong      = 0x0C,        // 12 "ulong"
            UInt       = 0x0D,        // 13 "uint"
            Struct     = 0x0E,        // 14 "struct"
            Class      = 0x0F,        // 15 "class"
            Collection = 0x10,        // 16 "ICollection"
            Dynamic    = 0x11,        // 17 "dynamic"
            Pair       = 0x12,        // 18 "name":"value"
            Compound   = 0x13,        // 19 "compound"
            List       = 0x14,        // 20 "list"
            Eoc        = 0x15,        // 21 End of compound
            Eof        = 0x16         // 22 End of file
        }
    }
}