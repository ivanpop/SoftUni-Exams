using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImportFootballerDto
    {
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required]
        public string? ContractStartDate { get; set; }

        [Required]
        public string? ContractEndDate { get; set; }

        [Required]
        public int? BestSkillType { get; set; }

        [Required]
        public int? PositionType { get; set; }
    }
}