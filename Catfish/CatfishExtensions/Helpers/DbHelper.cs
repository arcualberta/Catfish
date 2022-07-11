using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Helpers
{
    public static class DbHelper
    {
        public static void SetTablePrefix(ModelBuilder builder, string tablePrefix)
        {
            if (!string.IsNullOrEmpty(tablePrefix))
            {
                foreach (var entity in builder.Model.GetEntityTypes())
                    entity.SetTableName(tablePrefix + entity.GetTableName());
            }
        }
    }
}
