using Piranha.AttributeBuilder;
using Piranha.Models;

namespace catfish.Models
{
    [PostType(Title = "Standard post")]
    public class StandardPost  : Post<StandardPost>
    {
    }
}