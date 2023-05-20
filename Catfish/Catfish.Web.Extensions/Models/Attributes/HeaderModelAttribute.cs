using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HeaderModelAttribute : System.Attribute
    {
        public string Name { get; set; }
        public string ViewTemplate { get; set; }

        static async Task<IEnumerable<DataSelectFieldItem>> GetList()
        {
            var headers = Assets.GetHeaderTypes();

            return headers.Select(p => new DataSelectFieldItem
            {
                Id = p.ViewTemplate,
                Name = p.Name
            });
        }
    }
}
