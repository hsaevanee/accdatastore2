using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census
{
    public class AaeAttendanceObjT:BaseEntity
    {
        public int ID { get; set; }
        //public virtual int RecordId { get; set; }
        //public virtual string LACode { get; set; }
        public  int SeedCode { get; set; }
        //public  string StudentId { get; set; }
        //public  string Term { get; set; }
        public  string AttendanceCode { get; set; }
        //public  int HalfDays { get; set; }
        //public string status { get; set; }
        public int Total { get; set; }

        public AaeAttendanceObjT(int seedcode, string attcode, int count)
        {
            this.SeedCode = seedcode;
            this.AttendanceCode = attcode;
            this.Total = count;

        }
    }



     
}
