using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class CFAggregationAssociationsViewModel
    {
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }
        [JsonProperty("itemsPerPage")]
        public int ItemsPerPage { get; set; }
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
        [JsonProperty("data")]
        public IEnumerable<Object> Data { get; set; }
    }
}