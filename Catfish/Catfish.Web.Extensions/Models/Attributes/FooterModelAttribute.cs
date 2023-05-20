using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FooterModelAttribute : System.Attribute
    {
        public string Name { get; set; }
        public string ViewTemplate { get; set; }

        static async Task<IEnumerable<DataSelectFieldItem>> GetList()
        {
            var footers = Assets.GetFooterTypes();

            return footers.Select(p => new DataSelectFieldItem
            {
                Id = p.ViewTemplate,
                Name = p.Name
            });
        }
    }
}
