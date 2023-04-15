using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models
{
    public class ProductClient
    {
        [Required, ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;


        [Required, ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        public Client Client { get; set; } = null!;
    }
}