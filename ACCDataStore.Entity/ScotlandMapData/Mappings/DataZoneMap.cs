using ACCDataStore.Entity.DatahubProfile;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess
{
    class DataZoneMap : ClassMap<DataZoneObj>
    {
        public DataZoneMap()
        {
            Table("map_data_data_zone");
            Id(x => x.Reference).Column("Reference");
            Map(x => x.Reference_Parent);
            Map(x => x.Reference_Council);
            Map(x => x.GeoJSON);
        }
    }
}
