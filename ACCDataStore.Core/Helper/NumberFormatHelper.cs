using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Core.Helper
{
    public class NumberFormatHelper
    {
        public static object FormatNumber(object oData, int nDecimalPointNumber)
        {
            var nData = 0m;
            if (decimal.TryParse(Convert.ToString(oData), out nData))
            {
                return decimal.Round(Convert.ToDecimal(nData), nDecimalPointNumber, MidpointRounding.AwayFromZero).ToString("#,0.".PadRight(nDecimalPointNumber + 4, '0'));
            }
            else
            {
                return oData;
            }
        }

        public static object FormatNumber(object oData, int nDecimalPointNumber, string sNullValue)
        {
            if (oData != null)
            {
                var nData = 0m;
                if (decimal.TryParse(Convert.ToString(oData), out nData))
                {
                    return decimal.Round(Convert.ToDecimal(nData), nDecimalPointNumber, MidpointRounding.AwayFromZero).ToString("#,0.".PadRight(nDecimalPointNumber + 4, '0'));
                }
                else
                {
                    return oData;
                }
            }
            else
            {
                return sNullValue;
            }
        }

        public static float? ConvertObjectToFloat(object oValue)
        {
            var nValue = 0f;

            if (oValue!=null && (float.TryParse(oValue.ToString(), out nValue)))
            {
                return nValue;
            }
            else
            {
                return null;
            }
        }

        public static object FormatNumber(object oData, int nCriteriaNumber, string sNullValue, string replace)
        {
            var star = "*";

            if (oData != null)
            {
                var nData = 0m;
                if (decimal.TryParse(Convert.ToString(oData), out nData) && nData <= nCriteriaNumber)
                {
                        return star;
                }
                else
                {
                    return oData;
                }
            }
            else
            {
                return sNullValue;
            }

        }

        //public static string FormatNumberToString(float nData, int nDecimalPointNumber)
        //{
        //    return Math.Round(Convert.ToDecimal(nData), nDecimalPointNumber, MidpointRounding.AwayFromZero).ToString("#,0.".PadRight(nDecimalPointNumber + 4, '0'));
        //}

        //public static string FormatNumberToString(float? nData, int nDecimalPointNumber, string sNullValue)
        //{
        //    if (nData.HasValue)
        //    {
        //        return Math.Round(Convert.ToDecimal(nData), nDecimalPointNumber, MidpointRounding.AwayFromZero).ToString("#,0.".PadRight(nDecimalPointNumber + 4, '0'));
        //    }
        //    else
        //    {
        //        return sNullValue;
        //    }
        //}

        //public static float FormatNumber(float nData, int nDecimalPointNumber)
        //{
        //    return float.Parse(Math.Round(Convert.ToDecimal(nData), nDecimalPointNumber, MidpointRounding.AwayFromZero).ToString("#,0.".PadRight(nDecimalPointNumber + 4, '0')));
        //}
    }
}
