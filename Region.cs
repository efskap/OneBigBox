using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBigBox
{
    class Region : IComparable
    {
        public Block block;
        public int a;
        public int b;
        public int length => b - a;

        public Region(Block block, int a, int b)
        {
            if (a == b) throw new Exception("a==b");
            this.a = a;
            this.b = b;
            this.block = block;
        }
        // sort by starting index
        public int CompareTo(object obj)
        {
            Region r = (Region) obj;
            return Math.Sign(  a - r.a);
        }

        protected bool Equals(Region other)
        {
            return Equals(block, other.block) && a == other.a && b == other.b;
        }

        public bool Intersects(Region other)
        {
            return other.b >= a && b >= other.a;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (block != null ? block.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ a;
                hashCode = (hashCode*397) ^ b;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return block.ToString() + "["+ a + ":" + b+"]";
        }
    }
}
