namespace Invoices.DataProcessor.ImportDto
{
    public class ImportProductDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int CategoryType { get; set;}

        public int[] Clients { get; set; }
    }
}