using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Models.Blocks.ItemLayout
{
	public class StaticText: ComponentLayout
	{
		public string Content { get; set; }
		public StaticText() { Label = "Static Text"; }
	}
}
