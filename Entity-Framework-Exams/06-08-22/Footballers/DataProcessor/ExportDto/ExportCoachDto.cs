using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class ExportCoachDto
    {
        [XmlElement("CoachName")]
        public string CoachName { get; set; }

        [XmlAttribute("FootballersCount")]
        public int FootballersCount { get; set; }

        public ExportFootballerDto[] Footballers { get; set; }
    }

    [XmlType("Footballer")]
    public class ExportFootballerDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Position")]
        public string Position { get; set; }
    }
}