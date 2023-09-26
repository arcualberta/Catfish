namespace Catfish.API.Repository.Models.BackgroundJobs
{
    public class JobRecord
    {
        public Guid Id { get; set; }
        public string? JobLabel { get; set; }
        public int ProcessedDataRows { get; set; }
        public int ExpectedDataRows { get; set; }
        public string Status { get; set; }
        public string? DataFile { get; set; }
        public string? DownloadDataFileLink { get; set; }
        public string? DownloadStatsFileLink { get; set; }
        public long DataFileSize { get; set; }
        public DateTime Started { get; set; }
        public DateTime LastUpdated { get; set; }
        public string? Message { get; set; }

        public string? JobId { get; set; } //Hangfire job id

        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }

        public int Offset { get; set; } = 0;
        public int AttemptCount { get; set; } = 0;

        public string GetStatsFileName() =>  DataFile?.Substring(0, DataFile.Length - 4) + "-stats.csv";

    }
}
