using ACCDataStore.Entity.RenderObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class SecondaryChartData: ChartData
    {
        public BaseRenderObject ChartMiDYIS { get; set; }
        public BaseRenderObject ChartTimelineCfE { get; set; }
        public BaseRenderObject ChartSOSCAMaths { get; set; }
        public BaseRenderObject ChartSOSCAReading { get; set; }
        public BaseRenderObject ChartSOSCAScience { get; set; }
        public BaseRenderObject ChartCfe3LevelbyQuintile { get; set; }
        public BaseRenderObject ChartCfe4LevelbyQuintile { get; set; }
        public BaseRenderObject ChartCfe3Level { get; set; }
        public BaseRenderObject ChartCfe4Level { get; set; }
    }
}
