using System;
using System.Collections.Generic;
using System.Text;

namespace Shirhatti.Math.Stats.Internal
{
    internal static class BitShiftExtensions
    {
        public static int UnsignedRightShift(this int number, int shiftBy)
        {
            return (int)((uint)number >> shiftBy);
        }
    }
}
