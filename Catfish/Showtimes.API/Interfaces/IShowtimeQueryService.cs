using Showtimes.API.DTO;

namespace Showtimes.API.Interfaces
{
    public interface IShowtimeQueryService
    {
        public void NotifyUser(string requestLabel, string notificationEmail);
        public Task<int> CountShowtimes(QueryParams param);
        public string? BuildingQueryString(QueryParams queryParams);
    }
}
