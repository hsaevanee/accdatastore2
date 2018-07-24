using ACCDataStore.Entity.RenderObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile.Entities
{
    public class ChartData
    {
        public BaseRenderObject ChartPupilsAges { get; set; }
        public BaseRenderObject ChartPositiveDestinations { get; set; }
        public BaseRenderObject ChartNonPositiveDestinations { get; set; }
        public BaseRenderObject ChartOverallDestinations { get; set; }
        public BaseRenderObject ChartTimelineDestinations { get; set; }
        public BaseRenderObject ChartPositiveCohortbyCentre { get; set; }
        public BaseRenderObject ChartNonPositiveCohortbyCentre { get; set; }
        public BaseRenderObject ChartUnknownCohortbyCentre { get; set; }
        public BaseRenderObject ChartGroupDestinations { get; set; }
    }
}
