using ACCDataStore.Entity.DatahubProfile;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess
{
    class CouncilMap : ClassMap<CouncilObj>
    {
        public CouncilMap()
        {
            Table("map_data_councils");
            Id(x => x.Reference).Column("Reference");
            Map(x => x.Name);
            Map(x => x.GeoJSON);
        }
    }
}
