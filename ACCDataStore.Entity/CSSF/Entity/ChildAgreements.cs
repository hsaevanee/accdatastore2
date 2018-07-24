using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.CSSF
{
    public class ChildAgreements : BaseEntity
    {
         public virtual int Id { get; set; }
         public virtual string client_id {get;set;}
         public virtual string agreement_id { get; set; }
         public virtual string supplier_code { get; set; }
         public virtual int authorisation_status { get; set; }
         public virtual double active_weeks_cost { get; set; }
         public virtual DateTime agreement_started { get; set; }
         public virtual DateTime agreement_ended { get; set; }
         public virtual string supplier_name { get; set; }
         public virtual string Placement_Category { get; set; }
         public virtual string Service_Type { get; set; }
         public virtual double actual_cost { get; set; }
         public virtual int numberofdays { get; set; }
         public virtual string payattension { get; set; }
         public virtual string dataset { get; set; }


    }
}
