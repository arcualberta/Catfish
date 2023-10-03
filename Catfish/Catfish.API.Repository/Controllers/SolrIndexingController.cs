using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.API.Repository.Controllers
{
    [Route("api/[controller]")]
    [EnableCors(CorsPolicyNames.General)]
    [ApiController]
    public class SolrIndexingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _uploadRoot;
        private readonly int _numUploadSubfolders;
        private readonly List<string> _uploadSubfolders = new List<string>();
        private readonly Random _rand=  new Random();
        public SolrIndexingController(IConfiguration configuration)
        {
            _configuration = configuration;
            _uploadRoot = _configuration.GetSection("SolarConfiguration:IndexingPayloadUploadRoot").Value;
            if (string.IsNullOrEmpty(_uploadRoot))
                _uploadRoot = @"App_Data\SolrPayloads";

            if (!int.TryParse(_configuration.GetSection("SolarConfiguration:NumIndexingPayloadUploadFolders").Value, out _numUploadSubfolders))
                _numUploadSubfolders = 10;

            //Making sure upload folders exist
            for (int i = 0; i < _numUploadSubfolders; ++i)
                _uploadSubfolders.Add(Path.Combine(_uploadRoot, $"{i}"));
            foreach(var folder in _uploadSubfolders)
                Directory.CreateDirectory(folder); ;
        }

        // POST api/<SolrIndexingController>
        [HttpPost]
        public async Task Post([FromBody] string payload)
        {
            //Select an upload folder at a random
            string foder = _uploadSubfolders[_rand.Next(0, _numUploadSubfolders)];
            string path = Path.Combine(foder, $"{Guid.NewGuid()}.xml");
            await System.IO.File.WriteAllTextAsync(path, payload);
        }
    }
}
