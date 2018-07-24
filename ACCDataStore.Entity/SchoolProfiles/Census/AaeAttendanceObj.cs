using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class AaeAttendanceObj:BaseEntity
    {
        public virtual int ID { get; set; }
        //public virtual int RecordId { get; set; }
        //public virtual string LACode { get; set; }
        public virtual int SeedCode { get; set; }
        public virtual string StudentStage { get; set; }
        //public virtual string StudentId { get; set; }
        //public virtual string Term { get; set; }
        public virtual string AttendanceCode { get; set; }
        //public virtual int HalfDays { get; set; }
        public virtual int Total { get; set; }
    }

    public class AaeAttendance2007 : AaeAttendanceObj
    {
    }

    public class AaeAttendance2008 : AaeAttendanceObj
    {
    }
    public class AaeAttendance2009 : AaeAttendanceObj
    {
    }
    public class AaeAttendance2010 : AaeAttendanceObj
    {
    }
    public class AaeAttendance2011 : AaeAttendanceObj
    {
    }
    public class AaeAttendance2012 : AaeAttendanceObj
    {
    }
    public class AaeAttendance2013 : AaeAttendanceObj
    {
    }
    public class AaeAttendance2014 : AaeAttendanceObj
    {
    }
    public class AaeAttendance2015 : AaeAttendanceObj
    {
    }
}
