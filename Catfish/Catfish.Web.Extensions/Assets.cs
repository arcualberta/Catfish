using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions
{
    public static class Assets
    {
        public static List<PartialView> Headers { get; private set; } = new List<PartialView>();
        public static List<PartialView> Footers { get; private set; } = new List<PartialView>();
    }

    public class PartialView
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public PartialView() { }
        public PartialView(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}
