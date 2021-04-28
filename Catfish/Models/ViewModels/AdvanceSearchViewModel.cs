using Catfish.Core.Models.Solr;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.ViewModels
{
    public class AdvanceSearchViewModel
    {

        public int ItemPerPage { get; set; }
       
        List<SearchFieldConstraint> SearchConstrains { get; set; }

    }
}
