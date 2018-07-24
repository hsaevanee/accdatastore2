using ACCDataStore.Entity.DatahubProfile;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile
{
    public class NeighbourhoodMap : ClassMap<NeighbourhoodObj>
    {
        public NeighbourhoodMap()
        {
            Table("neighbourhood_postcodes1");
            Id(x => x.Id).Column("Id");
            Map(x => x.CSS_Postcode);
            Map(x => x.DataZone);
            Map(x => x.IntDataZone);
            Map(x => x.Neighbourhood);
            Map(x => x.Ref_No);
        }
    }
}
