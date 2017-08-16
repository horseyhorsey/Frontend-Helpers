using System.Xml.Serialization;

namespace Horsesoft.Frontends.Models.Hyperspin
{
    [XmlType(TypeName = "game")]
    public class Genre
    {        
        [XmlAttribute("name")]
        public string GenreName { get; set; }
    }
}
