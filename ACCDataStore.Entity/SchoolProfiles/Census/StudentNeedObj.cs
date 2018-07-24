using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class StudentNeedObj : BaseEntity
    {
        public virtual int ID { get; set; }
        //public virtual string LaCode { get; set; }
        public virtual int SeedCode { get; set; }
        public virtual string StudentID { get; set; }
        public virtual string NeedType { get; set; }
        public virtual string StudentStatus { get; set; }
        public virtual string StudentStage { get; set; }
    }


    public class StudentNeed2010 : StudentNeedObj
    {
    }

    public class StudentNeed2011 : StudentNeedObj
    {
    }

    public class StudentNeed2012 : StudentNeedObj
    {
    }

    public class StudentNeed2013 : StudentNeedObj
    {
    }

    public class StudentNeed2014 : StudentNeedObj
    {
    }

    public class StudentNeed2015 : StudentNeedObj
    {
    }

    public class StudentNeed2016 : StudentNeedObj
    {
    }
}
