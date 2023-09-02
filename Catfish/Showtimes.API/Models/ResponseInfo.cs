namespace Showtimes.API.Models
{
    public class ResponseInfo
    {
        public Guid Id { get; set; }
        public string? ResponseBody { get; set; }
        public string? ResponseFile { get; set; }
        public Guid RequestId { get; set; }
        public RequestInfo Request { get; set; }
    }
}
