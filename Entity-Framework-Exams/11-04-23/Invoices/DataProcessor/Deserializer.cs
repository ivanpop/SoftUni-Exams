namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Castle.Core.Internal;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            var clientsDtos = Deserializerr<ImportClientDto[]>(xmlString, "Clients");

            List<Client> clients = new List<Client>();

            StringBuilder sb = new StringBuilder();

            foreach (var clientDto in clientsDtos)
            {
                if (clientDto.Name.Length < 10 || clientDto.Name.Length > 25)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (clientDto.NumberVat.Length < 10 || clientDto.NumberVat.Length > 15)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client1 = new Client();
                client1.Name = clientDto.Name;
                client1.NumberVat = clientDto.NumberVat;

                foreach (var clientDtoAddress in clientDto.Addresses)
                {
                    if (clientDtoAddress.StreetName.Length < 10 || clientDtoAddress.StreetName.Length > 20)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (clientDtoAddress.City.Length < 5 || clientDtoAddress.City.Length > 15)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (clientDtoAddress.Country.Length < 5 || clientDtoAddress.Country.Length > 15)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address address1 = new Address();
                    address1.StreetName = clientDtoAddress.StreetName;
                    address1.StreetNumber = clientDtoAddress.StreetNumber;
                    address1.PostCode = clientDtoAddress.PostCode;
                    address1.City = clientDtoAddress.City;
                    address1.Country = clientDtoAddress.Country;

                    client1.Addresses.Add(address1);
                }

                clients.Add(client1);
                sb.AppendLine(string.Format(SuccessfullyImportedClients, client1.Name));
            }

            context.Clients.AddRange(clients);
            context.SaveChanges();

            return sb.ToString();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            ImportInvoiceDto[] invoiceDtos = JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString);

            List<Invoice> invoices = new List<Invoice>();

            StringBuilder sb = new StringBuilder();

            foreach (var invoiceDto in invoiceDtos)
            {
                if (invoiceDto.Number < 1000000000 || invoiceDto.Number > 1500000000)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(invoiceDto.IssueDate > invoiceDto.DueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (invoiceDto.Amount <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (invoiceDto.CurrencyType < 0 || invoiceDto.CurrencyType > 2)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (invoiceDto.ClientId < 1 || invoiceDto.ClientId > context.Clients.Count())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Invoice invoice1 = new Invoice();
                invoice1.Number = invoiceDto.Number;
                invoice1.IssueDate = invoiceDto.IssueDate;
                invoice1.DueDate = invoiceDto.DueDate;
                invoice1.Amount = invoiceDto.Amount;
                invoice1.CurrencyType = (Data.Models.Enums.CurrencyType)invoiceDto.CurrencyType;
                invoice1.ClientId = invoiceDto.ClientId;

                invoices.Add(invoice1);

                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice1.Number));
            }

            context.Invoices.AddRange(invoices);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            ImportProductDto[] productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);

            List<Product> products = new List<Product>();

            StringBuilder sb = new StringBuilder();

            foreach (var productDto in productDtos)
            {
                if (productDto.Name.Length < 9 || productDto.Name.Length > 30)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (productDto.Price < 5 || productDto.Price > 1000)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (productDto.CategoryType < 0 || productDto.CategoryType > 4)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product product1 = new Product();
                product1.Name = productDto.Name;
                product1.Price = productDto.Price;
                product1.CategoryType = (Data.Models.Enums.CategoryType)productDto.CategoryType;

                foreach (int productClientDto in productDto.Clients.Distinct())
                {
                    Client client1 = context.Clients.FirstOrDefault(x => x.Id == productClientDto);

                    if (client1 == default)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ProductClient productClient1 = new ProductClient();
                    productClient1.Product = product1;
                    productClient1.Client = client1;

                    product1.ProductsClients.Add(productClient1);
                }

                products.Add(product1);
                sb.AppendLine(string.Format(SuccessfullyImportedProducts, product1.Name, product1.ProductsClients.Count));
            }

            context.AddRange(products);
            context.SaveChanges();

            return sb.ToString();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static T Deserializerr<T>(string inputXml, string rootName)
        {
            XmlRootAttribute inputXML = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), inputXML);

            using StringReader reader = new StringReader(inputXml);

            T dtos = (T)serializer.Deserialize(reader);
            return dtos;
        }
    } 
}
