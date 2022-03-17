using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Models.Blocks.ItemLayout
{
	public class FieldLayout: ComponentLayout
	{
		public Guid FormTemplateId { get; set; }
		public Guid FieldId { get; set; }
		public FieldLayout() { Label = "Form Field"; }

		

	}
}
