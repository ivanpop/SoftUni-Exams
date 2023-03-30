using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class ImportDespatcherDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Position")]
        public string Position { get; set; }

        [XmlArray("Trucks")]
        [XmlArrayItem("Truck")]
        public ImportTruckDto[] Trucks { get; set; }        
    }

    [XmlType("Truck")]
    public class ImportTruckDto
    {
        [XmlElement("RegistrationNumber")]
        public string RegistrationNumber { get; set; }

        [XmlElement("VinNumber")]
        public string VinNumber { get; set; }

        [XmlElement("TankCapacity")]
        public int TankCapacity { get; set; }

        [XmlElement("CargoCapacity")]
        public int CargoCapacity { get; set; }

        [XmlElement("CategoryType")]
        public int CategoryType { get; set; }

        [XmlElement("MakeType")]
        public int MakeType { get; set; }
    }
}