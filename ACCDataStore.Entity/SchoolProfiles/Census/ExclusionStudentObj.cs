using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class ExclusionStudentObj : BaseEntity
    {
        public virtual int ID { get; set; }
        public virtual int RecordId { get; set; }
        public virtual string LACode { get; set; }
        public virtual int SeedCode { get; set; }
        public virtual string StudentId { get; set; }
        public virtual string ForeName { get; set; }
        public virtual string SurName { get; set; }
        public virtual string ScottishCandidateNumber { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual string Gender { get; set; }
        public virtual string StudentStage { get; set; }
        public virtual string StudentStatus { get; set; }

        public virtual string IncidentInClass { get; set; }
        public virtual string RemovedFromRegister { get; set; }
        public virtual DateTime FinishDate { get; set; }
        public virtual int LengthOfExclusion { get; set; }
        public virtual string Appeal { get; set; }
        public virtual int NoProvisionDays { get; set; }
        public virtual string StudentLookedAfter { get; set; }
        public virtual string StudentIncidentId { get; set; }

    }

    public class ExclusionStudent2008 : ExclusionStudentObj
    {
    }

    public class ExclusionStudent2009 : ExclusionStudentObj
    {
    }

    public class ExclusionStudent2010 : ExclusionStudentObj
    {
    }

    public class ExclusionStudent2011 : ExclusionStudentObj
    {
    }

    public class ExclusionStudent2012 : ExclusionStudentObj
    {
    }

    public class ExclusionStudent2013 : ExclusionStudentObj
    {
    }

    public class ExclusionStudent2014 : ExclusionStudentObj
    {
    }

    public class ExclusionStudent2015 : ExclusionStudentObj
    {
    }
}
