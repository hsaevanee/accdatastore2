using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class Year: BaseEntity
    {
        private string _year;
        public string year {
            get {
                return this._year;
            }
            set {
                this._year = value;
                switch (value)
                {
                    case "2008":
                        this.academicyear = "2008/09";
                        break;
                    case "2009":
                        this.academicyear = "2009/10";
                        break;
                    case "2010":
                        this.academicyear = "2010/11";
                        break;
                    case "2011":
                        this.academicyear = "2011/12";
                        break;
                    case "2012":
                        this.academicyear = "2012/13";
                        break;
                    case "2013":
                        this.academicyear = "2013/14";
                        break;
                    case "2014":
                        this.academicyear = "2014/15";
                        break;
                    case "2015":
                        this.academicyear = "2015/16";
                        break;
                    case "2016":
                        this.academicyear = "2016/17";
                        break;
                    case "2017":
                        this.academicyear = "2017/18";
                        break;
                    case "2018":
                        this.academicyear = "2018/19";
                        break;
                    case "2019":
                        this.academicyear = "2019/20";
                        break;
                    case "2020":
                        this.academicyear = "2020/21";
                        break;
                    case "2021":
                        this.academicyear = "2021/22";
                        break;
                    case "2022":
                        this.academicyear = "2022/23";
                        break;
                    case "2023":
                        this.academicyear = "2023/24";
                        break;
                }
            }
        }
        public string academicyear { get; set; }
        //public string isSelected { get; set; }

        public Year(string year)
        {
            this.year = year;
        }

        public Year()
        {
        }

        public object GetJson()
        {
            return new
            {
                Year = this.year,
                Academicyear = this.academicyear
            };
        }
    }
}
