using Catfish.Areas.Applets.Models.Processes;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Controllers
{
	[Route("applets/api/[controller]")]
	[ApiController]
	public class ReindexController : ControllerBase
	{
		private readonly AppDbContext _db;
		private readonly ISolrService _solrService;
		public ReindexController(AppDbContext db, ISolrService solrService)
		{
			_db = db;
			_solrService = solrService;
		}

		[HttpPost]
		[Route("data")]
		public IndexingStatus.eIndexingStatus ReindexData()
		{
			//TODO: Reimplement the following synchronous code to assynchrounus code using
			//Hangfire and return IndexingStatus.eIndexingStatus.InProgress at the end.

			//Indexing all items taking one batch at a time.
			int maxBatchSize = 500;
			int offset = 0;
			List<Entity> items = null;
			do
			{
				items = _db.Items.Skip(offset).Take(maxBatchSize).Select(item => item as Entity).ToList();
				_solrService.Index(items);
			}
			while (items.Count == maxBatchSize);

			return IndexingStatus.eIndexingStatus.Ready;
		}

		[HttpPost]
		[Route("pages")]
		public IndexingStatus.eIndexingStatus ReindexPages()
		{
			//TODO: Implement this action assynchrounusly to index pages using
			//Hangfire and return IndexingStatus.eIndexingStatus.InProgress at the end.

			//Just sleep to help testing front-end state-transitions.
			System.Threading.Thread.Sleep(1000);

			return IndexingStatus.eIndexingStatus.Ready;
		}

		[HttpGet]
		[Route("status")]
		public IndexingStatus ReindexStatus()
		{
			//TODO: Implement this action to check the status of page reindexing and data reindexing
			//jobs run with Hangfier and return their statuses as InProgress or Ready.

			return new IndexingStatus { pageIndexingStatus = IndexingStatus.eIndexingStatus.Ready, DataIndexingStatus = IndexingStatus.eIndexingStatus.Ready };
		}
	}
}
