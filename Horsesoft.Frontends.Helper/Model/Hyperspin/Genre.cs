using System.Xml.Serialization;

namespace Horsesoft.Frontends.Helper.Model.Hyperspin
{
    [XmlType(TypeName = "game")]
    public class Genre
    {        
        [XmlAttribute("name")]
        public string GenreName { get; set; }
    }
}
