using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.CSSF 
{
    public class Placements : BaseEntity
    {
        public virtual int id { get; set; }
        public virtual string code { get; set; }
        public virtual string name { get; set; }
        public virtual string lacode { get; set; }
    }
}
