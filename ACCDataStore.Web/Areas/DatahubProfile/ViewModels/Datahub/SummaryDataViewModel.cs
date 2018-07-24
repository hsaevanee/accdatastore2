using ACCDataStore.Entity.DatahubProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.ViewModels.Datahub
{
    public class SummaryDataViewModel
    {
        // We should be able to instantiate an empty symmarydataviewmodel if the summaryData that comes from the
        // database is null or an exception is thrown
        public SummaryDataViewModel() { }
        public SummaryDataViewModel(SummaryData summaryData)
        {
            this.name = summaryData.name;
            this.dataCode = summaryData.dataCode;
            this.allMale = summaryData.allMale;
            this.allFemale = summaryData.allFemale;
            this.allUnspecified = summaryData.allUnspecified;
            this.all15Male = summaryData.all15Male;
            this.all15Female = summaryData.all15Female;
            this.all15Unspecified = summaryData.all15Unspecified;
            this.all16Male = summaryData.all16Male;
            this.all16Female = summaryData.all16Female;
            this.all16Unspecified = summaryData.all16Unspecified;
            this.all17Male = summaryData.all17Male;
            this.all17Female = summaryData.all17Female;
            this.all17Unspecified = summaryData.all17Unspecified;
            this.all18Male = summaryData.all18Male;
            this.all18Female = summaryData.all18Female;
            this.all18Unspecified = summaryData.all18Unspecified;
            this.all19Male = summaryData.all19Male;
            this.all19Female = summaryData.all19Female;
            this.all19Unspecified = summaryData.all19Unspecified;
            this.schoolMale = summaryData.schoolMale;
            this.schoolFemale = summaryData.schoolFemale;
            this.schoolUnspecified = summaryData.schoolUnspecified;
            this.schoolInTransitionMale = summaryData.schoolInTransitionMale;
            this.schoolInTransitionFemale = summaryData.schoolInTransitionFemale;
            this.schoolInTransitionUnspecified = summaryData.schoolInTransitionUnspecified;
            this.movedOutScotlandMale = summaryData.movedOutScotlandMale;
            this.movedOutScotlandFemale = summaryData.movedOutScotlandFemale;
            this.movedOutScotlandUnspecified = summaryData.movedOutScotlandUnspecified;
            this.activityAgreementMale = summaryData.activityAgreementMale;
            this.activityAgreementFemale = summaryData.activityAgreementFemale;
            this.activityAgreementUnspecified = summaryData.activityAgreementUnspecified;
            this.fundStage2Male = summaryData.fundStage2Male;
            this.fundStage2Female = summaryData.fundStage2Female;
            this.fundStage2Unspecified = summaryData.fundStage2Unspecified;
            this.fundStage3Male = summaryData.fundStage3Male;
            this.fundStage3Female = summaryData.fundStage3Female;
            this.fundStage3Unspecified = summaryData.fundStage3Unspecified;
            this.fundStage4Male = summaryData.fundStage4Male;
            this.fundStage4Female = summaryData.fundStage4Female;
            this.fundStage4Unspecified = summaryData.fundStage4Unspecified;
            this.fullTimeEmploymentMale = summaryData.fullTimeEmploymentMale;
            this.fullTimeEmploymentFemale = summaryData.fullTimeEmploymentFemale;
            this.fullTimeEmploymentUnspecified = summaryData.fullTimeEmploymentUnspecified;
            this.furtherEducationMale = summaryData.furtherEducationMale;
            this.furtherEducationFemale = summaryData.furtherEducationFemale;
            this.furtherEducationUnspecified = summaryData.furtherEducationUnspecified;
            this.higherEducationMale = summaryData.higherEducationMale;
            this.higherEducationFemale = summaryData.higherEducationFemale;
            this.higherEducationUnspecified = summaryData.higherEducationUnspecified;
            this.modernApprenticeshipMale = summaryData.modernApprenticeshipMale;
            this.modernApprenticeshipFemale = summaryData.modernApprenticeshipFemale;
            this.modernApprenticeshipUnspecified = summaryData.modernApprenticeshipUnspecified;
            this.parttimeEmploymentMale = summaryData.parttimeEmploymentMale;
            this.parttimeEmploymentFemale = summaryData.parttimeEmploymentFemale;
            this.parttimeEmploymentUnspecified = summaryData.parttimeEmploymentUnspecified;
            this.personalDevelopmentMale = summaryData.personalDevelopmentMale;
            this.personalDevelopmentFemale = summaryData.personalDevelopmentFemale;
            this.personalDevelopmentUnspecified = summaryData.personalDevelopmentUnspecified;
            this.selfEmployedMale = summaryData.selfEmployedMale;
            this.selfEmployedFemale = summaryData.selfEmployedFemale;
            this.selfEmployedUnspecified = summaryData.selfEmployedUnspecified;
            this.trainingMale = summaryData.trainingMale;
            this.trainingFemale = summaryData.trainingFemale;
            this.trainingUnspecified = summaryData.trainingUnspecified;
            this.voluntaryWorkMale = summaryData.voluntaryWorkMale;
            this.voluntaryWorkFemale = summaryData.voluntaryWorkFemale;
            this.voluntaryWorkUnspecified = summaryData.voluntaryWorkUnspecified;
            this.AvgWeek = summaryData.AvgWeek;
            this.custodyMale = summaryData.custodyMale;
            this.custodyFemale = summaryData.custodyFemale;
            this.custodyUnspecified = summaryData.custodyUnspecified;
            this.economicallyInactiveMale = summaryData.economicallyInactiveMale;
            this.economicallyInactiveFemale = summaryData.economicallyInactiveFemale;
            this.economicallyInactiveUnspecified = summaryData.economicallyInactiveUnspecified;
            this.illHealthMale = summaryData.illHealthMale;
            this.illHealthFemale = summaryData.illHealthFemale;
            this.illHealthUnspecified = summaryData.illHealthUnspecified;
            this.unemployedMale = summaryData.unemployedMale;
            this.unemployedFemale = summaryData.unemployedFemale;
            this.unemployedUnspecified = summaryData.unemployedUnspecified;
            this.unknownMale = summaryData.unknownMale;
            this.unknownFemale = summaryData.unknownFemale;
            this.unknownUnspecified = summaryData.unknownUnspecified;
            this.type = summaryData.type;
            this.dataMonth = summaryData.dataMonth;
            this.dataYear = summaryData.dataYear;

            this.allPupils = summaryData.allFemale + summaryData.allMale + summaryData.allUnspecified;
            this.allPupils15 = summaryData.all15Female + summaryData.all15Male + summaryData.all15Unspecified;
            this.allPupils16 = summaryData.all16Female + summaryData.all16Male + summaryData.all16Unspecified;
            this.allPupils17 = summaryData.all17Female + summaryData.all17Male + summaryData.all17Unspecified;
            this.allPupils18 = summaryData.all19Female + summaryData.all19Male + summaryData.all19Unspecified;
            this.allPupils19 = summaryData.all19Female + summaryData.all19Male + summaryData.all19Unspecified;
            this.allPupilsInSchool = summaryData.schoolFemale + summaryData.schoolMale + summaryData.schoolUnspecified;
            this.allPupilsInSchoolTransition = summaryData.schoolInTransitionFemale + summaryData.schoolInTransitionMale + summaryData.schoolInTransitionUnspecified;
            this.allPupilsMovedOutwithScotland = summaryData.movedOutScotlandFemale + summaryData.movedOutScotlandMale + summaryData.movedOutScotlandUnspecified;
            this.allPupilsInActivityAgreement = summaryData.activityAgreementFemale + summaryData.activityAgreementMale + summaryData.activityAgreementUnspecified;
            this.allPupilsInEmployabilityFundStage2 = summaryData.fundStage2Female + summaryData.fundStage2Male + summaryData.fundStage2Unspecified;
            this.allPupilsInEmployabilityFundStage3 = summaryData.fundStage3Female + summaryData.fundStage3Male + summaryData.fundStage3Unspecified;
            this.allPupilsInEmployabilityFundStage4 = summaryData.fundStage4Female + summaryData.fundStage4Male + summaryData.fundStage4Unspecified;
            this.allPupilsInFullTimeEmployement = summaryData.fullTimeEmploymentFemale + summaryData.fullTimeEmploymentMale + summaryData.fullTimeEmploymentUnspecified;
            this.allPupilsInFurtherEducation = summaryData.furtherEducationFemale + summaryData.furtherEducationMale + summaryData.furtherEducationUnspecified;
            this.allPupilsInHigherEducation = summaryData.higherEducationFemale + summaryData.higherEducationMale + summaryData.higherEducationUnspecified;
            this.allPupilsInModernApprenship = summaryData.modernApprenticeshipFemale + summaryData.modernApprenticeshipMale + summaryData.modernApprenticeshipUnspecified;
            this.allPupilsInPartTimeEmployment = summaryData.parttimeEmploymentFemale + summaryData.parttimeEmploymentMale + summaryData.parttimeEmploymentUnspecified;
            this.allPupilsInPersonalSkillDevelopment = summaryData.personalDevelopmentFemale + summaryData.personalDevelopmentMale + summaryData.personalDevelopmentUnspecified;
            this.allPupilsInSelfEmployed = summaryData.selfEmployedFemale + summaryData.selfEmployedMale + summaryData.selfEmployedUnspecified;
            this.allPupilsInTraining = summaryData.trainingFemale + summaryData.trainingMale + summaryData.trainingUnspecified;
            this.allPupilsInVoulanteerWork = summaryData.voluntaryWorkFemale + summaryData.voluntaryWorkMale + summaryData.voluntaryWorkUnspecified;
            this.allPupilsInCustody = summaryData.custodyFemale + summaryData.custodyMale + summaryData.custodyUnspecified;
            this.allPupilsInEconomicallyInactive = summaryData.economicallyInactiveFemale + summaryData.economicallyInactiveMale + summaryData.economicallyInactiveUnspecified;
            this.allPupilsInUnavailableIllHealth = summaryData.illHealthFemale + summaryData.illHealthMale + summaryData.illHealthUnspecified;
            this.allPupilsInUnemployed = summaryData.unemployedFemale + summaryData.unemployedMale + summaryData.unemployedUnspecified;
            this.allPupilsInUnknown = summaryData.unknownFemale + summaryData.unknownMale + summaryData.unknownUnspecified;
        
            //this.getAllPupilsIncludingMovedOutwithScotland return getAllPupils() + getAllPupilsMovedOutwithScotland();
        }

        public string name { get; set; }
        public string dataCode { get; set; }
        public int allMale { get; set; }
        public int allFemale { get; set; }
        public int allUnspecified { get; set; }
        public int all15Male { get; set; }
        public int all15Female { get; set; }
        public int all15Unspecified { get; set; }
        public int all16Male { get; set; }
        public int all16Female { get; set; }
        public int all16Unspecified { get; set; }
        public int all17Male { get; set; }
        public int all17Female { get; set; }
        public int all17Unspecified { get; set; }
        public int all18Male { get; set; }
        public int all18Female { get; set; }
        public int all18Unspecified { get; set; }
        public int all19Male { get; set; }
        public int all19Female { get; set; }
        public int all19Unspecified { get; set; }
        public int schoolMale { get; set; }
        public int schoolFemale { get; set; }
        public int schoolUnspecified { get; set; }
        public int schoolInTransitionMale { get; set; }
        public int schoolInTransitionFemale { get; set; }
        public int schoolInTransitionUnspecified { get; set; }
        public int movedOutScotlandMale { get; set; }
        public int movedOutScotlandFemale { get; set; }
        public int movedOutScotlandUnspecified { get; set; }
        public int activityAgreementMale { get; set; }
        public int activityAgreementFemale { get; set; }
        public int activityAgreementUnspecified { get; set; }
        public int fundStage2Male { get; set; }
        public int fundStage2Female { get; set; }
        public int fundStage2Unspecified { get; set; }
        public int fundStage3Male { get; set; }
        public int fundStage3Female { get; set; }
        public int fundStage3Unspecified { get; set; }
        public int fundStage4Male { get; set; }
        public int fundStage4Female { get; set; }
        public int fundStage4Unspecified { get; set; }
        public int fullTimeEmploymentMale { get; set; }
        public int fullTimeEmploymentFemale { get; set; }
        public int fullTimeEmploymentUnspecified { get; set; }
        public int furtherEducationMale { get; set; }
        public int furtherEducationFemale { get; set; }
        public int furtherEducationUnspecified { get; set; }
        public int higherEducationMale { get; set; }
        public int higherEducationFemale { get; set; }
        public int higherEducationUnspecified { get; set; }
        public int modernApprenticeshipMale { get; set; }
        public int modernApprenticeshipFemale { get; set; }
        public int modernApprenticeshipUnspecified { get; set; }
        public int parttimeEmploymentMale { get; set; }
        public int parttimeEmploymentFemale { get; set; }
        public int parttimeEmploymentUnspecified { get; set; }
        public int personalDevelopmentMale { get; set; }
        public int personalDevelopmentFemale { get; set; }
        public int personalDevelopmentUnspecified { get; set; }
        public int selfEmployedMale { get; set; }
        public int selfEmployedFemale { get; set; }
        public int selfEmployedUnspecified { get; set; }
        public int trainingMale { get; set; }
        public int trainingFemale { get; set; }
        public int trainingUnspecified { get; set; }
        public int voluntaryWorkMale { get; set; }
        public int voluntaryWorkFemale { get; set; }
        public int voluntaryWorkUnspecified { get; set; }
        public double AvgWeek { get; set; }
        public int custodyMale { get; set; }
        public int custodyFemale { get; set; }
        public int custodyUnspecified { get; set; }
        public int economicallyInactiveMale { get; set; }
        public int economicallyInactiveFemale { get; set; }
        public int economicallyInactiveUnspecified { get; set; }
        public int illHealthMale { get; set; }
        public int illHealthFemale { get; set; }
        public int illHealthUnspecified { get; set; }
        public int unemployedMale { get; set; }
        public int unemployedFemale { get; set; }
        public int unemployedUnspecified { get; set; }
        public int unknownMale { get; set; }
        public int unknownFemale { get; set; }
        public int unknownUnspecified { get; set; }
        public string type { get; set; }
        public int dataMonth { get; set; }
        public int dataYear { get; set; }

        public int allPupils { get; set; }
        public int allPupils15 { get; set; }
        public int allPupils16 { get; set; }
        public int allPupils17 { get; set; }
        public int allPupils18 { get; set; }
        public int allPupils19 { get; set; }
        public int allPupilsInSchool { get; set; }
        public int allPupilsInSchoolTransition { get; set; }
        public int allPupilsMovedOutwithScotland { get; set; }
        public int allPupilsInActivityAgreement { get; set; }
        public int allPupilsInEmployabilityFundStage2 { get; set; }
        public int allPupilsInEmployabilityFundStage3 { get; set; }
        public int allPupilsInEmployabilityFundStage4 { get; set; }
        public int allPupilsInFullTimeEmployement { get; set; }
        public int allPupilsInFurtherEducation { get; set; }
        public int allPupilsInHigherEducation { get; set; }
        public int allPupilsInModernApprenship { get; set; }
        public int allPupilsInPartTimeEmployment { get; set; }
        public int allPupilsInPersonalSkillDevelopment { get; set; }
        public int allPupilsInSelfEmployed { get; set; }
        public int allPupilsInTraining { get; set; }
        public int allPupilsInVoulanteerWork { get; set; }
        public int allPupilsInCustody { get; set; }
        public int allPupilsInEconomicallyInactive { get; set; }
        public int allPupilsInUnavailableIllHealth { get; set; }
        public int allPupilsInUnemployed { get; set; }
        public int allPupilsInUnknown { get; set; }
        //public int getAllPupilsIncludingMovedOutwithScotland { get; set; }

        public decimal Percentage(int number)
        {
            // decimals are better suited for persentage
            if (this.allPupils == 0) return 0.0m;
            return (decimal)(number * 100m) / this.allPupils;
            //return result.HasValue ? (decimal)result : 0.0m;

        }

        public decimal Participating()
        {
            return (Percentage(
                this.allPupilsInSchool+
                this.allPupilsInSchoolTransition +
                this.allPupilsInActivityAgreement +
                this.allPupilsInEmployabilityFundStage2 +
                this.allPupilsInEmployabilityFundStage3 +
                this.allPupilsInEmployabilityFundStage4 +
                this.allPupilsInFullTimeEmployement +
                this.allPupilsInFurtherEducation +
                this.allPupilsInHigherEducation +
                this.allPupilsInModernApprenship +
                this.allPupilsInPartTimeEmployment +
                this.allPupilsInPersonalSkillDevelopment +
                this.allPupilsInSelfEmployed +
                this.allPupilsInTraining +
                this.allPupilsInVoulanteerWork
            ));
        }

        public decimal NotParticipating()
        {
            return (Percentage(
                this.allPupilsInCustody +
                this.allPupilsInEconomicallyInactive +
                this.allPupilsInUnavailableIllHealth +
                this.allPupilsInUnemployed
            ));
        }

        public object FormatNumber(int number)
        {
            if (number <= 10)
            {
                return "*";
            }
            else
            {
                return number;
            }

        }
    }
}