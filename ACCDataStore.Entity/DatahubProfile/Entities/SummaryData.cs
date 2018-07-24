using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile
{
    public class SummaryData : BaseEntity
    {
        public virtual int id { get; set; }
        public virtual string name { get; set; }
        public virtual string dataCode { get; set; }
        public virtual int allMale { get; set; }
        public virtual int allFemale {get; set;}
        public virtual int allUnspecified { get; set; }
        public virtual int all15Male { get; set; }
        public virtual int all15Female { get; set; }
        public virtual int all15Unspecified { get; set; }
        public virtual int all16Male { get; set; }
        public virtual int all16Female { get; set; }
        public virtual int all16Unspecified { get; set; }
        public virtual int all17Male { get; set; }
        public virtual int all17Female { get; set; }
        public virtual int all17Unspecified { get; set; }
        public virtual int all18Male { get; set; }
        public virtual int all18Female { get; set; }
        public virtual int all18Unspecified { get; set; }
        public virtual int all19Male { get; set; }
        public virtual int all19Female { get; set; }
        public virtual int all19Unspecified { get; set; }
        public virtual int schoolMale { get; set; }
        public virtual int schoolFemale { get; set; }
        public virtual int schoolUnspecified { get; set; }
        public virtual int schoolInTransitionMale { get; set; }
        public virtual int schoolInTransitionFemale { get; set; }
        public virtual int schoolInTransitionUnspecified { get; set; }
        public virtual int movedOutScotlandMale { get; set; }
        public virtual int movedOutScotlandFemale { get; set; }
        public virtual int movedOutScotlandUnspecified { get; set; }
        public virtual int activityAgreementMale { get; set; }
        public virtual int activityAgreementFemale { get; set; }
        public virtual int activityAgreementUnspecified { get; set; }
        public virtual int fundStage2Male { get; set; }
        public virtual int fundStage2Female { get; set; }
        public virtual int fundStage2Unspecified { get; set; }
        public virtual int fundStage3Male { get; set; }
        public virtual int fundStage3Female { get; set; }
        public virtual int fundStage3Unspecified { get; set; }
        public virtual int fundStage4Male { get; set; }
        public virtual int fundStage4Female { get; set; }
        public virtual int fundStage4Unspecified { get; set; }
        public virtual int fullTimeEmploymentMale { get; set; }
        public virtual int fullTimeEmploymentFemale { get; set; }
        public virtual int fullTimeEmploymentUnspecified { get; set; }
        public virtual int furtherEducationMale { get; set; }
        public virtual int furtherEducationFemale { get; set; }
        public virtual int furtherEducationUnspecified { get; set; }
        public virtual int higherEducationMale { get; set; }
        public virtual int higherEducationFemale { get; set; }
        public virtual int higherEducationUnspecified { get; set; }
        public virtual int modernApprenticeshipMale { get; set; }
        public virtual int modernApprenticeshipFemale { get; set; }
        public virtual int modernApprenticeshipUnspecified { get; set; }
        public virtual int parttimeEmploymentMale { get; set; }
        public virtual int parttimeEmploymentFemale { get; set; }
        public virtual int parttimeEmploymentUnspecified { get; set; }
        public virtual int personalDevelopmentMale { get; set; }
        public virtual int personalDevelopmentFemale { get; set; }
        public virtual int personalDevelopmentUnspecified { get; set; }
        public virtual int selfEmployedMale { get; set; }
        public virtual int selfEmployedFemale { get; set; }
        public virtual int selfEmployedUnspecified{ get; set; }
        public virtual int trainingMale { get; set; }
        public virtual int trainingFemale { get; set; }
        public virtual int trainingUnspecified { get; set; }
        public virtual int voluntaryWorkMale { get; set; }
        public virtual int voluntaryWorkFemale { get; set; }
        public virtual int voluntaryWorkUnspecified { get; set; }
        public virtual double AvgWeek { get; set; }
        public virtual int custodyMale { get; set; }
        public virtual int custodyFemale { get; set; }
        public virtual int custodyUnspecified { get; set; }
        public virtual int economicallyInactiveMale { get; set; }
        public virtual int economicallyInactiveFemale { get; set; }
        public virtual int economicallyInactiveUnspecified { get; set; }
        public virtual int illHealthMale { get; set; }
        public virtual int illHealthFemale { get; set; }
        public virtual int illHealthUnspecified { get; set; }
        public virtual int unemployedMale { get; set; }
        public virtual int unemployedFemale { get; set; }
        public virtual int unemployedUnspecified { get; set; }
        public virtual int unknownMale { get; set; }
        public virtual int unknownFemale { get; set; }
        public virtual int unknownUnspecified { get; set; }
        public virtual string type { get; set; }
        public virtual int dataMonth { get; set; }
        public virtual int dataYear { get; set; }
    }
    public class AberdeenSummary : SummaryData { }
    public class GlasgowSummary : SummaryData { }
    public class AberdeenshireSummary : SummaryData { }
}
