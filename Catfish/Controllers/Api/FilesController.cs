using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ItemService _itemService;
        private readonly AppDbContext _db;
        public FilesController(AppDbContext db, ItemService itemService, IConfiguration configuration)
        {
            _itemService = itemService;
            _db = db;

            ConfigHelper.Configuration = configuration;
        }

        [HttpPost]
        public IActionResult Post(ICollection<IFormFile> files)
        {
            List<FileReference> fileRefs = _itemService.UploadFiles(files);
            return Ok(fileRefs);
        }

    }
}
