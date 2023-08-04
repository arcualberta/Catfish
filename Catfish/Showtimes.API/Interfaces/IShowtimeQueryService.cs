using Showtimes.API.DTO;

namespace Showtimes.API.Interfaces
{
    public interface IShowtimeQueryService
    {
        public void NotifyUser(string requestLabel, string notificationEmail);
        public Task<int> CountShowtimesAsync(QueryParams param);
        public Task<int> CountShowtimes(QueryParams param, out int count);
        public string? BuildingQueryString(QueryParams queryParams);
    }
}
