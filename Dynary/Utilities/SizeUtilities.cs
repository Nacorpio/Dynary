using System.Linq;
using System.Runtime.InteropServices;

namespace Dynary.IO
{
    public static class SizeUtilities
    {
        /// <summary>
        /// Gets the size of the specified structure.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <param name="structure">The object.</param>
        /// <returns></returns>
        public static int SizeOf<T>(this T structure) where T : struct
        {
            return Marshal.SizeOf(structure);
        }
    }
}
