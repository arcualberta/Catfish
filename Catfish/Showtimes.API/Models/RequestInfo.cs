using Showtimes.API.Constants;

namespace Showtimes.API.Models
{
    public class RequestInfo
    {
        public Guid Id { get; set; }
        public string RequestLabel { get; set; }
        public string NotificationEmail { get; set; }
        public string RequestBody { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime CompletedAt { get; set; }
        public eRequestStatus RequestStatus { get; set; }

        public List<ResponseInfo> Responses { get; set; } = new List<ResponseInfo>();
    }

}
