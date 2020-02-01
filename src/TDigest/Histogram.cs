using System;
using System.Collections.Generic;
using System.Text;

namespace Shirhatti.Math.Stats
{
    public abstract class Histogram
    {
        private long[] _counts;

        public long[] GetCounts()
        {
            return _counts;
        }

        protected double _min;
        protected double _max;
        protected double _logFactor;
        protected double _logOffset;

        public Histogram(double min, double max)
        {
            _min = min;
            _max = max;
        }

        protected void SetupBins(double min, double max)
        {
            int binCount = BucketIndex(max) + 1;
            if (binCount > 10000)
            {
                throw new ArgumentOutOfRangeException($"Excessive number of bins {binCount} resulting from min,max = {min}, {max}");
            }
            _counts = new long[binCount];
        }

        public void Add(double v)
        {
            GetCounts()[Bucket(v)]++;
        }

        public double[] GetBounds()
        {
            double[] r = new double[GetCounts().Length];
            for (int i = 0; i < r.Length; i++)
            {
                r[i] = LowerBound(i);
            }
            return r;
        }

        // exposed for testing
        int Bucket(double x)
        {
            if (x <= _min)
            {
                return 0;
            }
            else if (x >= _max)
            {
                return GetCounts().Length - 1;
            }
            else
            {
                return BucketIndex(x);
            }
        }

        protected abstract int BucketIndex(double x);

        // exposed for testing
        protected abstract double LowerBound(int k);

        protected abstract long[] GetCompressedCounts();

        protected abstract void Add(IEnumerable<Histogram> others);
    }
}
