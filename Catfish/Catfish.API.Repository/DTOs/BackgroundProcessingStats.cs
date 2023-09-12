namespace Catfish.API.Repository.DTOs
{
    public class BackgroundProcessingStats
    {
        public List<int> Frequencies { get; set; } = new List<int>();
        public int UniqueRecordCount { get; set; } = 0;
        public int TotalCount { get; set; } = 0;
    }
}
