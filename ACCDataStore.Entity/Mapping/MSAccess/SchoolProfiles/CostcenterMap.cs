using ACCDataStore.Entity.SchoolProfiles;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess.SchoolProfile
{
    public class CostcenterMap : ClassMap<Costcentre>
    {
        public CostcenterMap() {
            Table("View_Costcentre"); //from Scotxed_15 database
            Id(x => x.CostCentreKey);
            Map(x => x.auth);
            Map(x => x.main);
            Map(x => x.cost);
            Map(x => x.name);
            Map(x => x.sed_no);
            Map(x => x.schoolType_id);
            Map(x => x.CostCentreKey);
            Map(x => x.ClickNGo);  
        }
    }
}
