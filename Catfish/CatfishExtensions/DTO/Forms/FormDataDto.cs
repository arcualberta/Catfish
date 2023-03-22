using CatfishExtensions.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO.Forms
{
    public class FormDataDto
    {
        /// <summary>
        /// Unique FormData object ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Form definition ID
        /// </summary>
        public Guid FormId { get; set; }

        /// <summary>
        /// Created timestamp.
        /// </summary>
      //
        /// <summary>
        /// Last updated timestamp.
        /// </summary>
      //  public DateTime Updated { get; set; }

        public eState State { get; set; }
        public IList<FieldData>? FieldData { get; set; }
    }
}
