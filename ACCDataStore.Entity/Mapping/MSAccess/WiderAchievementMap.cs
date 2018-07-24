using ACCDataStore.Entity.SchoolProfile;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess
{
    public class WiderAchievementMap : ClassMap<WiderAchievementObj>
    {
        public WiderAchievementMap()
        {
            Table("WiderAchievementdata");
            Id(x => x.ID).Column("Id");
            Map(x => x.centre);
            Map(x => x.age_range);
            Map(x => x.awardname);
            Map(x => x.scqf_rating);
            Map(x => x.post_out);
            Map(x => x.post_in);
            Map(x => x.gender);
            Map(x => x.award2013);
            Map(x => x.award2014);
            Map(x => x.award2015);
            Map(x => x.award2016);  
        }
    }
}
