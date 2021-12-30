using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Regions
{
	public class Restrictions
	{
        [Field(Title = "Lock the site", Placeholder = "Lock the site")]
        public CheckBoxField LockSite { get; set; } = false;

		[Field(Title = "Site Lock Image")]
		public ImageField LockImage { get; set; }

		[Field(Title = "Site Lock Message")]
		public HtmlField LockMessage { get; set; }
	}
}
