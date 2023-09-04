using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Models.BackgroundJobs;
using Catfish.API.Repository.Solr;
using CatfishExtensions.DTO;
using CatfishExtensions.Services;
using CsvHelper;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Catfish.API.Repository.Services
{
    public class SolrService : ISolrService
    {
        protected readonly IConfiguration _config;
        private readonly string _solrCoreUrl;
        //private readonly ErrorLog _errorLog;
        private readonly bool _indexFieldNames;
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly RepoDbContext _db;
        private readonly IEmailService _email;

        //public SolrService()
        //{

        //}

        public SolrService(IConfiguration config, RepoDbContext db, IEmailService email)
        {
            _config = config;
            _db = db;
            _email = email;
            _solrCoreUrl = config.GetSection("SolarConfiguration:solrCore").Value.TrimEnd('/');
            //_errorLog = errorLog;

            _indexFieldNames = false;
            _ = bool.TryParse(_config.GetSection("SolarConfiguration:IndexFieldNames").Value, out _indexFieldNames);
        }
        public async Task Index(EntityData entity, List<FormTemplate> forms)
        {
            SolrDoc doc = new SolrDoc(entity, forms, _indexFieldNames);
            await AddUpdateAsync(doc);
        }
        public async Task AddUpdateAsync(SolrDoc doc)
        {
            XElement payload = new XElement("add");
            payload.Add(doc.Root);

            await AddUpdateAsync(payload);
        }
        
        public async Task Index(IList<EntityData> entities, List<FormTemplate> forms)
        {
            var docs = entities.Select(entity => new SolrDoc(entity, forms, _indexFieldNames)).ToList();
            await Index(docs);
        }

        public async Task Index(List<SolrDoc> docs)
        {
            XElement payload = new XElement("add");
            foreach (var doc in docs)
                payload.Add(doc.Root);

            await AddUpdateAsync(payload); 
        }

        public async Task AddUpdateAsync(XElement payload)
        {
            await AddUpdateAsync(payload.ToString(SaveOptions.DisableFormatting));
        }

        public async Task AddUpdateAsync(string payloadXmlString)
        {
            var uri = new Uri(_solrCoreUrl + "/update?commit=true");

            using var content = new StringContent(payloadXmlString, Encoding.UTF8, "text/xml");
            using var httpResponse = await _httpClient.PostAsync(uri, content);

            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task CommitAsync()
        {
            var uri = new Uri(_solrCoreUrl + "/update?commit=true");

            using var httpResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();
        }

        
        public async Task<SearchResult> Search(string searchText, int start, int maxRows, int maxHighlightsPerEntry = 1)
        {
            string query = "doc_type_ss:item";

            if (!string.IsNullOrEmpty(searchText))
            {
                string[] fieldNames = GetFieldNames();
                List<string> queryParams = new List<string>();
                foreach (var name in fieldNames)
                    queryParams.Add(string.Format("{0}:\"{1}\"", name, searchText));

                query = string.Join(" OR ", queryParams);
                query = string.Format("({0}) AND doc_type_ss:item", query);
            }

            return await ExecuteSearch(query, start, maxRows, null, null, null, maxHighlightsPerEntry);
        }
       

        /// <summary>
        /// Executes a given valid solr query.
        /// </summary>Advanced search
        /// <param name="constraints"></param>
        /// <param name="start"></param>
        /// <param name="maxRows"></param>
        /// <param name="maxHighlightsPerEntry"></param>
        /// <returns></returns>
        public async Task<SearchResult> Search(SearchFieldConstraint[] constraints, int start, int maxRows, int maxHighlightsPerEntry = 1)
        {
            List<string> queryParams = new List<string>();
            foreach (var constraint in constraints)
            {
                string solrFieldType = "ts";
                var fieldName = string.Format("{0}_{1}_{2}_{3}",
                    SearchFieldConstraint.ScopeStr(constraint.Scope),
                    constraint.ContainerId,
                    constraint.FieldId,
                    solrFieldType);

                queryParams.Add(string.Format("{0}:\"{1}\"", fieldName, constraint.SearchText));
            }
            queryParams.Add("doc_type_ss:item");
            string query = string.Join(" AND ", queryParams);

            return await ExecuteSearch(query, start, maxRows, null, null, null, maxHighlightsPerEntry);
        }

        /// <summary>
        /// /Executes a given valid solr query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="start"></param>
        /// <param name="max"></param>
        /// <param name="maxHiglightSnippets"></param>
        /// <returns></returns>
        public async Task<SearchResult> ExecuteSearch(
            string query,
            int start,
            int max,
            string? filterQuery = null,
            string? sortBy = null,
            string? fieldList = null,
            int maxHiglightSnippets = 1,
            bool useSolrJson = false)
        {
            string qUrl = _solrCoreUrl + "/select?hl=on";
            var parameters = new Dictionary<string, string>();
            parameters["q"] = query;
            parameters["start"] = start.ToString();                                 
            parameters["rows"] = max.ToString();
            if (!string.IsNullOrEmpty(filterQuery)) parameters["fq"] = filterQuery;
            if (!string.IsNullOrEmpty(sortBy)) parameters["sort"] = sortBy;
            if (!string.IsNullOrEmpty(fieldList)) parameters["fl"] = fieldList;
            parameters["hl.fl"] = "*";
            parameters["hl.snippets"] = maxHiglightSnippets.ToString();
            parameters["wt"] = useSolrJson ? "json" : "xml";

            var postResponse = await _httpClient.PostAsync(new Uri(qUrl), new FormUrlEncodedContent(parameters));
            postResponse.EnsureSuccessStatusCode();
            var postContents = await postResponse.Content.ReadAsStringAsync();

            SearchResult result = new SearchResult();
            if (useSolrJson)
                result.InitFromJson(postContents);
            else
                result.InitFromXml(postContents);

            result.ItemsPerPage = max;
            return result;
        }

        protected string[] GetFieldNames(string[] acceptedFieldPrefixes = null)
        {
            string queryUri = _solrCoreUrl + "/select?q=*:*&wt=csv&rows=0&facet";
            if (acceptedFieldPrefixes == null)
                acceptedFieldPrefixes = new string[] { "data", "metadata" };

            //hl=on&q=apple&hl.fl=manu&fl=id,name,manu,cat
            string[] fieldNames = null;
            using (var task = _httpClient.GetAsync(new Uri(queryUri)))
            {
                task.Wait(60000);
                var task2 = task.Result.Content.ReadAsStringAsync();
                task2.Wait(60000);
                fieldNames = task2.Result
                    .Split(",")
                    .Where(f => acceptedFieldPrefixes.Contains(f.Split("_")[0]))
                    .ToArray();
            };
            return fieldNames;
        }

        public async Task SubmitSearchJobAsync(
            string query, 
            string? fieldList,
            string notificationEmail,
            string jobLabel,
            string solrCoreUrl,
            string downloadEndpoint,
            int batchSize,
            int maxRows,
            bool? selectUniqueEntries, 
            int? numDecimalPoints)
        {
            JobRecord jobRecord = new JobRecord()
            {
                JobLabel = jobLabel,
                Started = DateTime.Now,
                LastUpdated = DateTime.Now,
                Status = "In Progress",
                ExpectedDataRows = maxRows
            };
            _db.JobRecords.Add(jobRecord);

            try
            {
                string fileName = $@"{jobLabel.Replace(" ", "_").Trim()}_{Guid.NewGuid()}.csv";

                string folderRoot = Path.Combine("App_Data");
                if (!(Directory.Exists(folderRoot)))
                    Directory.CreateDirectory(folderRoot);
                string outFile = Path.Combine(folderRoot, fileName);
                if (File.Exists(outFile))
                    File.Delete(outFile);

                string downloadLink = downloadEndpoint + "?fileName=" + fileName;

                List<string> uniqueKeys = new List<string>();
                string[] fieldTypes = null;
                bool[] decimalFieldIndices = new bool[]{ };

                for (int offset = 0; offset < maxRows; offset += batchSize)
                {
                    var result = await ExecuteSolrSearch(solrCoreUrl, query, offset, batchSize, null, null, fieldList);

                    if(offset == 0)
                    {
                        fieldTypes = result.Substring(0, result.IndexOf("\n")).Split(new char[] { ',' });
                        decimalFieldIndices = fieldTypes.Select(ft => ft.EndsWith("_d")).ToArray();
                    }
                    
                    //Skipping the header line if this is not the first batch
                    result = result.Substring(result.IndexOf("\n") + 1);
                   
                    if(selectUniqueEntries.HasValue && selectUniqueEntries.Value)
                    {
                        List<string> selectedLines = new List<string>();
                        string[] lines = result.Split(new char[] {'\n'});
                        foreach(var line in lines)
                        {
                            string[] fieldValues = line.Split(new char[] {','});
                            string key = GetUniqueKey(fieldValues, numDecimalPoints, decimalFieldIndices);
                            if (!uniqueKeys.Contains(key))
                            {
                                uniqueKeys.Add(key);
                                selectedLines.Add(line);
                            }
                        }


                        if (offset == 0)
                            await File.AppendAllLinesAsync(outFile, new string[] { string.Join(",", fieldTypes!) });

                        if (selectedLines.Count > 0)
                            await File.AppendAllLinesAsync(outFile, selectedLines);


                        ////StringBuilder outputCsvBuilder = new StringBuilder(); ;
                        ////using (var writer = new StringWriter(outputCsvBuilder))
                        ////using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        ////{
                        ////    using (var reader = new StringReader(result))
                        ////    using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                        ////    {
                        ////        var rows = csvReader.GetRecords<string[]>().ToList();
                        ////        foreach(var row in rows)
                        ////        {
                        ////            string key = GetUniqueKey(row, numDecimalPoints, decimalFieldIndices);
                        ////            if (!uniqueKeys.Contains(key))
                        ////            {
                        ////                uniqueKeys.Add(key);
                        ////                csvWriter.WriteRecord(row);
                        ////            }
                        ////        }
                        ////    }
                        ////}

                        ////if (offset == 0)
                        ////    await File.AppendAllLinesAsync(outFile, new string[] { string.Join(",", fieldTypes!) });

                        ////await File.AppendAllTextAsync(outFile, outputCsvBuilder.ToString());
                    }
                    else
                    {
                        if (offset == 0)
                            await File.AppendAllLinesAsync(outFile, new string[] { string.Join(",", fieldTypes!) });

                        await File.AppendAllTextAsync(outFile, result);
                    }

                    jobRecord.ProcessedDataRows = offset + batchSize;
                    jobRecord.LastUpdated = DateTime.Now;
                    jobRecord.DataFileSize = new FileInfo(outFile).Length;
                    if (string.IsNullOrEmpty(jobRecord.DownloadLink))
                    {
                        jobRecord.DataFile = fileName;
                        jobRecord.DownloadLink = downloadLink;
                    }
                    _db.SaveChanges();
                }

                jobRecord.ProcessedDataRows = maxRows;
                jobRecord.Status = "Completed";
                jobRecord.LastUpdated = DateTime.Now;
                _db.SaveChanges();

                CatfishExtensions.DTO.Email emailDto = new CatfishExtensions.DTO.Email();
                emailDto.Subject = "Background Job Completed";
                emailDto.ToRecipientEmail = new List<string> { notificationEmail };
                emailDto.CcRecipientEmail = new List<string> { "arcrcg@ualberta.ca" };

                emailDto.Body = $@"Your background-job is done. You could download your data :<a href='{downloadLink}' target='_blank'> {fileName} </a>";
                _email.SendEmail(emailDto);
            }
            catch(Exception ex)
            {
                jobRecord.Status = "Failed";
                jobRecord.LastUpdated= DateTime.Now;
                _db.SaveChanges();

                CatfishExtensions.DTO.Email emailDto = new CatfishExtensions.DTO.Email();
                emailDto.Subject = "Background Job Failed";
                emailDto.ToRecipientEmail = new List<string> { notificationEmail };
                emailDto.CcRecipientEmail = new List<string> { "arcrcg@ualberta.ca" };

                emailDto.Body = $@"Your background-job failed. \n\n{ex.Message}\n\n{ex.StackTrace}";
                _email.SendEmail(emailDto);
            }

            ////var result = await ExecuteSolrSearch(solrCoreUrl, query, 0, int.MaxValue, null, null, fieldList);
            //////var result = task.Result;

            //////save the searchResult??
            ////string folderRoot = Path.Combine("App_Data");
            ////if (!(System.IO.Directory.Exists(folderRoot)))
            ////    System.IO.Directory.CreateDirectory(folderRoot);
            ////string outFile = Path.Combine(folderRoot, filename);
            ////if (System.IO.File.Exists(outFile))
            ////    System.IO.File.Delete(outFile);
            ////WriteToCsv(result, outFile);

        }

        private string GetUniqueKey(string[] fieldValues, int? numDecimalPoints, bool[] decimalFieldIndices)
        {
            if (!numDecimalPoints.HasValue)
                return string.Join(",", fieldValues);
            else
            {
                string key = "";
                for(int i=0; i< fieldValues.Length; ++i)
                {
                    if (decimalFieldIndices[i] && decimal.TryParse(fieldValues[i], out decimal decimalValue))
                        key = $"{key},{Math.Round(decimalValue, numDecimalPoints.Value)}";
                    else
                        key = $"{key},{fieldValues[i]}";
                }
                return key;
            }
        }

        public async Task<int> GetMatchCount(string query, string solrCoreUrl = "")
        {
            var result = await ExecuteSearch(query, 0, 1);
            return result.TotalMatches;
        }


        public async Task<string> ExecuteSolrSearch(string solrCoreUrl, string query, int start, int max, string? filterQuery = null, string? sortBy = null, string? fieldList = null, int maxHiglightSnippets = 1, string outputFormat = "csv")
        {
            string qUrl = solrCoreUrl + "/select?"; //"http://localhost:8983/solr/showtimes3/select?";// _solrCoreUrl + "/select?";// "/select?hl=on";
            var parameters = new Dictionary<string, string>();
            parameters["q"] = query;
            parameters["start"] = start.ToString();
            parameters["rows"] = max.ToString();
            if (!string.IsNullOrEmpty(filterQuery)) parameters["fq"] = filterQuery;
            if (!string.IsNullOrEmpty(sortBy)) parameters["sort"] = sortBy;
            if (!string.IsNullOrEmpty(fieldList)) parameters["fl"] = fieldList;
            parameters["hl.fl"] = "*";
            parameters["hl.snippets"] = maxHiglightSnippets.ToString();
            parameters["wt"] = outputFormat;

          
            
            Uri fullUri = new Uri(qUrl);
            var postResponse = await _httpClient.PostAsync(fullUri, new FormUrlEncodedContent(parameters));
            //var postResponse = await _httpClient.PostAsync(new Uri(qUrl), new FormUrlEncodedContent(parameters));
            postResponse.EnsureSuccessStatusCode();
            var postContents = await postResponse.Content.ReadAsStringAsync();

            return postContents;
        }
    }
}
