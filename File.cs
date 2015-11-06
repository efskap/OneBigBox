using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBigBox
{
    struct File
    {
        public List<Region> regions;
        public int length;
        public string name;
        public File(List<Region> regions, int length, string name)
        {
            this.regions = regions;
            this.length = length;
            this.name = name;

        }

        public override string ToString()
        {
            return name + "->" + regions.Select(x => x.ToString()).Aggregate((a, b) => a + ", " + b);
        }
    }
}
