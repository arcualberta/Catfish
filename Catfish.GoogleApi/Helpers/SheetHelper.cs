using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.GoogleApi.Helpers
{
    public static class SheetHelper
    {
        public static int Letter2Column(string col)
        {
            col = col.ToUpper();
            int column = 0, length = col.Length;
            for (var i = 0; i < length; i++)
            {
                column += (col[i] - 64) * (int)Math.Pow(26, length - i - 1);
            }
            return column;
        }

        public static string Column2Letter(int col)
        {
            int temp;
            string letter = "";
            while (col > 0)
            {
                temp = (col - 1) % 26;
                letter = ((char)(temp + 65)) + letter;
                col = (col - temp - 1) / 26;
            }
            return letter;
        }
    }
}
