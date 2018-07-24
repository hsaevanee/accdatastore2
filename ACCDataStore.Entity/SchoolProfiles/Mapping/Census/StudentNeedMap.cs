using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Mapping
{
    public class StudentNeed2010Map : ClassMap<StudentNeed2010>
    {
        public StudentNeed2010Map()
        {
            Table("sch_studentneed_2010");
            Id(x => x.ID).Column("ID");
            Map(x => x.SeedCode);
            Map(x => x.StudentID);
            Map(x => x.NeedType);
            Map(x => x.StudentStatus);
            Map(x => x.StudentStage);
        }

    }

    public class StudentNeed2011Map : ClassMap<StudentNeed2011>
    {
        public StudentNeed2011Map()
        {
            Table("sch_studentneed_2011");
            Id(x => x.ID).Column("ID");
            Map(x => x.SeedCode);
            Map(x => x.StudentID);
            Map(x => x.NeedType);
            Map(x => x.StudentStatus);
            Map(x => x.StudentStage);
        }

    }

    public class StudentNeed2012Map : ClassMap<StudentNeed2012>
    {
        public StudentNeed2012Map()
        {
            Table("sch_studentneed_2012");
            Id(x => x.ID).Column("ID");
            Map(x => x.SeedCode);
            Map(x => x.StudentID);
            Map(x => x.NeedType);
            Map(x => x.StudentStatus);
            Map(x => x.StudentStage);
        }

    }

    public class StudentNeed2013Map : ClassMap<StudentNeed2013>
    {
        public StudentNeed2013Map()
        {
            Table("sch_studentneed_2013");
            Id(x => x.ID).Column("ID");
            Map(x => x.SeedCode);
            Map(x => x.StudentID);
            Map(x => x.NeedType);
            Map(x => x.StudentStatus);
            Map(x => x.StudentStage);
        }

    }

    public class StudentNeed2014Map : ClassMap<StudentNeed2014>
    {
        public StudentNeed2014Map()
        {
            Table("sch_studentneed_2014");
            Id(x => x.ID).Column("ID");
            Map(x => x.SeedCode);
            Map(x => x.StudentID);
            Map(x => x.NeedType);
            Map(x => x.StudentStatus);
            Map(x => x.StudentStage);
        }

    }

    public class StudentNeed2015Map : ClassMap<StudentNeed2015>
    {
        public StudentNeed2015Map()
        {
            Table("sch_studentneed_2015");
            Id(x => x.ID).Column("ID");
            Map(x => x.SeedCode);
            Map(x => x.StudentID);
            Map(x => x.NeedType);
            Map(x => x.StudentStatus);
            Map(x => x.StudentStage);
        }

    }

    public class StudentNeed2016Map : ClassMap<StudentNeed2016>
    {
        public StudentNeed2016Map()
        {
            Table("sch_studentneed_2016");
            Id(x => x.ID).Column("ID");
            Map(x => x.SeedCode);
            Map(x => x.StudentID);
            Map(x => x.NeedType);
            Map(x => x.StudentStatus);
            Map(x => x.StudentStage);
        }

    }
}
