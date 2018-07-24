using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ACCDataStore.Core.Helper
{
    public class ConvertHelper
    {
        public static string ConvertListString2String(string[] listString, string sDelim)
        {
            var sbResult = new StringBuilder();
            foreach (var item in listString)
            {
                if (sbResult.ToString().Length > 0)
                {
                    sbResult.Append(sDelim);
                }
                sbResult.Append("'" + item + "'");
            }
            return sbResult.ToString();
        }

        public static string Object2Xml(object obj)
        {
            string sXml = "";
            var x = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            var ns = new System.Xml.Serialization.XmlSerializerNamespaces();
            ns.Add("", "");
            using (var ms = new System.IO.MemoryStream())
            {
                var settings = new System.Xml.XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.Encoding = System.Text.Encoding.UTF8;
                var writer = System.Xml.XmlWriter.Create(ms, settings);
                x.Serialize(writer, obj, ns);
                sXml = System.Text.Encoding.UTF8.GetString(ms.GetBuffer());
            }
            return sXml;
        }

        public static void Object2XmlFile(object obj, string sFileName)
        {
            var x = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            var ns = new System.Xml.Serialization.XmlSerializerNamespaces();
            ns.Add("", "");
            using (var fs = new System.IO.FileStream(sFileName, System.IO.FileMode.Create))
            {
                var settings = new System.Xml.XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                settings.Encoding = System.Text.Encoding.UTF8;
                var writer = System.Xml.XmlWriter.Create(fs, settings);
                x.Serialize(writer, obj, ns);
            }
        }

        public static object Xml2Object(string sXml, Type objType)
        {
            var x = new System.Xml.Serialization.XmlSerializer(objType);
            object objResult = (object)x.Deserialize(new System.IO.StringReader(sXml));
            return objResult;
        }

        public static object XmlFile2Object(string sFileName, Type objType)
        {
            object objResult = null;
            var x = new System.Xml.Serialization.XmlSerializer(objType);
            using (var fs = new System.IO.FileStream(sFileName, System.IO.FileMode.Open))
            {
                objResult = (object)x.Deserialize(fs);
            }
            return objResult;
        }

        public static object XmlFile2Object(string sFileName, Type objType, Encoding encode)
        {
            object objResult = null;
            var x = new System.Xml.Serialization.XmlSerializer(objType);
            using (var fs = new System.IO.FileStream(sFileName, System.IO.FileMode.Open))
            {
                using (var xmlTextWriter = new XmlTextWriter(fs, encode))
                {
                    objResult = (object)x.Deserialize(fs);
                }
            }
            return objResult;
        }

        public static string Unicode2MS874(string sText) 
        {
            if (sText != null)
            {
                byte[] arrbText = Encoding.Unicode.GetBytes(sText);
                var sResult = Encoding.GetEncoding("windows-874").GetString(arrbText);
                return sResult.Replace("\0", "");
            }
            return sText;
        }

        public static int String2Int(string sText)
        {
            var nResult = 0;
            int.TryParse(sText, out nResult);
            return nResult;
        }

        public static int String2Int(string sText, int nDefault)
        {
            var nResult = nDefault;
            if (int.TryParse(sText, out nResult))
            {
                return nResult;
            }
            else
            {
                return nDefault;
            }
        }

        public static long String2Long(string sText)
        {
            var nResult = 0L;
            long.TryParse(sText, out nResult);
            return nResult;
        }

        public static long String2Long(string sText, long nDefault)
        {
            var nResult = nDefault;
            if (long.TryParse(sText, out nResult))
            {
                return nResult;
            }
            else
            {
                return nDefault;
            }
        }

        public static double String2Double(string sText)
        {
            var nResult = 0d;
            double.TryParse(sText, out nResult);
            return nResult;
        }

        public static double String2Double(string sText, double nDefault)
        {
            var nResult = nDefault;
            if (double.TryParse(sText, out nResult))
            {
                return nResult;
            }
            else
            {
                return nDefault;
            }
        }

        public static DateTime String2DateTime(string sText, string sFormat)
        {
            var nResult = DateTime.MinValue;
            DateTime.TryParseExact(sText, sFormat, null, System.Globalization.DateTimeStyles.None, out nResult);
            return nResult;
        }

        public static DateTime String2DateTime(string sText, string sFormat, DateTime dtDefault)
        {
            var dtResult = dtDefault;
            if (DateTime.TryParseExact(sText, sFormat, null, System.Globalization.DateTimeStyles.None, out dtResult))
            {
                return dtResult;
            }
            else
            {
                return dtDefault;
            }
        }

        public static object CloneObject(object o)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();

            Object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance,
                null, o, null);

            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    pi.SetValue(p, pi.GetValue(o, null), null);
                }
            }

            return p;
        }
    }
}
