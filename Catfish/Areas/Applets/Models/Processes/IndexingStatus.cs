using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Models.Processes
{
	public class IndexingStatus
	{
		public enum eIndexingStatus { InProgress = 1, Ready };

		public eIndexingStatus pageIndexingStatus { get; set; }
		public eIndexingStatus DataIndexingStatus { get; set; }
	}
}
