using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile.Entities
{
    public class DataHubData
    {
        public string SeedCode { get; set; }
        public string SchoolName { get; set; }
        public string Centretype { get; set; }
        public GenericData AllClients { get; set; }
        public GenericData Males { get; set; }
        public GenericData Females { get; set; }
        public GenericData Pupil16 { get; set; }
        public GenericData Pupil17 { get; set; }
        public GenericData Pupil18 { get; set; }
        public GenericData Pupil19 { get; set; }
        public List<GenericData> ListPupilsbyAges { get; set; }
        public List<GenericData> PositiveDestinations { get; set; }
        public List<GenericData> NonPositiveDestinations { get; set; }
        public SDSGroup listPEducationGroup { get; set; }
        public SDSGroup listPEmploymentGroup { get; set; }
        public SDSGroup listPTrainingGroup { get; set; }
        public SDSGroup listNPUnemployedSeekingGroup { get; set; }
        public SDSGroup listNPUnemployedNotSeekingGroup { get; set; }
        public GenericData Unknown { get; set; }
        public GenericData MovedOutwithScotland { get; set; }
        public List<SummaryDHdata> listsumarydestination { get; set; }
        public int Participating_count { get; set; }
        public int NotParticipating_count { get; set; }
        public double Participating_Percent { get; set; }
        public double NotParticipating_Percent { get; set; }

    }
}
