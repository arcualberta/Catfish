using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public string ToXml(Dictionary<int, List<string>> data)
        {
            List<string> rows = new List<string>();
            foreach(var item in data)
            {
                string row = "<td>" + item.Key.ToString() + "</td><td>" + string.Join("</td><td>", item.Value) + "</td>";
                rows.Add(row);
            }

            string xml = "<table><tr>" + string.Join("</tr><tr>", rows) + "</tr></table>";
            return xml;
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


        /// <summary>
        /// Recursively through all fields in a form to find fields of type ExternalMediaField and 
        /// checks whether the media sources referred by them exist. Returns the list of URLs of media 
        /// sources that does not exist.
        /// </summary>
        /// <param name="model">The form or field.</param>
        /// <returns></returns>
        public List<string> CheckMedia(CFXmlModel model)
        {
            List<string> errorneousMedia = new List<string>();

            if (typeof(Form).IsAssignableFrom(model.GetType()))
            {
                Form form = model as Form;
                foreach (var child in form.Fields)
                    errorneousMedia.AddRange(CheckMedia(child));
            }
            else if (typeof(ExternalMediaField).IsAssignableFrom(model.GetType()))
            {
                if (!CheckMedia((model as ExternalMediaField).Source))
                    errorneousMedia.Add((model as ExternalMediaField).Source);
            }
            else if (typeof(CompositeFormField).IsAssignableFrom(model.GetType()))
            {
                CompositeFormField cfield = model as CompositeFormField;

                foreach (var child in cfield.Header)
                    errorneousMedia.AddRange(CheckMedia(child));

                foreach (var child in cfield.Fields)
                    errorneousMedia.AddRange(CheckMedia(child));

                foreach (var child in cfield.Footer)
                    errorneousMedia.AddRange(CheckMedia(child));
            }

            return errorneousMedia;
        }

        public bool CheckMedia(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                //Any exception will returns false.
                return false;
            }
        }

    }
}
