using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LocalizationHelper
{
    public static class Mapper
    {
        public static root MapToroot(this IEnumerable<string> localizerNames)
        {
            return new root
            {
                data = localizerNames.Select(i => new rootData
                {
                    name = i,
                    space = "preserve",
                    value = i,
                    Text = null,
                    Generated = i
                }).ToList(),
                resheader = new[]
                {
                    new rootResheader
                    {
                        name = "resmimetype",
                        value = "text/microsoft-resx"
                    },
                    new rootResheader
                    {
                        name = "version",
                        value = "1.3"
                    },
                    new rootResheader
                    {
                        name = "reader",
                        value = "System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.3500.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
                    },
                    new rootResheader
                    {
                        name = "writer",
                        value = "System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.3500.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
                    }
                }
            };
        }
        
        public static T DeSeriXML<T>(this string filePath)
        {
            XmlSerializer seri = new XmlSerializer(typeof(T));
            T ret = default(T);
            using (Stream s = System.IO.File.Open(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                ret = (T)seri.Deserialize(s);
            }
            return ret;
        }
    }
}
