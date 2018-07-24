using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ACCDataStore.Entity;

namespace ACCDataStore.Entity
{
    public class RightsCodeMap : ClassMap<RightsCode>
    {
        public RightsCodeMap()
        {
            Id(x => x.RightCode).GeneratedBy.Assigned();
            Map(x => x.RightDesc);
            Map(x => x.Ord);
        }
    }
}
