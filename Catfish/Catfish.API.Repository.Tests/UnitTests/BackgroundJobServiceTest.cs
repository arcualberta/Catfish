using Catfish.API.Repository.Controllers;
using Catfish.API.Repository.Interfaces;

namespace Catfish.API.Repository.Tests.UnitTests
{
    public class BackgroundJobServiceTests 
    {
      
        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://localhost:5020") };
        // private readonly IBackgroundJobService _backgroundJobService;
        //public BackgroundJobServiceTests(IBackgroundJobService backgroundJobService)
        //{
        //    _backgroundJobService = backgroundJobService;
        //}

        [Fact]
        public async Task RunBackgroundJobDummyTest()
        {
            // Arrange.

            // Act.
            var jobid = await _httpClient.GetAsync("/api/background-job");
            // Assert.

          
            Assert.NotNull(jobid);
        }
    }
}
