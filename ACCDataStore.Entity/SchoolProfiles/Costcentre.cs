using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class Costcentre:BaseEntity
    {
        public virtual string auth { get; set; }
        public virtual string main { get; set; }
        public virtual string cost { get; set; }
        public virtual string name { get; set; }
        public virtual string sed_no { get; set; }
        public virtual int schoolType_id { get; set; }
        public virtual int CostCentreKey { get; set; }
        public virtual int ClickNGo { get; set; }
    }
}
