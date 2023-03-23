using CatfishExtensions.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO.Forms
{
   public class FormTemplateDto
    {
        /// <summary>
        /// Unique form ID.
        /// </summary>
      
        public Guid Id { get; set; }

        /// <summary>
        /// Status of the form.
        /// </summary>
        public eState Status { get; set; }

        /// <summary>
        /// Form name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Form description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Created timestamp.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;

        /// <summary>
        /// Last updated timestamp.
        /// </summary>
        public DateTime Updated { get; set; }
        public IList<Field>? Fields { get; set; }= new List<Field>();

    }
}
