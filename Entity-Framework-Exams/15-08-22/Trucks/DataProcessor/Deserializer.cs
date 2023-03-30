namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;
    using Castle.Core.Internal;
    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            var despatchersDtos = Deserialize<ImportDespatcherDto[]>(xmlString, "Despatchers");

            StringBuilder sb = new StringBuilder();

            List<Despatcher> despatchers = new List<Despatcher>();

            foreach (var despatcherDto in despatchersDtos)
            {
                Despatcher despatcher1 = new Despatcher();

                if (despatcherDto.Name.IsNullOrEmpty() || despatcherDto.Name.Length < 2 || despatcherDto.Name.Length > 40 || despatcherDto.Position.IsNullOrEmpty())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                despatcher1.Name = despatcherDto.Name;
                despatcher1.Position = despatcherDto.Position;

                Regex regex = new Regex(@"[A-Z]{2}\d{4}[A-Z]{2}");

                foreach (var truck in despatcherDto.Trucks.Distinct())
                {
                    Match match = regex.Match(truck.RegistrationNumber);

                    if (truck.RegistrationNumber.IsNullOrEmpty() || truck.RegistrationNumber.Length != 8 || !match.Success)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck truck1 = new Truck();
                    truck1.RegistrationNumber = truck.RegistrationNumber;

                    if (truck.VinNumber.Length != 17 )
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    truck1.VinNumber = truck.VinNumber;

                    if (truck.TankCapacity < 950 || truck.TankCapacity > 1420)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    truck1.TankCapacity = truck.TankCapacity;

                    if (truck.CargoCapacity < 5000 || truck.CargoCapacity > 29000)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    truck1.CargoCapacity = truck.CargoCapacity;

                    if (truck.CategoryType < 0 || truck.CategoryType > 3)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    truck.CategoryType = truck.CategoryType;

                    if (truck.MakeType < 0 || truck.MakeType > 4)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    truck.MakeType = truck.MakeType;

                    despatcher1.Trucks.Add(truck1);                    
                }

                sb.AppendLine(String.Format(SuccessfullyImportedDespatcher, despatcher1.Name, despatcher1.Trucks.Count));
                despatchers.Add(despatcher1);
            }

            context.AddRange(despatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportClient(TrucksContext context, string jsonString)
        {
            ImportClientDto[] clientDtos = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();

            foreach (var clientDto in clientDtos)
            {
                Client client1 = new Client();

                if (clientDto.Name.Length < 3 || clientDto.Name.Length > 40)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                client1.Name = clientDto.Name;

                if (clientDto.Nationality.Length < 2 || clientDto.Nationality.Length > 40)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                client1.Nationality = clientDto.Nationality;

                if (clientDto.Type.IsNullOrEmpty() || clientDto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                client1.Type = clientDto.Type;

                foreach (int clientTruckDto in clientDto.Trucks.Distinct())
                {
                    Truck truck1 = context.Trucks.FirstOrDefault(x => x.Id == clientTruckDto);

                    if (truck1 == default)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck clientTruck1 = new ClientTruck();
                    clientTruck1.Truck = truck1;
                    clientTruck1.Client = client1;

                    client1.ClientsTrucks.Add(clientTruck1);
                }

                sb.AppendLine(string.Format(SuccessfullyImportedClient, client1.Name, client1.ClientsTrucks.Count()));

                context.Clients.Add(client1);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
        private static T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute inputXML = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), inputXML);

            using StringReader reader = new StringReader(inputXml);

            T dtos = (T)serializer.Deserialize(reader);
            return dtos;
        }
    }
}