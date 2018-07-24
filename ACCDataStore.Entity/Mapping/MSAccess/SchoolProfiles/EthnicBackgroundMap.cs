using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using ACCDataStore.Entity.SchoolProfiles;

namespace ACCDataStore.Entity.Mapping.MSAccess.SchoolProfiles
{
    public class EthnicBackgroundMap : ClassMap<EthnicBackground>
    {
        public EthnicBackgroundMap() {
            Table("Lu_EthnicBackground"); //from Scotxed_15 database
            Id(x => x.ScotXedcode);
            Map(x => x.code);
            Map(x => x.value);
            Map(x => x.ScotXedcode);  
        }
    }
}
