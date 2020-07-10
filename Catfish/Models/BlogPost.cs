using Catfish.Models.Regions;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;

namespace Catfish.Models
{
    /// <summary>
    /// Basic post with main content in markdown.
    /// </summary>
    [PostType(Title = "Blog post")]
    public class BlogPost : Post<BlogPost>
    {
        /// <summary>
        /// Gets/sets the heading.
        /// </summary>
        [Region()]
        public Regions.Hero Hero { get; set; }
    }
}