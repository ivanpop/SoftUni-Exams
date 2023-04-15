using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoices.Data.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 10)]
        public string StreetName  { get; set; } = null!;

        public int StreetNumber { get; set; }

        public string PostCode { get; set; } = null!;

        [StringLength(15, MinimumLength = 5)]
        public string City { get; set; } = null!;

        [StringLength(15, MinimumLength = 5)]
        public string Country { get; set; } = null!;

        [Required]
        public int ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!;
    }
}