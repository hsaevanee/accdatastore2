using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.ViewModels.Datahub
{
    public class DatahubData  
    {
        public string datacode { get; set; } // store school code / datazone code
        public string name { get; set; } // store school code / datazone code
        public int allpupils { get; set; }
        public int allFemalepupils { get; set; }
        public int allMalepupils { get; set; }
        public int all15pupils { get; set; }
        public int all16pupils { get; set; }
        public int all17pupils { get; set; }
        public int all18pupils { get; set; }
        public int all19pupils { get; set; }
        public int schoolpupils { get; set; }
        public int schoolpupilsintransition { get; set; }
        public int schoolpupilsmovedoutinscotland { get; set; }
        public int pupilsinAtivityAgreement { get; set; }
        public int pupilsinEmployFundSt2 { get; set; }
        public int pupilsinEmployFundSt3 { get; set; }
        public int pupilsinEmployFundSt4 { get; set; }
        public int pupilsinFullTimeEmployed { get; set; }
        public int pupilsinFurtherEdu { get; set; }
        public int pupilsinHigherEdu{ get; set; }
        public int pupilsinModernApprenship{ get; set; }
        public int pupilsinPartTimeEmployed { get; set; }
        public int pupilsinPersonalSkillDevelopment { get; set; }
        public int pupilsinSelfEmployed { get; set; }
        public int pupilsinTraining { get; set; }
        public int pupilsinVolunteerWork{ get; set; }
        public double AvgWeekssinceLastPositiveDestination { get; set; }
        public int pupilsinCustody { get; set; }
        public int pupilsinEconomically { get; set; }
        public int pupilsinUnavailableillHealth { get; set; }
        public int pupilsinUnemployed{ get; set; }
        public int pupilsinUnknown { get; set; }
        public int allpupilsexcludemovedoutscotland { get; set; }

        public double Percentage(int number)
        {
            double result = (double)(number * 100) / allpupils;
            return double.IsNaN(result) ? 0.0 : result;
             
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

        //public double Participating()
        //{
        //    return (double)(Percentage(this.schoolpupils) + 
        //        Percentage(this.schoolpupilsintransition) +
        //        Percentage(this.pupilsinAtivityAgreement) +
        //        Percentage(this.pupilsinEmployFundSt2) +
        //        Percentage(this.pupilsinEmployFundSt3)+
        //        Percentage(this.pupilsinEmployFundSt4)+
        //        Percentage(this.pupilsinFullTimeEmployed)+
        //        Percentage(this.pupilsinFurtherEdu)+
        //        Percentage(this.pupilsinHigherEdu) +
        //        Percentage(this.pupilsinModernApprenship) +
        //        Percentage(this.pupilsinPartTimeEmployed) +
        //        Percentage(this.pupilsinPersonalSkillDevelopment)+
        //        Percentage(this.pupilsinSelfEmployed)+
        //        Percentage(this.pupilsinTraining)+
        //        Percentage(this.pupilsinVolunteerWork));
        //}

        public double Participating()
        {
            return (double)(Percentage(this.schoolpupils +
               this.schoolpupilsintransition+
                this.pupilsinAtivityAgreement+
                this.pupilsinEmployFundSt2 +
                this.pupilsinEmployFundSt3 +
                this.pupilsinEmployFundSt4 +
                this.pupilsinFullTimeEmployed +
                this.pupilsinFurtherEdu +
                this.pupilsinHigherEdu +
                this.pupilsinModernApprenship +
                this.pupilsinPartTimeEmployed +
                this.pupilsinPersonalSkillDevelopment +
                this.pupilsinSelfEmployed +
                this.pupilsinTraining +
                this.pupilsinVolunteerWork));
        }

        public double NotParticipating()
        {
            return (double)(Percentage(this.pupilsinCustody) + Percentage(this.pupilsinEconomically) + Percentage(this.pupilsinUnavailableillHealth) + Percentage(this.pupilsinUnemployed));
        }

    }
}