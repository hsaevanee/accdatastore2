using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile
{
    public class AllSchools : BaseEntity
    {
        public virtual int id { get; set; }
        public virtual string seedCode { get; set; }
        public virtual string name { get; set; }
        public virtual string referenceCouncil { get; set; }
    }
}
