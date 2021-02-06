using Catfish.Core.Models;
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
        private readonly AppDbContext _db;
        public FilesController(AppDbContext db, ItemService itemService)
        {
            _itemService = itemService;
            _db = db;
        }

        [HttpGet("{fieldId}")]
        public IActionResult Get(Guid fieldId)
        {
            List<FileReference> fileRefs = _db.FileReferences
                .Where(fr => fr.FieldId == fieldId)
                .ToList();

            return Ok(fileRefs);
        }

        [HttpPost("{fieldId}")]
        public IActionResult Post(Guid fieldId, ICollection<IFormFile> files)
        {
            if (files.Count > 0)
                _itemService.UploadFiles(fieldId, files);

            List<FileReference> fileRefs = _db.FileReferences
                .Where(fr => fr.FieldId == fieldId)
                .ToList();

            return Ok(fileRefs);
        }
    }
}
