using ACCDataStore.Entity.SchoolProfiles;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess.SchoolProfiles
{
    public class EnglishLevelMap : ClassMap<EnglishLevel>
    {
        public EnglishLevelMap()
        {
            Table("Lu_EnglishLevel"); //from Scotxed_15 database
            Id(x => x.ScotXedCode);
            Map(x => x.Description);
        }
    }
}
