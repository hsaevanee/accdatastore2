using ACCDataStore.Entity.CSSF;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.CSSF.Mapping
{
    public class PlacementsMap: ClassMap<Placements>
    {
        public PlacementsMap()
        {
            Table("cssf_placements");
            Id(x => x.id).GeneratedBy.Increment();
            Map(x => x.code);
            Map(x => x.name);
            Map(x => x.lacode);
        }
    }
}
