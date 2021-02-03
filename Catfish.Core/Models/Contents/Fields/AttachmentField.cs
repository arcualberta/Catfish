using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class AttachmentField : MonolingualTextField//, IFormFile
    {
      
       // public IFormFile AttachmentFile { get; set; }

       public string FileNames { get; set; }
        public List<string> AcceptExtensions { get; set; } = new List<string>();
        public int MaxFileUploadLimit { get; set; }

       // public string ContentDisposition => AttachmentFile.ContentDisposition;  //throw new NotImplementedException();

      //  public string ContentType => AttachmentFile.ContentType; //throw new NotImplementedException();

      //  public string FileName => AttachmentFile.FileName;


     //   public IHeaderDictionary Headers => AttachmentFile.Headers; //throw new NotImplementedException();

      //  public long Length => AttachmentFile.Length;// throw new NotImplementedException();

     //   string IFormFile.Name => AttachmentFile.Name; //throw new NotImplementedException();

        //public int SetValue(object val, int valueIndex = 0)
        //{
        //    if (Values.Count <= valueIndex)
        //    {
        //        Values.Add(new Text());
        //        valueIndex = Values.Count - 1;
        //    }
        //    Values[valueIndex].Value = val == null ? FileName : val.ToString();
        //    return valueIndex;
        //}
      
        //public void CopyTo(Stream target)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        //public Stream OpenReadStream()
        //{
        //    throw new NotImplementedException();
        //}

        public AttachmentField() {
            DisplayLabel = "Attachment Field";
           // Values = new List<string>(); 
        }
        public AttachmentField(XElement data) : base(data) { DisplayLabel = "Attachment Field"; }
        public AttachmentField(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Attachment Field"; }
    }
}
