using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Client")]
    public class ImportClientDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("NumberVat")]
        public string NumberVat { get; set; }

        [XmlArray("Addresses")]
        [XmlArrayItem("Address")]
        public ImportAddressDto[] Addresses { get; set; }
    }

    [XmlType("Address")]
    public class ImportAddressDto
    {
        [XmlElement("StreetName")]
        public string StreetName { get; set; }

        [XmlElement("StreetNumber")]
        public int StreetNumber { get; set; }

        [XmlElement("PostCode")]
        public string PostCode { get; set; }

        [XmlElement("City")]
        public string City { get; set; }

        [XmlElement("Country")]
        public string Country { get; set; }
    }
}