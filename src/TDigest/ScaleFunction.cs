using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace Shirhatti.Math.Stats
{
    public enum ScaleFunction
    {
        K_0,
        K_1,
        K_1_Fast,
        K_2,
        K_2_NO_NORM,
        K_3,
        K_3_NO_NORM
    }

    // TODO Need to fill in everything except K_0
    public static class ScaleFunctionExtensions
    {
        public static double K(this ScaleFunction scaleFunction, double q, double compression, double n)
        {
            switch(scaleFunction)
            {
                case ScaleFunction.K_0:
                    return compression * q / 2;
                case ScaleFunction.K_1:
                    return 0;
                case ScaleFunction.K_1_Fast:
                    return 0;
                case ScaleFunction.K_2:
                    return 0;
                case ScaleFunction.K_2_NO_NORM:
                    return 0;
                case ScaleFunction.K_3:
                    return 0;
                case ScaleFunction.K_3_NO_NORM:
                    return 0;
                default:
                    return 0;
            }
        }
        public static double K(this ScaleFunction scaleFunction, double q, double normalizer)
        {
            switch (scaleFunction)
            {
                case ScaleFunction.K_0:
                    return normalizer * q;
                case ScaleFunction.K_1:
                    return 0;
                case ScaleFunction.K_1_Fast:
                    return 0;
                case ScaleFunction.K_2:
                    return 0;
                case ScaleFunction.K_2_NO_NORM:
                    return 0;
                case ScaleFunction.K_3:
                    return 0;
                case ScaleFunction.K_3_NO_NORM:
                    return 0;
                default:
                    return 0;
            }
        }

        public static double Q(this ScaleFunction scaleFunction, double k, double compression, double n)
        {
            switch (scaleFunction)
            {
                case ScaleFunction.K_0:
                    return 2 * k / compression;
                case ScaleFunction.K_1:
                    return 0;
                case ScaleFunction.K_1_Fast:
                    return 0;
                case ScaleFunction.K_2:
                    return 0;
                case ScaleFunction.K_2_NO_NORM:
                    return 0;
                case ScaleFunction.K_3:
                    return 0;
                case ScaleFunction.K_3_NO_NORM:
                    return 0;
                default:
                    return 0;
            }
        }

        public static double Q(this ScaleFunction scaleFunction, double k, double normalizer)
        {
            switch (scaleFunction)
            {
                case ScaleFunction.K_0:
                    return k / normalizer;
                case ScaleFunction.K_1:
                    return 0;
                case ScaleFunction.K_1_Fast:
                    return 0;
                case ScaleFunction.K_2:
                    return 0;
                case ScaleFunction.K_2_NO_NORM:
                    return 0;
                case ScaleFunction.K_3:
                    return 0;
                case ScaleFunction.K_3_NO_NORM:
                    return 0;
                default:
                    return 0;
            }
        }

        public static double Max(this ScaleFunction scaleFunction, double q, double compression, double n)
        {
            switch (scaleFunction)
            {
                case ScaleFunction.K_0:
                    return 2 / compression;
                case ScaleFunction.K_1:
                    return 0;
                case ScaleFunction.K_1_Fast:
                    return 0;
                case ScaleFunction.K_2:
                    return 0;
                case ScaleFunction.K_2_NO_NORM:
                    return 0;
                case ScaleFunction.K_3:
                    return 0;
                case ScaleFunction.K_3_NO_NORM:
                    return 0;
                default:
                    return 0;
            }
        }

        public static double Max(this ScaleFunction scaleFunction, double q, double normalizer)
        {
            switch (scaleFunction)
            {
                case ScaleFunction.K_0:
                    return 1 / normalizer;
                case ScaleFunction.K_1:
                    return 0;
                case ScaleFunction.K_1_Fast:
                    return 0;
                case ScaleFunction.K_2:
                    return 0;
                case ScaleFunction.K_2_NO_NORM:
                    return 0;
                case ScaleFunction.K_3:
                    return 0;
                case ScaleFunction.K_3_NO_NORM:
                    return 0;
                default:
                    return 0;
            }
        }

        public static double Normalizer(this ScaleFunction scaleFunction, double compresison, double n)
        {
            switch (scaleFunction)
            {
                case ScaleFunction.K_0:
                    return compresison / 2;
                case ScaleFunction.K_1:
                    return 0;
                case ScaleFunction.K_1_Fast:
                    return 0;
                case ScaleFunction.K_2:
                    return 0;
                case ScaleFunction.K_2_NO_NORM:
                    return 0;
                case ScaleFunction.K_3:
                    return 0;
                case ScaleFunction.K_3_NO_NORM:
                    return 0;
                default:
                    return 0;
            }
        }
    }
}
