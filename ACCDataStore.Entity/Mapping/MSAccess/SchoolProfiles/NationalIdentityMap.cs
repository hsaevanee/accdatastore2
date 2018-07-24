using ACCDataStore.Entity.SchoolProfiles;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess.SchoolProfiles
{
    public class NationalIdentityMap : ClassMap<NationalIdentity>
    {
        public NationalIdentityMap()
        {
            Table("Lu_NationalIdentity"); //from Scotxed_15 database
            Id(x => x.scotXedcode);
            Map(x => x.Code);
            Map(x => x.Description);
            Map(x => x.scotXedcode);  
        }
    }
}
