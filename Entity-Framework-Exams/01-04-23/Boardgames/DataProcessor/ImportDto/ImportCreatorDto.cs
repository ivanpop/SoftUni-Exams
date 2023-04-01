using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class ImportCreatorDto
    {
        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlArray("Boardgames")]
        [XmlArrayItem("Boardgame")]
        public ImportBoardgameDto[] Boardgames { get; set; }
    }

    public class ImportBoardgameDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Rating")]
        public double Rating { get; set; }

        [XmlElement("YearPublished")]
        public int YearPublished { get; set; }

        [XmlElement("CategoryType")]
        public int CategoryType { get; set; }

        [XmlElement("Mechanics")]
        public string Mechanics { get; set; }
    }
}