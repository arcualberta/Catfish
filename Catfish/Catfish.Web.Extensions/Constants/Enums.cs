using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Constants
{
    public class Enums
    {
        public enum eUsage
        {
            Carousel = 0,
            Content

        }
        public enum eSlideLayout
        {
            ImageOnly = 0,
            Overlay,
            SideBySide,
            TextTop,
            TextBottom
           
        }
        public enum eArchiveListLayout
        {
            Block = 0,
            List
        }
        public enum eArchivePostLayout
        {
            SideBySide = 0,
            Upright
        }
    }
}
