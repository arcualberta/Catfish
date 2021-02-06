using Catfish.Core.Models.Contents;
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
        public IActionResult OnPost(ICollection<IFormFile> files)
        {
            string dictFileNames = "";

            foreach (IFormFile file in files)
            {
                FileReference fileRef = new FileReference();
                fileRef.Size = file.Length;
                fileRef.OriginalName = file.FileName;
                fileRef.ContentType = file.ContentType;

                string newGuid = Guid.NewGuid().ToString(); //System.Text.RegularExpressions.Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                //create temp directory for login user
                if (!Directory.Exists("wwwroot/uploads/temp"))
                    Directory.CreateDirectory("wwwroot/uploads/temp");

                string fileN = newGuid + "_" + file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/temp", fileN);

                string[] _fileNames = file.FileName.Split("__"); //this will get the field index ==> filed_4, this will be the key for the file name(s) of this field
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                    //fileNames.Add(fileN);
                    if (!dictFileNames.Contains(_fileNames[1], StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrWhiteSpace(dictFileNames))
                            dictFileNames = _fileNames[1] + ":" + fileN; //still empty
                        else
                            dictFileNames += "|" + _fileNames[1] + ":" + fileN;
                    }
                    else
                    {
                        dictFileNames += "," + fileN;
                    }
                    // dictFileNames.Add(_fileNames[1], new List<string>(listfnames));
                }
            }
            return Ok(dictFileNames);
        }
    }
}
