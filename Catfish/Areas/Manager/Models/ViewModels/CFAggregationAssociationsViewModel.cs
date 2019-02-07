using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class CFAggregationAssociationsViewModel
    {
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
        [JsonProperty("headers")]
        public Object[] Headers { get; set; }
        [JsonProperty("data")]
        public IEnumerable<Object> Data { get; set; }
    }
}