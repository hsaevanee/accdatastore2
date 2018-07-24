using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess
{
    public class SchPrimarySchoolMap : ClassMap<SchPrimarySchool>
    {
        public SchPrimarySchoolMap()
        {
            Table("sch_PrimarySchool_t");
            Id(x => x.RecordId).Column("RecordId");
            Map(x => x.LaCode);
            Map(x => x.SeedCode);
            Map(x => x.SchoolFundingType);
            Map(x => x.Name);
            Map(x => x.OpeningsPerWeek);
            Map(x => x.UsingBS7666AddressFormat);
            Map(x => x.SchoolBoardType);
            Map(x => x.DateOfLastElection);
        }
    }
}
