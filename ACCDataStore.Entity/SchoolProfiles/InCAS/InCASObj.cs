using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.InCAS
{
    public class InCASObj: BaseEntity
    {
        public virtual int ID { get; set; }
        public virtual string SchoolId { get; set; }
        public virtual string SoftwareVersion { get; set; }
        public virtual string UPN { get; set; }
        public virtual string YearGroup { get; set; }
        public virtual string ClassName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
        public virtual string Gender { get; set; }
        public virtual double? AgeAtTest_DevAbil { get; set; }
        public virtual double? AgeEquiv_DevAbil { get; set; }
        public virtual double? AgeDiff_DevAbil { get; set; }
        public virtual double? Standardised_DevAbil { get; set; }
        public virtual double? AgeAtTest_Reading { get; set; }
        public virtual double? AgeEquiv_Reading { get; set; }
        public virtual double? AgeDiff_Reading { get; set; }
        public virtual double? Standardised_Reading { get; set; }
        public virtual double? AgeAtTest_Spelling { get; set; }
        public virtual double? AgeEquiv_Spelling { get; set; }
        public virtual double? AgeDiff_Spelling { get; set; }
        public virtual double? AgeAtTest_MentArith { get; set; }
        public virtual double? AgeEquiv_MentArith { get; set; }
        public virtual double? AgeDiff_MentArith { get; set; }
        public virtual double? Standardised_MentArith { get; set; }
        public virtual double? AgeAtTest_GenMaths { get; set; }
        public virtual double? AgeEquiv_GenMaths { get; set; }
        public virtual double? AgeDiff_GenMaths { get; set; }
        public virtual double? Standardised_GenMaths { get; set; }

    }

    public class InCAS2012 : InCASObj
    {
    }
    public class InCAS2013 : InCASObj
    {
    }
    public class InCAS2014 : InCASObj
    {
    }
    public class InCAS2015 : InCASObj
    {
    }
}
