using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using ElmahCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.Core.Services.FormBuilder
{
    public class FormService : DbEntityService, IFormService
    {
        public FormService(AppDbContext db, ErrorLog errorLog)
          : base(db, errorLog)
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

            CheckboxField checkboxes = new CheckboxField();
            checkboxes.SetName("Enter field name", "en");
            checkboxes.SetName("Entrez le nom du champ", "fr");
            checkboxes.SetDescription("Enter field description", "en");
            checkboxes.SetDescription("Entrez la description du champ", "fr");
            checkboxes.AddOption(new string[] { "Option one", "Option un" }, new string[] { "en", "fr" });
            checkboxes.AddOption(new string[] { "Option two", "Option undeux" }, new string[] { "en", "fr"});
            fields.Add(checkboxes);

            RadioField radio = new RadioField();
            radio.SetName("Enter field name", "en");
            radio.SetName("Entrez le nom du champ", "fr");
            radio.SetDescription("Enter field description", "en");
            radio.SetDescription("Entrez la description du champ", "fr");
            radio.AddOption(new string[] { "Option one", "Option un" }, new string[] { "en", "fr" });
            radio.AddOption(new string[] { "Option two", "Option undeux" }, new string[] { "en", "fr" });
            fields.Add(radio);

            SelectField select = new SelectField();
            select.SetName("Enter field name", "en");
            select.SetName("Entrez le nom du champ", "fr");
            select.SetDescription("Enter field description", "en");
            select.SetDescription("Entrez la description du champ", "fr");
            select.AddOption(new string[] { "Option one", "Option un" }, new string[] { "en", "fr" });
            select.AddOption(new string[] { "Option two", "Option undeux" }, new string[] { "en", "fr" });
            fields.Add(select);

            TextField txtField = new TextField();
            txtField.SetName("Enter field name", "en");
            txtField.SetName("Entrez le nom du champ", "fr");
            txtField.SetDescription("Enter field description", "en");
            txtField.SetDescription("Entrez la description du champ", "fr");
            txtField.SetValue("Enter default field value, if necessary", "en");
            txtField.SetValue("Entrez la valeur de champ par défaut, si nécessaire", "fr");
            fields.Add(txtField);

            TextArea textArea = new TextArea();
            textArea.SetName("Enter field name", "en");
            textArea.SetName("Entrez le nom du champ", "fr");
            textArea.SetDescription("Enter field description", "en");
            textArea.SetDescription("Entrez la description du champ", "fr");
            textArea.SetValue("Enter default field value, if necessary", "en");
            textArea.SetValue("Entrez la valeur de champ par défaut, si nécessaire", "fr");
            fields.Add(textArea);

            DateField date = new DateField();
            date.SetName("Enter field name", "en");
            date.SetName("Entrez le nom du champ", "fr");
            date.SetDescription("Enter field description", "en");
            date.SetDescription("Entrez la description du champ", "fr");
            fields.Add(date);

            DecimalField dec = new DecimalField();
            dec.SetName("Enter field name", "en");
            dec.SetName("Entrez le nom du champ", "fr");
            dec.SetDescription("Enter field description", "en");
            dec.SetDescription("Entrez la description du champ", "fr");
            fields.Add(dec);

            InfoSection info = new InfoSection();
            info.SetName("Enter field name", "en");
            info.SetName("Entrez le nom du champ", "fr");
            info.SetDescription("Enter field description", "en");
            info.SetDescription("Entrez la description du champ", "fr");
            fields.Add(info);

            IntegerField num = new IntegerField();
            num.SetName("Enter field name", "en");
            num.SetName("Entrez le nom du champ", "fr");
            num.SetDescription("Enter field description", "en");
            num.SetDescription("Entrez la description du champ", "fr");
            fields.Add(num);

            MonolingualTextField monoText = new MonolingualTextField();
            monoText.SetName("Enter field name", "en");
            monoText.SetDescription("Enter field description", "en");
            monoText.Values.Add(new Text() { Value = "" });
            fields.Add(monoText);

            fields = fields.OrderBy(f => f.DisplayLabel).ToList();

            return fields;
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
