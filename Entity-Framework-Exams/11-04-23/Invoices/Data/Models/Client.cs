using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models
{
    public class Client
    {
        public Client()
        {
            ProductsClients = new HashSet<ProductClient>();
            Addresses = new HashSet<Address>();
            Invoices = new HashSet<Invoice>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(25, MinimumLength = 10)]
        public string Name { get; set; } = null!;

        [StringLength(15, MinimumLength = 10)]
        public string NumberVat { get; set; } = null!;

        public ICollection<Invoice> Invoices { get; set; } = null!;

        public ICollection<Address> Addresses { get; set; } = null!;

        public ICollection<ProductClient> ProductsClients { get; set; } = null!;
    }
}