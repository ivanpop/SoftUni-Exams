namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;
    using System.Text;
    using Boardgames.Data;
    using System.Xml.Serialization;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Data.Models;
    using Castle.Core.Internal;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            var creatersDtos = Deserializerr<ImportCreatorDto[]>(xmlString, "Creators");

            StringBuilder sb = new StringBuilder();

            List<Creator> creators = new List<Creator>();

            foreach (var creatorDto in creatersDtos)
            {
                Creator creator1 = new Creator();

                if (creatorDto.FirstName.Length < 2 || creatorDto.FirstName.Length > 7)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (creatorDto.LastName.Length < 2 || creatorDto.LastName.Length > 7)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                creator1.FirstName = creatorDto.FirstName;
                creator1.LastName = creatorDto.LastName;


                foreach (var boardgameDto in creatorDto.Boardgames.Distinct())
                {
                    if (boardgameDto.Name.Length < 10 || boardgameDto.Name.Length > 20)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }                    

                    if (boardgameDto.Rating < 1.0 || boardgameDto.Rating > 10.0)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (boardgameDto.YearPublished < 2018 || boardgameDto.YearPublished > 2023)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (boardgameDto.CategoryType < 0 || boardgameDto.CategoryType > 4)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (boardgameDto.Mechanics.IsNullOrEmpty())
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgame1 = new Boardgame();
                    boardgame1.Name = boardgameDto.Name;
                    boardgame1.Rating = boardgameDto.Rating;
                    boardgame1.YearPublished = boardgameDto.YearPublished;
                    boardgame1.CategoryType = (Data.Models.Enums.CategoryType)boardgameDto.CategoryType;
                    boardgame1.Mechanics = boardgameDto.Mechanics;

                    creator1.Boardgames.Add(boardgame1);
                }

                sb.AppendLine(String.Format(SuccessfullyImportedCreator, creator1.FirstName, creator1.LastName, creator1.Boardgames.Count));
                creators.Add(creator1);
            }

            context.AddRange(creators);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            ImportSellerDto[] sellerDtos = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();

            Regex regex = new Regex(@"www[.][A-Za-z\d-]+[.]com");

            List<Seller> sellers = new List<Seller>();

            foreach (var sellerDto in sellerDtos)
            {
                if (sellerDto.Name.Length < 5 || sellerDto.Name.Length > 20)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (sellerDto.Address.Length < 2 || sellerDto.Address.Length > 30)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (sellerDto.Country.IsNullOrEmpty())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Match match = regex.Match(sellerDto.Website);

                if (!match.Success)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller1 = new Seller();
                seller1.Name = sellerDto.Name;
                seller1.Address = sellerDto.Address;
                seller1.Country = sellerDto.Country;
                seller1.Website = sellerDto.Website;

                foreach (int boardGameDto in sellerDto.Boardgames.Distinct())
                {
                    Boardgame boardgame1 = context.Boardgames.FirstOrDefault(x => x.Id == boardGameDto);

                    if (boardgame1 == default)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    BoardgameSeller boardgameSeller1 = new BoardgameSeller();
                    boardgameSeller1.Boardgame = boardgame1;
                    boardgameSeller1.Seller = seller1;

                    seller1.BoardgamesSellers.Add(boardgameSeller1);
                }

                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller1.Name, seller1.BoardgamesSellers.Count));

                context.Sellers.Add(seller1);
                context.SaveChanges();
            }

            return sb.ToString();
        }

        private static bool IsValid(object dto)
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
