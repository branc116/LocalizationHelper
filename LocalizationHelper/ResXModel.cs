using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace LocalizationHelper
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class root
    {

        /// <remarks/>
        [XmlElement("resheader")]
        public rootResheader[] resheader { get; set; }

        /// <remarks/>
        [XmlElement("data")]
        public List<rootData> data { get; set; }
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class rootResheader
    {

        /// <remarks/>
        public string value { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name { get; set; }
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class rootData
    {

        /// <remarks/>
        public string value { get; set; }

        /// <remarks/>
        [XmlText()]
        public string[] Text { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string name { get; set; }

        /// <remarks/>
        [XmlAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string space { get; set; }
        [XmlAttribute()]
        public string Generated { get; set; }
    }


}