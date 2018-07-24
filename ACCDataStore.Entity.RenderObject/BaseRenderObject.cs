using ACCDataStore.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.RenderObject
{
    public abstract class BaseRenderObject
    {
        protected void CreateConfigFile(string sConfigFileName)
        {
            ConvertHelper.Object2XmlFile(this, ApplicationHelper.GetApplicationPath() + @"\Config\" + sConfigFileName);
        }
    }
}
