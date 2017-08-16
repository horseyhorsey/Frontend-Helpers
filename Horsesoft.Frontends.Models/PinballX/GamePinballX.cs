﻿using System.Xml.Serialization;

namespace Frontends.Models.PinballX
{
    [XmlType("game")]
    public class GamePinballX : GamePinball
    {
        [XmlElement("enabled")]
        public bool Enabled { get; set; }

        [XmlElement("rom")]
        public string RomName { get; set; }

        [XmlElement("author")]
        public string Author { get; set; }

        [XmlElement("rating")]
        public int Rating { get; set; }

        [XmlElement("hidedmd")]
        public bool HideDmd { get; set; }

        [XmlElement("hidebackglass")]
        public bool HideBackGlass { get; set; }

        [XmlElement("alternateExe")]
        public string AlternateExe { get; set; }

        [XmlElement("desktop")]
        public bool Desktop { get; set; }

        [XmlIgnore]
        public bool TableFileExists { get; set; }

    }
}
