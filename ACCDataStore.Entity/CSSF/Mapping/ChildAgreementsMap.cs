using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.CSSF.Mapping
{
    public class ChildAgreementsMap: ClassMap<ChildAgreements>
    {
        public ChildAgreementsMap()
        {
            Table("cssf_child_agreements");
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.client_id);
            Map(x => x.agreement_id);
            Map(x => x.authorisation_status);
            Map(x => x.supplier_code);
            Map(x => x.supplier_name);
            Map(x => x.active_weeks_cost);
            Map(x => x.agreement_started);
            Map(x => x.agreement_ended);
            Map(x => x.Placement_Category);
            Map(x => x.Service_Type);
            Map(x => x.dataset);
        }
    }
}
