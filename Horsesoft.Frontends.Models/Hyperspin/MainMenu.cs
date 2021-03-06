﻿using System;
using System.Xml.Serialization;

namespace Frontends.Models.Hyperspin
{
    [Serializable]
    [XmlType(TypeName = "game")]
    public class MainMenu
    {
        #region Properties
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("enabled")]
        public int Enabled { get; set; }

        [XmlIgnore]
        public Uri SysIcon { get; set; }

        [XmlIgnore]
        public bool XmlExists { get; set; }

        [XmlIgnore]
        public bool GenreExists { get; set; }

        #endregion

        #region Constructors
        public MainMenu()
        {

        }

        public MainMenu(string _name)
        {
            Name = _name;

        }

        public MainMenu(string _name, int _enabled = 1)
        {
            Name = _name;
            Enabled = _enabled;
        }

        public MainMenu(string _name, Uri pathToIcon, int _enabled = 1)
        {
            Name = _name;
            Enabled = _enabled;
            if (pathToIcon != null)
                SysIcon = pathToIcon;
        }

        #endregion
    }
}
