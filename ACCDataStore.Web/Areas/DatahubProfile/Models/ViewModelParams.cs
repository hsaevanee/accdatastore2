using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.Models
{
    public class ViewModelParams
    {
        public ViewModelParams()
        {
            this.neighbourhood = new List<string>();
            this.school = new List<string>();
            this.councilName = "";
        }

        public ViewModelParams(List<string> schools, List<string> neighbourhoods, string council)
        {
            this.councilName = council;
            this.neighbourhood = neighbourhoods;
            this.school = schools;
        } 

        public List<string> school { get; set; }
        public List<string> neighbourhood { get; set; }
        public string councilName { get; set; }
    }
}