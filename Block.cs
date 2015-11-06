using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBigBox
{
    // on the actual file system, a block is represented as a file.
    class Block
    {
        public byte[] data;
        public int length { get; }
        int used;
        string filepath;
        public FileSystem FileSystem { get; }

        bool cacheValid = false;
        private readonly FileSystem _fileSystem;
        public List<Region> freeSpace;
        public bool isFull
        {
            get
            {
                return used >= length;
            }
        }
        public int usedSpace
        {
            get
            {
                return used;
            }
        }

        public override string ToString()
        {
            return filepath;
        }

        public Block(string filepath, int length, FileSystem fs)
        {
            data = new byte[length];
            this.filepath = filepath;
            this.length = length;
            this.FileSystem = fs;
            freeSpace = new List<Region> { new Region(this, 0, length) };




            _fileSystem = new FileSystem();
        }

        public void MergeRegions()
        {
            List<Region> toRemove = new List<Region>();
            freeSpace.Sort();
            for (int i = 1; i < freeSpace.Count; i++)
            {
                Region r0 = freeSpace[i - 1];
                Region r1 = freeSpace[i];
                if (r0.b >= r1.b && r0.a <= r1.a)
                {
                    toRemove.Add(r1);
                }
                else if (r1.b >= r0.b && r1.a <= r0.a)  //idk 
                {
                    toRemove.Add(r0);
                }
                else if (r0.b == r1.a)
                {
                    r0.b = r1.b;
                    toRemove.Add(r1);
                }

            }
            foreach (var v in toRemove)
                freeSpace.Remove(v);
        }
        public byte get(int i)
        {
            return data[i];
        }

        public Region GetSmallestRegion()
        {
            return freeSpace.Min();
        }
        Region hitscanRegion(int i)
        {
            return freeSpace.FirstOrDefault(reg => i >= reg.a && i < reg.b);
        }
        IEnumerable<Region> hitscanRegion(Region r)
        {
            return freeSpace.Where(x => x.Intersects(r));
        }
        private void set(int i, byte data)
        {
            invaliateCache();
            Region hit = hitscanRegion(i);
            if (hit != null)
            {

                if (i > 0 && hit.a != i && hitscanRegion(i - 1) == hit)
                    freeSpace.Add(new Region(this, hit.a, i));
                if (i < hit.b && i+1 != hit.b)
                    freeSpace.Add(new Region(this, i + 1, hit.b));
                freeSpace.Remove(hit);
            }
            else
            {
                throw new Exception("overwriting region");
            }
            this.data[i] = data;
          
        }

        public void writeByte(int i, byte data)
        {
            used++;
            set(i, data);
        }
        // IMPORTANT: CALL MERGEREGIONS AFTER
        public void freeRegion(Region r)
        {
            freeSpace.Add(r);
            reportFreed(r.length);
        }
        private void invaliateCache()
        {
            cacheValid = false;
        }


        public void reportFreed(int i)
        {
            used -= i;
        }

      
        protected bool Equals(Block other)
        {
            return Equals(data, other.data) && string.Equals(filepath, other.filepath) && Equals(FileSystem, other.FileSystem);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = data?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (filepath != null ? filepath.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (FileSystem != null ? FileSystem.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
