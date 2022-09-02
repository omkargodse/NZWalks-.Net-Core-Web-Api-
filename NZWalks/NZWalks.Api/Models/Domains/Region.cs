namespace NZWalks.Api.Models.Domains
{
    public class Region
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public double Area { get; set; }
        public double Lat { get; set; }

        public double Long { get; set; }
        public long Population { get; set; }

        //Navigation property ( 1 Region has Many walks)
        public IEnumerable<Walk> Walks  { get; set; }
    }
}
