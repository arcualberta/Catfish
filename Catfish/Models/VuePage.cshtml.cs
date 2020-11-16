using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.AttributeBuilder;
using Piranha.Models;

namespace Catfish.Models
{
    [PageType(Title = "Vue Page")]
    public class VuePage : StandardPage
    {
    }
}
