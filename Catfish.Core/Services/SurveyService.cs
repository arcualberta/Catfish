using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    public class SurveyService : EntityService
    {
        public SurveyService(CatfishDbContext db) : base(db) { }

        public Dictionary<int, List<string>> ExportFormData(int collectionId)
        {
            //TODO: Restrict exporting data only from items that are viewable to the current user.
            CollectionService collectionSrv = new CollectionService(Db);
            CFCollection collection = collectionSrv.GetCollection(collectionId, AccessMode.Read);
            if (collection == null)
                return null;

            List<Dictionary<string, string>> submissionDataSet = new List<Dictionary<string, string>>();
            foreach (var item in collection.ChildItems)
            {
                foreach (var submission in (item as CFItem).FormSubmissions)
                {
                    Form form = submission.FormData;

                    //Exporting data from all fields in the form
                    Dictionary<string, List<string>> formData = new Dictionary<string, List<string>>();
                    foreach (var field in form.Fields)
                    {
                        Dictionary<string, List<string>> fieldValues = ExportValues(field);
                        foreach (var val in fieldValues)
                        {
                            if (formData.ContainsKey(val.Key))
                                formData[val.Key].AddRange(val.Value);
                            else
                                formData.Add(val.Key, val.Value);
                        }
                    }

                    Dictionary<string, string> response = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(form.ReferenceCode))
                        response.Add("-1", form.ReferenceCode);

                    foreach (var answer in formData)
                    {
                        response.Add(answer.Key, string.Join("|", answer.Value));
                    }

                    submissionDataSet.Add(response);
                }
            }

            //Exporting data into a spreadsheet
            List<int> questionKeys = new List<int>();
            foreach(var submissionData in submissionDataSet)
            {
                var keys = submissionData.Keys.Select(k => int.Parse(k));
                questionKeys = questionKeys.Union(keys).ToList();
            }
            questionKeys.Sort();

            Dictionary<int, List<string>> exportData = new Dictionary<int, List<string>>();
            foreach (var key in questionKeys)
                exportData.Add(key, new List<string>());

            foreach(var submission in submissionDataSet)
            {
                foreach(var key in questionKeys)
                {
                    string val = submission.ContainsKey(key.ToString()) ? submission[key.ToString()] : "";
                    exportData[key].Add(val);
                }
            }

            return exportData;
        }

        public string ToCsv(Dictionary<int, List<string>> data)
        {
            List<string> rows = new List<string>();
            foreach(var item in data)
            {
                string row = item.Key.ToString() + "," + string.Join(",", item.Value);
                rows.Add(row);
            }

            string csv = string.Join("\n", rows);
            return csv;
        }

        public Dictionary<string, List<string>> ExportValues(FormField field)
        {
            Dictionary<string, List<string>> values = new Dictionary<string, List<string>>();

            if (field is HtmlField || field is ExternalMediaField)
                return values; //These fields do not take user inputs.

            if(field is CompositeFormField)
            {
                //exporting values from the header
                foreach(var child in (field as CompositeFormField).Header)
                {
                    Dictionary<string, List<string>> childValues = ExportValues(child);
                    foreach(var obj in childValues)
                    {
                        if (values.ContainsKey(obj.Key))
                            values[obj.Key].AddRange(obj.Value);
                        else
                            values.Add(obj.Key, obj.Value);
                    }
                }

                //exporting values from the child fields
                foreach (var child in (field as CompositeFormField).Fields)
                {
                    Dictionary<string, List<string>> childValues = ExportValues(child);
                    foreach (var obj in childValues)
                    {
                        if (values.ContainsKey(obj.Key))
                            values[obj.Key].AddRange(obj.Value);
                        else
                            values.Add(obj.Key, obj.Value);
                    }
                }

                //exporting values from the footer fields
                foreach (var child in (field as CompositeFormField).Footer)
                {
                    Dictionary<string, List<string>> childValues = ExportValues(child);
                    foreach (var obj in childValues)
                    {
                        if (values.ContainsKey(obj.Key))
                            values[obj.Key].AddRange(obj.Value);
                        else
                            values.Add(obj.Key, obj.Value);
                    }
                }

            }
            else
            {
                var vals = field.GetValues();
                if(vals.Count() > 0)
                {
                    foreach (var val in vals)
                    {
                        if (values.ContainsKey(field.ReferenceLabel))
                            values[field.ReferenceLabel].Add(val.Value);
                        else
                            values.Add(field.ReferenceLabel, new List<string>() { val.Value });
                    }
                }
                else
                    values.Add(field.ReferenceLabel, new List<string>());
            }

            return values;
        }
    }
}
