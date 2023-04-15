namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoiceDto
    {
        public int Number { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public Decimal Amount { get; set; }

        public int CurrencyType { get; set; }

        public int ClientId { get; set; }
    }
}