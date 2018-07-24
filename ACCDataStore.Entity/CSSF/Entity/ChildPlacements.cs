using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.CSSF
{
    public class ChildPlacements : BaseEntity
    {
         public virtual int Id { get; set; }
         public virtual string client_id {get;set;}
         public virtual DateTime dob { get; set; }
         public virtual string gender { get; set; }
         public virtual string placement_id { get; set; }
         public virtual string placement_name { get; set; }
         public virtual DateTime placement_started { get; set; }
         public virtual DateTime placement_ended { get; set; }
         public virtual string placement_code { get; set; }
         public virtual string placement_category { get; set; }
         public virtual string service_type { get; set; }
         public virtual string payattension { get; set; }
         public virtual string dataset { get; set; }

    }
}
