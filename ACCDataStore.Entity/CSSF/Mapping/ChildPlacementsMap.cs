using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.CSSF.Mapping
{
    public class ChildPlacementsMap: ClassMap<ChildPlacements>
    {
        public ChildPlacementsMap()
        {
            Table("cssf_child_placements");
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.client_id);
            Map(x => x.dob);
            Map(x => x.gender);
            Map(x => x.placement_id);
            Map(x => x.placement_name);
            Map(x => x.placement_started);
            Map(x => x.placement_ended);
            Map(x => x.placement_code);
            Map(x => x.placement_category);
            Map(x => x.service_type);
            Map(x => x.dataset);
            //References(x => x.placements, "code");
        }
    }
}
