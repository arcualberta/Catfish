using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.Core.Services.FormBuilder
{
    public class FormService : DbEntityService, IFormService
    {
        public FormService(AppDbContext db)
          : base(db)
        {

        }

        public FieldContainer Get(Guid id)
        {
            FieldContainer container = Db.Forms.Where(x => x.Id == id).FirstOrDefault();
            container.Initialize(XmlModel.eGuidOption.Ignore);
            return container;
        }

        public FieldContainerListVM Get(int offset = 0, int? max = null)
        {
            IQueryable<Form> query = Db.Forms.Skip(offset);
            if (max.HasValue)
                query = query.Take(max.Value);

            var forms = query.ToList();
            FieldContainerListVM result = new FieldContainerListVM()
            {
                OffSet = offset,
                Max = max,
                Entries = forms.Select(x => new FieldContainerListEntry(x)).ToList()
            };
            return result;
        }

        public List<BaseField> GetFieldDefinitions()
        {
            List<BaseField> fields = new List<BaseField>();

            TextField txt = new TextField();
            txt.SetName("Text Field", "en");
            txt.SetName("Champ de texte", "fr");
            txt.SetValue("Text Value", "en");
            txt.SetValue("Valeur du texte", "fr");
            fields.Add(txt);


            return fields;
        }

        private MultilingualName CreateName(string priVal, string priLang, string altVal = null, string altLang = null)
        {
            MultilingualName obj = new MultilingualName();
            obj.Values.Add(new Text(priVal, priLang));
            if (!string.IsNullOrEmpty(altVal))
                obj.Values.Add(new Text(altVal, altLang));

            return obj;
        }

        private MultilingualDescription CreateDescriptoin(string priVal, string priLang, string altVal = null, string altLang = null)
        {
            MultilingualDescription obj = new MultilingualDescription();
            obj.Values.Add(new Text(priVal, priLang));
            if (!string.IsNullOrEmpty(altVal))
                obj.Values.Add(new Text(altVal, altLang));

            return obj;
        }
        ////public FieldContainerListVM GetMetadataSets(int offset = 0, int max = 0)
        ////{
        ////    IQueryable<MetadataSet> query = Db.MetadataSets.Skip(offset);
        ////    if (max > 0)
        ////        query = query.Take(max);

        ////    var forms = query.ToList();
        ////    FieldContainerListVM result = new FieldContainerListVM()
        ////    {
        ////        OffSet = offset,
        ////        Max = max,
        ////        Entries = forms.Select(x => new FieldContainerListEntry(x)).ToList()
        ////    };
        ////    return result;

        ////}
    }
}
