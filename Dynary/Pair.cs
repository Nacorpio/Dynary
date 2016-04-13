namespace Dynary
{
    /// <summary>
    /// Represents a dynamic pair, consisting of a key and a value.
    /// </summary>
    public class Pair
    {
        /// <summary>
        /// Initializes an instance of the <see cref="Pair{T,TE}"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public Pair(dynamic key, dynamic value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets the key of this Pair.
        /// </summary>
        public dynamic Key { get; }

        /// <summary>
        /// Gets the value of this Pair.
        /// </summary>
        public dynamic Value { get; }
    }

    /// <summary>
    /// Represents a generic pair, consisting of a key and a value.
    /// </summary>
    /// <typeparam name="T">The key type.</typeparam>
    /// <typeparam name="TE">The value type.</typeparam>
    public class Pair<T, TE> : Pair
    {
        /// <summary>
        /// Initializes an instance of the <see cref="Pair{T,TE}"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public Pair(T key, TE value) : base(key, value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets the key of this Pair.
        /// </summary>
        public new T Key { get; }

        /// <summary>
        /// Gets the value of this Pair.
        /// </summary>
        public new TE Value { get; }
    }
}
