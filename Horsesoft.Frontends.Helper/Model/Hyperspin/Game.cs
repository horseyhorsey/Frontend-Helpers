using Horsesoft.Frontends.Helper.Model.RocketLauncher;
using System;
using System.Xml.Serialization;

namespace Horsesoft.Frontends.Helper.Model.Hyperspin
{
    [XmlType(TypeName = "game")]
    public class Game : IComparable<Game>
    {
        #region Constructors

        public Game()
        {
            MenuAudit = new AuditGame();
            RlAudit = new RlAudit();
        }

        /// <summary>
        /// standard romname & desc contructor
        /// </summary>
        /// <param name="Gamename"></param>
        /// <param name="Description"></param>
        public Game(string Gamename, string Description) : this()
        {
            this.RomName = Gamename;
            this.Description = Description;            
        }

        public Game(string name, string index, string image, string desc, string cloneof, string crc, string manu, int year, string genre, string rating, int enabled) : this()
        {
            RomName = name;
            Index = index;
            Image = image;
            Description = desc;
            CloneOf = cloneof;
            Crc = crc;
            Manufacturer = manu;
            Year = year;
            Genre = genre;
            Rating = rating;
            GameEnabled = enabled;            
        }

        public Game(string name, string index, string image, string desc, string cloneof,
            string crc, string manu, int year, string genre, string rating, int enabled, string system) : this()
        {
            RomName = name;
            Index = index;
            Image = image;
            Description = desc;
            CloneOf = cloneof;
            Crc = crc;
            Manufacturer = manu;
            Year = year;
            Genre = genre;
            Rating = rating;
            GameEnabled = enabled;
            System = system;            
        }        

        #endregion

        #region Seriliazable From Hyperspin Properties
        [XmlAttribute("name")]
        public string RomName { get; set; }

        [XmlAttribute("enabled")]
        public int GameEnabled { get; set; }

        [XmlAttribute("index")]
        public string Index { get; set; }

        [XmlAttribute("image")]
        public string Image { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("cloneof")]
        public string CloneOf { get; set; }

        [XmlElement("crc")]
        public string Crc { get; set; }

        [XmlElement("manufacturer")]
        public string Manufacturer { get; set; }

        [XmlElement("genre")]
        public string Genre { get; set; }

        [XmlElement("year")]
        public int Year { get; set; }

        [XmlElement("rating")]
        public string Rating { get; set; }

        #endregion

        #region Extra Properties

        [XmlIgnore]
        public AuditGame GameAudit { get; set; }

        [XmlIgnore]
        public AuditGame MenuAudit { get; set; }

        [XmlIgnore]
        public RlAudit RlAudit { get; set; }        

        [XmlIgnore]
        public bool RomExists { get; set; }

        private bool isFavorite;
        [XmlIgnore]
        public bool IsFavorite
        {
            get { return isFavorite; }
            set { isFavorite = value; }
        }

        [XmlIgnore]
        public string Name
        {
            get { return RomName; }
            set { RomName = value; }
        }

        [XmlIgnore]
        public string System { get; set; }
        #endregion

        #region Public Methods
        public int CompareTo(Game otherGame)
        {
            if (otherGame == null)
                throw new ArgumentException("Game is not a game");

            return this.Description.CompareTo(otherGame.Description);
        }
        #endregion

    }

}
