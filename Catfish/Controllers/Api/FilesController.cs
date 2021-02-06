using Catfish.Core.Models.Contents;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public FilesController(ItemService itemService)
        {
            _itemService = itemService;
        }
        public IActionResult OnPost(Guid id, ICollection<IFormFile> files)
        {
            string dictFileNames = "";

            var fileReferences = _itemService.UploadFiles(id, files);

            return Ok(dictFileNames);
        }
    }
}
