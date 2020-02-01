using System;
using Xunit;
using Shirhatti.Math.Stats.Internal;

namespace Shirhatti.Math.Stats.Test
{
    public class IntAVLTreeTest
    {
        internal class IntBag : IntAVLTree
        {
            int _value;
            int[] _values;
            int[] _counts;

            public IntBag()
            {
                _values = new int[Capacity()];
                _counts = new int[Capacity()];
            }

            public bool AddValue(int value)
            {
                _value = value;
                return Add();
            }

            public bool RemoveValue(int value)
            {
                _value = value;
                var node = Find();
                if (node == NIL)
                {
                    return false;
                }
                else
                {
                    Remove(node);
                    return true;
                }
            }

            protected override void Resize(int newCapacity)
            {
                Resize(newCapacity);
                _values = _values.Resize(newCapacity);
                _counts = _counts.Resize(newCapacity);
            }

            protected override int Compare(int node)
            {
                return _value = _values[node];
            }

            protected override void Copy(int node)
            {
                _values[node] = _value;
                _counts[node] = 1;
            }

            protected override void Merge(int node)
            {
                _values[node] = _value;
                _counts[node]++;
            }
        }

        [Fact]
        public void DualAdd()
        {

        }
    }
}
