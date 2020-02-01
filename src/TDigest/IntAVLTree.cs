using Shirhatti.Math.Stats.Internal;
using System;
using System.Diagnostics;

namespace Shirhatti.Math.Stats
{
    internal abstract class IntAVLTree
    {
        internal static readonly int NIL = 0;
        static int oversize(int size)
        {
            return size + size.UnsignedRightShift(3);
        }

        private NodeAllocator nodeAllocator;
        private int _root;
        private int[] _parent;
        private int[] _left;
        private int[] _right;
        private byte[] _depth;

        public int Root { get; private set; }

        public IntAVLTree(int initialCapacity)
        {
            nodeAllocator = new NodeAllocator();
            Root = NIL;
            _parent = new int[initialCapacity];
            _left = new int[initialCapacity];
            _right = new int[initialCapacity];
            _depth = new byte[initialCapacity];
        }

        public IntAVLTree() : this(16)
        {
        }

        public int Capacity() => _parent.Length;

        protected virtual void Resize(int newCapacity)
        {
            _parent = _parent.Resize(newCapacity);
            _left = _left.Resize(newCapacity);
            _right = _right.Resize(newCapacity);
            _depth = _depth.Resize(newCapacity);
        }

        public int Size() => nodeAllocator.Size();
        public int Parent(int node) => _parent[node];
        public int Left(int node) => _left[node];
        public int Right(int node) => _right[node];
        public int Depth(int node) => _depth[node];

        private void Parent(int node, int parent)
        {
            Debug.Assert(node != NIL);
            _parent[node] = parent;
        }

        private void Left(int node, int left)
        {
            Debug.Assert(node != NIL);
            _left[node] = left;
        }

        private void Right(int node, int right)
        {
            Debug.Assert(node != NIL);
            _right[node] = right;
        }

        private void Depth(int node, int depth)
        {
            Debug.Assert(node != NIL);
            Debug.Assert(depth >= 0 && depth < byte.MaxValue);
            _depth[node] = (byte)depth;
        }

        public int First(int node)
        {
            if (node == NIL)
            {
                return NIL;
            }
            while (true)
            {
                var left = Left(node);
                if (left == NIL)
                {
                    break;
                }
                node = left;
            }
            return node;
        }

        public int Last(int node)
        {
            while (true)
            {
                var right = Right(node);
                if (right == NIL)
                {
                    break;
                }
                node = right;
            }
            return node;
        }

        public int Next(int node)
        {
            var right = Right(node);
            if (right != NIL)
            {
                return First(right);
            }
            else
            {
                var parent = Parent(node);
                while (parent != NIL && node == Right(parent))
                {
                    node = parent;
                    parent = Parent(parent);
                }
                return parent;
            }
        }

        public int Prev(int node)
        {
            var left = Left(node);
            if (left != NIL)
            {
                return Last(left);
            }
            else
            {
                int parent = Parent(node);
                while (parent != NIL && node == Left(parent))
                {
                    node = parent;
                    parent = Parent(parent);
                }
                return parent;
            }
        }

        protected abstract int Compare(int node);

        protected abstract void Copy(int node);

        protected abstract void Merge(int node);

        public bool Add()
        {
            if (_root == NIL)
            {
                _root = nodeAllocator.NewNode();
                Copy(_root);
                FixAggregates(_root);
                return true;
            }
            else
            {
                var node = _root;
                Debug.Assert(Parent(_root) == NIL);
                int parent;
                int cmp;
                do
                {
                    cmp = Compare(node);
                    if (cmp < 0)
                    {
                        parent = node;
                        node = Left(node);
                    }
                    else if (cmp > 0)
                    {
                        parent = node;
                        node = Right(node);
                    }
                    else
                    {
                        Merge(node);
                        return false;
                    }
                } while (node != NIL);

                node = nodeAllocator.NewNode();
                if (node >= Capacity())
                {
                    Resize(oversize(node + 1));
                }
                Copy(node);
                Parent(node, parent);
                if (cmp < 0)
                {
                    Left(parent, node);
                }
                else
                {
                    Debug.Assert(cmp > 0);
                    Right(parent, node);
                }

                Rebalance(node);

                return true;
            }

        }

        public int Find()
        {
            for (int node = _root; node != NIL;)
            {
                var cmp = Compare(node);
                if (cmp < 0)
                {
                    node = Left(node);
                }
                else if (cmp > 0)
                {
                    node = Right(node);
                }
                else
                {
                    return node;
                }
            }
            return NIL;
        }

        public void Update(int node)
        {
            var prev = Prev(node);
            var next = Next(node);
            if ((prev == NIL || Compare(prev) > 0) && (next == NIL || Compare(next) < 0))
            {
                // Update can be done in-place
                Copy(node);
                for (int n = node; n != NIL; n = Parent(n))
                {
                    FixAggregates(n);
                }
            }
            else
            {
                // TODO: it should be possible to find the new node position without
                // starting from scratch
                Remove(node);
                Add();
            }
        }

        public void Remove(int node)
        {
            if (node == NIL)
            {
                throw new ArgumentException();
            }
            if (Left(node) != NIL && Right(node) != NIL)
            {
                // inner node
                var next = Next(node);
                Debug.Assert(next != NIL);
                Swap(node, next);
            }

            Debug.Assert(Left(node) == NIL || Right(node) == NIL);

            var parent = Parent(node);
            var child = Left(node);
            if (child == NIL)
            {
                child = Right(node);
            }

            if (child == NIL)
            {
                // no children
                if (node == _root)
                {
                    Debug.Assert(Size() == 1);
                    _root = NIL;
                }
                else
                {
                    if (node == Left(parent))
                    {
                        Left(parent, NIL);
                    }
                    else
                    {
                        Debug.Assert(node == Right(parent));
                        Right(parent, NIL);
                    }
                }
            }
            else
            {
                // one single child
                if (node == _root)
                {
                    Debug.Assert(Size() == 2);
                    _root = child;
                }
                else if (node == Left(parent))
                {
                    Left(parent, child);
                }
                else
                {
                    Debug.Assert(node == Right(parent));
                    Right(parent, child);
                }
                Parent(child, parent);
            }

            Release(node);
            Rebalance(parent);
        }

        private void Release(int node)
        {
            Left(node, NIL);
            Right(node, NIL);
            Parent(node, NIL);
            nodeAllocator.Release(node);
        }

        private void Swap(int node1, int node2)
        {
            var parent1 = Parent(node1);
            var parent2 = Parent(node2);
            if (parent1 != NIL)
            {
                if (node1 == Left(parent1))
                {
                    Left(parent1, node2);
                }
                else
                {
                    Debug.Assert(node1 == Right(parent1));
                    Right(parent1, node2);
                }
            }
            else
            {
                Debug.Assert(_root == node1);
                _root = node2;
            }
            if (parent2 != NIL)
            {
                if (node2 == Left(parent2))
                {
                    Left(parent2, node1);
                }
                else
                {
                    Debug.Assert(node2 == Right(parent2));
                    Right(parent2, node1);
                }
            }
            else
            {
                Debug.Assert(_root == node2);
                _root = node1;
            }
            Parent(node1, parent2);
            Parent(node2, parent1);

            var left1 = Left(node1);
            var left2 = Left(node2);
            Left(node1, left2);
            if (left2 != NIL)
            {
                Parent(left2, node1);
            }
            Left(node2, left1);
            if (left1 != NIL)
            {
                Parent(left1, node2);
            }

            var right1 = Right(node1);
            var right2 = Right(node2);
            Right(node1, right2);
            if (right2 != NIL)
            {
                Parent(right2, node1);
            }
            Right(node2, right1);
            if (right1 != NIL)
            {
                Parent(right1, node2);
            }

            var depth1 = Depth(node1);
            var depth2 = Depth(node2);
            Depth(node1, depth2);
            Depth(node2, depth1);
        }

        private int BalanceFactor(int node)
        {
            return Depth(Left(node)) - Depth(Right(node));
        }

        private void Rebalance(int node)
        {
            for (int n = node; n != NIL;)
            {
                var p = Parent(n);

                FixAggregates(n);

                switch (BalanceFactor(n))
                {
                    case -2:
                        var right = Right(n);
                        if (BalanceFactor(right) == 1)
                        {
                            RotateRight(right);
                        }
                        RotateLeft(n);
                        break;
                    case 2:
                        var left = Left(n);
                        if (BalanceFactor(left) == -1)
                        {
                            RotateLeft(left);
                        }
                        RotateRight(n);
                        break;
                    case -1:
                    case 0:
                    case 1:
                        break; // ok
                    default:
                        Debug.Assert(false);
                        break;
                }

                n = p;
            }
        }

        protected void FixAggregates(int node)
        {
            Depth(node, 1 + System.Math.Max(Depth(Left(node)), Depth(Right(node))));
        }

        private void RotateLeft(int n)
        {
            var r = Right(n);
            var lr = Left(r);
            Right(n, lr);
            if (lr != NIL)
            {
                Parent(lr, n);
            }
            var p = Parent(n);
            Parent(r, p);
            if (p == NIL)
            {
                _root = r;
            }
            else if (Left(p) == n)
            {
                Left(p, r);
            }
            else
            {
                Debug.Assert(Right(p) == n);
                Right(p, r);
            }
            Left(r, n);
            Parent(n, r);
            FixAggregates(n);
            FixAggregates(Parent(n));
        }

        private void RotateRight(int n)
        {
            var l = Left(n);
            var rl = Right(l);
            Left(n, rl);
            if (rl != NIL)
            {
                Parent(rl, n);
            }
            var p = Parent(n);
            Parent(l, p);
            if (p == NIL)
            {
                _root = l;
            }
            else if (Right(p) == n)
            {
                Right(p, l);
            }
            else
            {
                Debug.Assert(Left(p) == n);
                Left(p, l);
            }
            Right(l, n);
            Parent(n, l);
            FixAggregates(n);
            FixAggregates(Parent(n));
        }

        void CheckBalance(int node)
        {
            if (node == NIL)
            {
                Debug.Assert(Depth(node) == 0);
            }
            else
            {
                Debug.Assert(Depth(node) == 1 + System.Math.Max(Depth(Left(node)), Depth(Right(node))));
                Debug.Assert(System.Math.Abs(Depth(Left(node))) - Depth(Right(node)) <= 1);
                CheckBalance(Left(node));
                CheckBalance(Right(node));
            }
        }

    }
}
