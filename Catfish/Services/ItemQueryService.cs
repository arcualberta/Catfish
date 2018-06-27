using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Catfish.Core.Services;
using Catfish.Core.Models;
using Catfish.Models.Regions;

namespace Catfish.Services
{
    public class ItemQueryService //: ServiceBase
    {
        
       // public ItemQueryService(CatfishDbContext db) : base(db) { }

        public IEnumerable<QueryResultObject> GetCalculatedField(string functionName, string SelectedFieldMetadataSet, string SelectedField, string SelectedFilterMetadataSet, string selectedFilterField, int min, int max)
        {
            CatfishDbContext db = new CatfishDbContext();
            string xQuerySelect = "SELECT " + functionName + "(a.Amount) AS calculatedValue " +
                             " FROM(" +
                             " SELECT  Content.value('(/item/metadata/metadata-set[@guid=\"" + SelectedFieldMetadataSet + "\"]/fields/field[@guid=\"" + SelectedField + "\"]/value/text/text())[1]', 'DECIMAL') AS Amount, " +
                              " Content.value('(/item/metadata/metadata-set[@guid=\"" + SelectedFilterMetadataSet + "\"]/fields/field[@guid=\"" + selectedFilterField + "\"]/value/text/text())[1]', 'INT') AS Year " +

                              " FROM [dbo].[CFXmlModels]" +
                              " WHERE Discriminator = 'CFItem' AND Content.exist('/item/metadata/metadata-set[@guid=\"" + SelectedFieldMetadataSet + "\"]') = 1" +
                              " ) as a" +
                               " WHERE a.Year >= " + min + " AND a.Year <= " + max;


            var result = db.Database.SqlQuery<QueryResultObject>(xQuerySelect, new object[] { functionName, SelectedFieldMetadataSet, SelectedField, SelectedFilterMetadataSet, selectedFilterField, SelectedFieldMetadataSet });
            return result;
        }

    }
}