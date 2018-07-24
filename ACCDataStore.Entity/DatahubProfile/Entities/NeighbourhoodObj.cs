using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile
{
    public class NeighbourhoodObj : BaseEntity
    {
        public virtual int Id { get; set; }
        public virtual string CSS_Postcode { get; set; }
        public virtual string DataZone { get; set; }
        public virtual string IntDataZone { get; set; }
        public virtual string Neighbourhood { get; set; }
        public virtual string Ref_No { get; set; }
    }
    
}
