using ACCDataStore.Entity.SchoolProfile;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess
{
    public class LuEnglishLevelMap : ClassMap<LuEnglishLevel>
    {
        public  LuEnglishLevelMap(){
            Table("Lu_EnglishLevel");
            Id(x => x.ScotXedCode);
            Map(x => x.ScotXedCode);
            Map(x => x.Description);
        }
    }
}
