using ACCDataStore.Entity.DatahubProfile;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess
{
    class IntermediateZoneMap : ClassMap<IntermediateZoneObj>
    {
        public IntermediateZoneMap()
        {
            Table("map_data_intermediate_zone");
            Id(x => x.Reference).Column("Reference");
            Map(x => x.Name);
            Map(x => x.Reference_Council);
            Map(x => x.GeoJSON);
        }
    }
}
