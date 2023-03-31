namespace Footballers.DataProcessor
{
    using AutoMapper;
    using Castle.Core.Internal;
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            var coachesDto = Deserialize<ImportCoachDto[]>(xmlString, "Coaches");

            StringBuilder sb = new StringBuilder();

            List<Coach> coaches = new List<Coach>();

            foreach (var coach in coachesDto)
            {
                if (!IsValid(coach) || coach.Nationality.IsNullOrEmpty() || coach.Name.Length < 2 || coach.Name.Length > 40)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                List<Footballer> footballers = new List<Footballer>();

                foreach (var footballer in coach.Footballers)
                {
                    bool footballerIsValid = true;

                    if (!IsValid(footballer) || footballer.Name.IsNullOrEmpty() || footballer.Name.Length < 2 || footballer.Name.Length > 40
                        || !footballer.PositionType.HasValue || footballer.PositionType.Value > 3 || footballer.PositionType.Value < 0
                        || !footballer.BestSkillType.HasValue || footballer.BestSkillType.Value > 4 || footballer.BestSkillType.Value < 0
                        || footballer.ContractStartDate.IsNullOrEmpty() || footballer.ContractStartDate.IsNullOrEmpty())
                    {
                        footballerIsValid = false;
                    }

                    if (footballerIsValid)
                    {
                        DateTime startDate = DateTime.ParseExact(footballer.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        DateTime endDate = DateTime.ParseExact(footballer.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if (startDate > endDate)
                            footballerIsValid = false;
                    }

                    if (!footballerIsValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer footballerEntity = new Footballer();
                    footballerEntity.Name = footballer.Name;
                    footballerEntity.ContractStartDate = DateTime.ParseExact(footballer.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    footballerEntity.ContractEndDate = DateTime.ParseExact(footballer.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    footballerEntity.BestSkillType = (BestSkillType)footballer.BestSkillType;
                    footballerEntity.PositionType = (PositionType)footballer.PositionType;

                    footballers.Add(footballerEntity);
                }

                Coach coachEntity = new Coach();
                coachEntity.Name = coach.Name;
                coachEntity.Nationality = coach.Nationality;
                coachEntity.Footballers = footballers;
                coaches.Add(coachEntity);

                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coachEntity.Name, coachEntity.Footballers.Count));
            }

            context.AddRange(coaches);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportTeamDto[] teamDtos = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);

            Regex regex = new Regex((@"[a-zA-z.\-\d\s]+$"));

            List<Team> teams = new List<Team>();

            foreach (var team in teamDtos)
            {
                Match match = regex.Match(team.Name);

                if (!match.Success || team.Name.Length < 3 || team.Name.Length > 40
                    || team.Nationality.IsNullOrEmpty() || team.Nationality.Length < 2 || team.Nationality.Length > 40
                    || team.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team teamEntity = new Team();
                teamEntity.Name = team.Name;
                teamEntity.Nationality = team.Nationality;
                teamEntity.Trophies = team.Trophies;

                foreach (var footballer in team.Footballers.Distinct())
                {
                    Footballer footballerEntity = context.Footballers.Find(footballer);

                    if (footballerEntity == null) 
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TeamFootballer teamFootballerEntity = new TeamFootballer();
                    teamFootballerEntity.Footballer = footballerEntity;
                    teamFootballerEntity.Team = teamEntity;

                    teamEntity.TeamsFootballers.Add(teamFootballerEntity);
                }

                teams.Add(teamEntity);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, teamEntity.Name, teamEntity.TeamsFootballers.Count));
            }

            context.AddRange(teams);
            context.SaveChanges();

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