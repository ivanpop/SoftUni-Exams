namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellerDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public int[] Boardgames { get; set; }
    }
}