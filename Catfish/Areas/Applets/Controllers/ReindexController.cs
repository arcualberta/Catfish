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
		public bool ReindexData()
		{
			var items = _db.Items.Select(item => item as Entity).ToList();
			_solrService.Index(items);
			return true;
		}

		[HttpPost]
		[Route("pages")]
		public bool ReindexPages()
		{
			return true;
		}

		[HttpGet]
		[Route("status")]
		public IndexingStatus ReindexStatus()
		{

			return new IndexingStatus { PageIndexingInprogress = false, DataIndexingInprogress = false };
		}
	}
}
