using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBigBox
{
    struct Loc 
    {
        public Block block;
        public int pos;
        public Loc(Block block, int pos)
        {
            this.block = block;
            this.pos = pos;
        }
    }
}
