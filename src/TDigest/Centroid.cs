using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Shirhatti.Math.Stats
{
    public class Centroid : IComparable<Centroid>
    {
        // _uniqueCount is an atomic integer and should only be accessed via atomic operations
        private static int _uniqueCount;

        private double _centroid = 0;

        private List<double> _actualData = null;

        // TODO: Can I replace with Lazy<T>?
        public List<double> Data() => _actualData;

        public int Count { get; private set; } = 0;

        // Id is a transient int.
        // TODO: Find out if this matters?
        public int Id { get; private set; }

        private Centroid(bool record)
        {
            Id = Interlocked.Increment(ref _uniqueCount);
            if (record)
            {
                _actualData = new List<double>();
            }
        }

        public Centroid(double x) : this(false)
        {
            Start(x, 1, Interlocked.Increment(ref _uniqueCount));
        }

        public Centroid(double x, int w) : this(false)
        {
            Start(x, w, Interlocked.Increment(ref _uniqueCount));
        }

        public Centroid(double x, int w, int id) : this(false)
        {
            Start(x, w, id);
        }

        public Centroid(double x, int id, bool record) : this(record)
        {
            Start(x, 1, id);
        }

        public Centroid(double x, int w, List<double> data) : this(x, w)
        {
            _actualData = data;
        }

        private void Start(double x, int w, int id)
        {
            Id = id;
            Add(x, w);
        }

        public void Add(double x, int w)
        {
            if (_actualData != null)
            {
                _actualData.Add(x);
            }
            Count += w;
            _centroid += w * (x - _centroid) / Count;
        }

        public double Mean => _centroid;

        public override string ToString()
        {
            return "Centroid{" +
                    "centroid=" + _centroid +
                    ", count=" + Count +
                    '}';
        }

        public override int GetHashCode() => Id;

        public int CompareTo(Centroid other)
        {
            var r = _centroid.CompareTo(other._centroid);
            if (r == 0)
            {
                r = Id - other.Id;
            }
            return r;
        }

        public void InsertData(double x)
        {
            if (_actualData == null)
            {
                _actualData = new List<double>();
            }
            _actualData.Add(x);
        }

        public static Centroid CreateWeighted(double x, int w, IEnumerable<double> data)
        {
            var r = new Centroid(data != null);
            r.Add(x, w, data);
            return r;
        }

        public void Add(double x, int w, IEnumerable<double> data)
        {
            if (_actualData != null)
            {
                if (data != null)
                {
                    _actualData.AddRange(data);
                }
                else
                {
                    _actualData.Add(x);
                }
            }
            //_centroid = AbstractTDigest.weightedAverage(_centroid, Count, x, w);
            Count += w;
        }
    }
}
