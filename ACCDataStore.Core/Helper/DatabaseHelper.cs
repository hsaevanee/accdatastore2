using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ACCDataStore.Core.Helper
{
    public class DatabaseHelper
    {
        public int Type { get; set; } // 0 - MySQL, 1 - SQL Server, 2 - Oracle, 3 - MS Access
        public string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConnectionString
        {
            get
            {
                switch (this.Type)
                {
                    case 0:
                        return "server=" + this.Host + ";user id=" + this.Username + ";password=" + this.Password + ";persist security info=True;database=" + this.Name;
                    case 1:
                        return "Data Source=" + this.Host + ";User ID=" + this.Username + ";Password=" + this.Password + ";Initial Catalog=" + this.Name;
                    case 2:
                        return "server=" + this.Host + ";user id=" + this.Username + ";password=" + this.Password + ";persist security info=True;database=" + this.Name;
                    case 3:
                        return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.Name + ";Persist Security Info=False;Jet OLEDB:Database Password=";
                    default:
                        return "server=" + this.Host + ";user id=" + this.Username + ";password=" + this.Password + ";persist security info=True;database=" + this.Name;
                }
            }
        }
    }
}
