using ACCDataStore.Entity.RenderObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class LocalityChartData: ChartData
    {
        public BaseRenderObject ChartTimelineCfE { get; set; }
        public BaseRenderObject ChartCfeP1Level { get; set; }
        public BaseRenderObject ChartCfeP4Level { get; set; }
        public BaseRenderObject ChartCfeP7Level { get; set; }
        public BaseRenderObject ChartCfeP1LevelbyQuintile { get; set; }
        public BaseRenderObject ChartCfeP4LevelbyQuintile { get; set; }
        public BaseRenderObject ChartCfeP7LevelbyQuintile { get; set; }
        public BaseRenderObject ChartCfe3LevelbyQuintile { get; set; }
        public BaseRenderObject ChartCfe4LevelbyQuintile { get; set; }
        public BaseRenderObject ChartCfe3Level { get; set; }
        public BaseRenderObject ChartCfe4Level { get; set; }
    }
}
