using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models
{
    public class SaveCommentModel
    {
        public Guid Id { get; set; }
        public string CommentAuthor { get; set; }
        public string CommentEmail { get; set; }
        public string CommentUrl { get; set; }
        public string CommentBody { get; set; }
    }
}
