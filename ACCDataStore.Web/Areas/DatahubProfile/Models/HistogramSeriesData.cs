using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.Models
{
    public class HistogramSeriesData
    {

        public HistogramSeriesData()
        {
            this.months = new List<string>();
            this.participating = new List<double>();
            this.notParticipating = new List<double>();
            this.unknown = new List<double>();
            this.name = "";
        }

        public HistogramSeriesData(List<string> months, List<double> participating, List<double> notParticipating, List<double> unknown, string name)
        {
            this.months = months;
            this.participating = participating;
            this.notParticipating = notParticipating;
            this.unknown = unknown;
            this.name = name;
        }

        public List<string> months { get; set; }
        public List<double> participating { get; set; }
        public List<double> notParticipating { get; set; }
        public List<double> unknown { get; set; }
        public string name { get; set; }
    }
}