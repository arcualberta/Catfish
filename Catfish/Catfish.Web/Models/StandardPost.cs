using Piranha.AttributeBuilder;
using Piranha.Models;

namespace Catfish.Web.Models
{
    [PostType(Title = "Standard post")]
    public class StandardPost  : Post<StandardPost>
    {
    }
}