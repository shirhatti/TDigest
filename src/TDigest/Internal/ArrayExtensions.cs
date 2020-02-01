using System;
using System.Collections.Generic;
using System.Text;

namespace Shirhatti.Math.Stats.Internal
{
    internal static class ArrayExtensions
    {
        public static T[] Resize<T>(this T[] array, int newCapacity)
        {
            int size = System.Math.Min(array.Length, newCapacity);
            var ret = new T[size];
            Array.Copy(array, ret, size);
            return ret;
        }
    }
}
