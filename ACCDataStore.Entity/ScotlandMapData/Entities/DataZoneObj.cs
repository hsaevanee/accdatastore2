using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile
{
    public class DataZoneObj : BaseEntity
    {
        public virtual string Reference { get; set; }
        public virtual string Reference_Parent { get; set; }
        public virtual string Reference_Council { get; set; }
        public virtual string GeoJSON { get; set; }
    }
}
