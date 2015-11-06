using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBigBox
{
    class FileSystem
    {
        public Loc loc;
        public HashSet<Block> blockPool;
      

        public List<File> ToC;
        public FileSystem()
        {
            blockPool = new HashSet<Block>();
            ToC = new List<File>();
        
        }
        public void add(Block block)
        {
            blockPool.Add(block);
        }
        public void open(Block block)
        {
            loc = new Loc(block, 0);
            blockPool.Add(block);
        }
     

        public File GetFile(string name)
        {
            return ToC.FirstOrDefault(x => x.name == name);
        }
        public byte[] readFileBytes(File f)
        {
            byte[] o = new byte[f.length];
            int m = 0;
            foreach(Region r in f.regions)
            {
                for (int i = r.a; i < r.b; i ++)
                    o[m++] = r.block.get(i);
            }
            return o;
        }
  
        public void writeNext()
        {

        }
        List<Block> getFreeBlocks()
        {
            return blockPool.Where((x) => !x.isFull).OrderByDescending((x) => x.usedSpace).ToList();
        }
        void ensureFreeSpace()
        {
            if (loc.block.isFull)
            {
                var free = getFreeBlocks();
                if (free.Count < 1) throw new Exception("All full!");
                loc.block = free[0];
                loc.pos = 0;
            }
        }


        public Region findSmallestFreeSpace()
        {

            List<Region> regs = new List<Region>();
            foreach (var block in getFreeBlocks())
            {
                regs.Add(block.GetSmallestRegion());
            }
            return regs.Min();

        }



        public File writeFile(string name, byte[] bytes)
        {
            int ii = 0;

            Region currentReg;
            List<Region> regions = new List<Region>();
            while (ii < bytes.Length)
            {
                Loc oldLoc = loc;
                ensureFreeSpace();
                currentReg = findSmallestFreeSpace();
                loc.pos = currentReg.a;
                loc.block = currentReg.block;
                if (ii == 0)
                {

                    // done here after we find an initial writing pos
                    //   loc.block.ToC.Add((File)receipt);
                }
                else
                {
               //     oldLoc.block.FileSystem.addJump(oldLoc, loc);
                }
                int z = 0;
                while (loc.pos < currentReg.b && ii < bytes.Length)
                {
                    loc.block.writeByte(loc.pos++, bytes[ii++]);
                    z++;
                }
                regions.Add((loc.pos < currentReg.b) ? new Region(loc.block, currentReg.a, currentReg.a + z) : currentReg);

            }
            File f = new File(regions, bytes.Length, name);
            ToC.Add(f);
            return f;

        }
        public void delete(File f)
        {
            int z = 0;
            ToC.Remove(f);
            HashSet<Block> blocks = new HashSet<Block>();
            foreach (Region r in f.regions)
            {
                r.block.freeRegion(r);
                blocks.Add(r.block);

            }
            foreach (Block b in blocks)
            {
                b.MergeRegions();
            }
        }

        private void deleteRegion(Region region)
        {
            // HELP
        }
    }
}

