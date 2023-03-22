using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO.Forms
{
    public class FileReference
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string Thumbnail { get; set; }
        public string ContentType { get; set; }
        public int Size { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public Guid FormDataId { get; set; }
        public Guid FieldId { get; set; }
    }
}
