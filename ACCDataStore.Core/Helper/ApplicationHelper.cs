using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACCDataStore.Core.Helper
{
    public class ApplicationHelper
    {
        public static string GetApplicationPath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "");
        }
    }
}
