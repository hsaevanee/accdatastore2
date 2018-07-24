using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfile
{
    public class WiderAchievementObj: BaseEntity
    {
        public virtual int ID { get; set; }
        public virtual string centre { get; set; }
        public virtual string age_range { get; set; }
        public virtual string awardname { get; set; }
        public virtual string scqf_rating { get; set; }
        public virtual string post_out { get; set; }
        public virtual string post_in { get; set; }
        public virtual string gender { get; set; }
        public virtual int award2013 { get; set; }
        public virtual int award2014 { get; set; }
        public virtual int award2015 { get; set; }
        public virtual int award2016 { get; set; }

    }
}
