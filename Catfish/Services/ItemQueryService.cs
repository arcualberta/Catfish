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
            if(functionName == "COUNT")
            {
                functionName = "SUM"; //when it's only asking for count, this function operation is not matter
            }
            string xQuerySelect = "SELECT " + functionName + "(a.Amount) AS calculatedValue, COUNT(*) as 'count' " +
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

        public IEnumerable<GraphQueryObject> GetGraphData(string xMetadataSet, string xField, string yMetadataSet, string yField, string catMetadataSet, string catField, int xmin = 0, int xmax = 0)
        {
            CatfishDbContext db = new CatfishDbContext();
            xmin = xmin == 0 ? DateTime.MinValue.Year : xmin;
            xmax = xmax == 0 ? DateTime.Now.Year : xmax;
            string xQuerySelect = "SELECT a.Year as YValue, SUM(a.Amount) AS XValue, COUNT(*) as 'Count', a.Category" +
                                  " FROM(" +
                                  " SELECT  Content.value('(/item/metadata/metadata-set[@guid=\"" + xMetadataSet + "\"]/fields/field[@guid=\"" + xField + "\"]/value/text/text())[1]', 'INT') AS Year ," +
                                   " Content.value('(/item/metadata/metadata-set[@guid=\"" + yMetadataSet + "\"]/fields/field[@guid=\"" + yField + "\"]/value/text/text())[1]', 'DECIMAL') AS Amount," +
                                   " Content.value('(/item/metadata/metadata-set[@guid=\"" + catMetadataSet + "\"]/fields/Field[@guid=\"" + catField + "\"]/options/option[@selected=\"true\"]/text/text())[1]', 'VARCHAR(25)') AS Category" +
                                   " FROM[dbo].[CFXmlModels]" +
                                   " WHERE Discriminator = 'CFItem' AND Content.exist('/item/metadata/metadata-set[@guid=\"" + xMetadataSet + "\"]') = 1" +
                                   " ) as a" +
                                    " WHERE a.Year >= " + xmin + " AND a.Year <= " + xmax +
                                    " GROUP BY a.Year, a.Category" +
                                    " ORDER BY a.Year";


            var result = db.Database.SqlQuery<GraphQueryObject>(xQuerySelect, new object[] { xMetadataSet, xField, yMetadataSet, yField, catMetadataSet, catField, xMetadataSet });

            return result;

        }
    }
}