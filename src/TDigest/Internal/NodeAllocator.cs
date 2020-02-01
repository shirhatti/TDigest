using System.Collections.Generic;
using System.Diagnostics;

namespace Shirhatti.Math.Stats.Internal
{
    internal class NodeAllocator
    {
        private static readonly int NIL = 0;
        private int _nextNode;
        private Stack<int> _releasedNodes;

        internal NodeAllocator()
        {
            _nextNode = NIL + 1;
            _releasedNodes = new Stack<int>();
        }

        public int NewNode()
        {
            if (_releasedNodes.Count > 0)
            {
                return _releasedNodes.Pop();
            }
            else
            {
                return _nextNode++;
            }
        }

        public void Release(int node)
        {
            Debug.Assert(node < _nextNode);
            _releasedNodes.Push(node);
        }

        public int Size()
        {
            return _nextNode - _releasedNodes.Count - 1;
        }

    }
}
