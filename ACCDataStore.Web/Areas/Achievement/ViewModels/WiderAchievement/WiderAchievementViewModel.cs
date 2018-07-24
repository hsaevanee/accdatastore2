using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.Achievement.ViewModels.WiderAchievement
{
    public class WiderAchievementViewModel : SchoolProfileViewModel
    {
        public DataTable dtTable1 { get; set; }
        public DataTable dtTable2 { get; set; }
        public List<School> Listschoolname { get; set; }
        public List<string> Listawardname { get; set; }
        public string selectedawardname { get; set; }
        public List<WiderAchievementObj> Listresults { get; set; }
        public List<string> Listscqf_rating { get; set; }
        public string selectescqf_rating { get; set; }
    }
}