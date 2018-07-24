using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Web.Helpers
{
    public static class ConvertHelper
    {
        public static DataTable AsDataTable<T>(this IEnumerable<T> list)
    where T : class
        {
            DataTable dtOutput = new DataTable("dataTableOutput");

            if (list.Count() == 0)
                return dtOutput;

            PropertyInfo[] properties = list.FirstOrDefault().GetType().
                GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in properties)
                dtOutput.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);

            DataRow dr;
            foreach (T t in list)
            {
                dr = dtOutput.NewRow();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    dr[propertyInfo.Name] = propertyInfo.GetValue(t, null);
                }
                dtOutput.Rows.Add(dr);
            }
            return dtOutput;
        }

        public static void SetColumnsOrder(this DataTable table, params String[] columnNames)
        {
            for (int columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
            {
                table.Columns[columnNames[columnIndex]].SetOrdinal(columnIndex);
            }
        }
    }
}