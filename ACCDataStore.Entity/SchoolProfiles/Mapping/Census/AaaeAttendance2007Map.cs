using ACCDataStore.Entity.SchoolProfiles;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Mapping.Census
{
    public class AaaeAttendance2007Map: ClassMap<AaeAttendance2007>
    {

        public AaaeAttendance2007Map() 
        {
            Table("aae_attendance_2007");
            Id(x => x.ID).Column("ID");
            //Map(x => x.RecordId);
            //Map(x => x.LACode);
            Map(x => x.SeedCode);
            Map(x => x.StudentStage);
            //Map(x => x.StudentId);
            //Map(x => x.Term);
            Map(x => x.AttendanceCode);
            //Map(x => x.HalfDays);
            Map(x => x.Total);
        
        }
    }

    public class AaaeAttendance2008Map : ClassMap<AaeAttendance2008>
    {

        public AaaeAttendance2008Map()
        {
            Table("aae_attendance_2008");
            Id(x => x.ID).Column("ID");
            //Map(x => x.RecordId);
            //Map(x => x.LACode);
            Map(x => x.SeedCode);
            Map(x => x.StudentStage);
            //Map(x => x.StudentId);
            //Map(x => x.Term);
            Map(x => x.AttendanceCode);
            //Map(x => x.HalfDays);
            Map(x => x.Total);
        }
    }

    public class AaaeAttendance2009Map : ClassMap<AaeAttendance2009>
    {

        public AaaeAttendance2009Map()
        {
            Table("aae_attendance_2009");
            Id(x => x.ID).Column("ID");
            //Map(x => x.RecordId);
            //Map(x => x.LACode);
            Map(x => x.SeedCode);
            Map(x => x.StudentStage);
            //Map(x => x.StudentId);
            //Map(x => x.Term);
            Map(x => x.AttendanceCode);
            //Map(x => x.HalfDays);
            Map(x => x.Total);
        }
    }

    public class AaaeAttendance2010Map : ClassMap<AaeAttendance2010>
    {

        public AaaeAttendance2010Map()
        {
            Table("aae_attendance_2010");
            Id(x => x.ID).Column("ID");
            //Map(x => x.RecordId);
            //Map(x => x.LACode);
            Map(x => x.SeedCode);
            Map(x => x.StudentStage);
            //Map(x => x.StudentId);
            //Map(x => x.Term);
            Map(x => x.AttendanceCode);
            //Map(x => x.HalfDays);
            Map(x => x.Total);
        }
    }

    public class AaaeAttendance2011Map : ClassMap<AaeAttendance2011>
    {

        public AaaeAttendance2011Map()
        {
            Table("aae_attendance_2011");
            Id(x => x.ID).Column("ID");
            //Map(x => x.RecordId);
            //Map(x => x.LACode);
            Map(x => x.SeedCode);
            Map(x => x.StudentStage);
            //Map(x => x.StudentId);
            //Map(x => x.Term);
            Map(x => x.AttendanceCode);
            //Map(x => x.HalfDays);
            Map(x => x.Total);
        }
    }

    public class AaaeAttendance2012Map : ClassMap<AaeAttendance2012>
    {

        public AaaeAttendance2012Map()
        {
            Table("aae_attendance_2012");
            Id(x => x.ID).Column("ID");
            //Map(x => x.RecordId);
            //Map(x => x.LACode);
            Map(x => x.SeedCode);
            Map(x => x.StudentStage);
            //Map(x => x.StudentId);
            //Map(x => x.Term);
            Map(x => x.AttendanceCode);
            //Map(x => x.HalfDays);
            Map(x => x.Total);
        }
    }

    public class AaaeAttendance2013Map : ClassMap<AaeAttendance2013>
    {

        public AaaeAttendance2013Map()
        {
            Table("aae_attendance_2013");
            Id(x => x.ID).Column("ID");
            //Map(x => x.RecordId);
            //Map(x => x.LACode);
            Map(x => x.SeedCode);
            Map(x => x.StudentStage);
            //Map(x => x.StudentId);
            //Map(x => x.Term);
            Map(x => x.AttendanceCode);
            //Map(x => x.HalfDays);
            Map(x => x.Total);
        }
    }

    public class AaaeAttendance2014Map : ClassMap<AaeAttendance2014>
    {

        public AaaeAttendance2014Map()
        {
            Table("aae_attendance_2014");
            Id(x => x.ID).Column("ID");
            //Map(x => x.RecordId);
            //Map(x => x.LACode);
            Map(x => x.SeedCode);
            Map(x => x.StudentStage);
            //Map(x => x.StudentId);
            //Map(x => x.Term);
            Map(x => x.AttendanceCode);
            //Map(x => x.HalfDays);
            Map(x => x.Total);
        }
    }

    public class AaaeAttendance2015Map : ClassMap<AaeAttendance2015>
    {

        public AaaeAttendance2015Map()
        {
            Table("aae_attendance_2015");
            Id(x => x.ID).Column("ID");
            //Map(x => x.RecordId);
            //Map(x => x.LACode);
            Map(x => x.SeedCode);
            Map(x => x.StudentStage);
            //Map(x => x.StudentId);
            //Map(x => x.Term);
            Map(x => x.AttendanceCode);
            //Map(x => x.HalfDays);
            Map(x => x.Total);
        }
    }
}
