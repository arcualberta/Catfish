using Catfish.API.Repository.DTOs;
using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Models.BackgroundJobs;
using Catfish.API.Repository.Solr;
using CatfishExtensions.DTO;
using CatfishExtensions.Services;
using CsvHelper;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using System;

namespace Catfish.API.Repository.Services
{
    public class SolrService : ISolrService
    {
        protected readonly IConfiguration _config;
        private readonly string _solrCoreUrl;
        private readonly string _solrDocUploadApi;
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
            if (!string.IsNullOrEmpty(config.GetSection("SolarConfiguration:SolrDocUploadApi")?.Value))
                _solrDocUploadApi = config.GetSection("SolarConfiguration:SolrDocUploadApi").Value.TrimEnd('/');
            //_errorLog = errorLog;

            _indexFieldNames = false;
            _ = bool.TryParse(_config.GetSection("SolarConfiguration:IndexFieldNames").Value, out _indexFieldNames);

            
        }

        public void SetHttpClientTimeoutSeconds(int seconds)
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(seconds);
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

        public string GetPayloadString(List<SolrDoc> docs)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<add>");
            foreach (var doc in docs)
                sb.Append(doc.Root.ToString(SaveOptions.DisableFormatting));
            sb.Append("</add>");

            return sb.ToString();
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
            //await File.WriteAllTextAsync("C:\\codebase\\docs.xml", payloadXmlString);
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

        public async Task UploadIndexingSolrDocs(List<SolrDoc> docs, string? basicAuthenticationCredentials)
        {
            string payloadXmlString = GetPayloadString(docs).Replace("\"", "\\\"");

            HttpResponseMessage httpResponse;
            if (!string.IsNullOrEmpty(basicAuthenticationCredentials))
            {
                // Set up basic authentication credentials
                var authValue = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(basicAuthenticationCredentials))
                );

                _httpClient.DefaultRequestHeaders.Authorization = authValue;

                var content = new StringContent($"\"{payloadXmlString}\"", Encoding.UTF8, "application/json");

                httpResponse = await _httpClient.PostAsync(_solrDocUploadApi, content);
            }
            else
            {
                var uri = new Uri(_solrDocUploadApi);
                using var content = new StringContent(payloadXmlString, Encoding.UTF8, "text/plain");
                httpResponse = await _httpClient.PostAsync(uri, content);
            }

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
            string? notificationEmail,
            Guid jobRecordId,
            string solrCoreUrl,
            string downloadEndpoint,
            int batchSize,
            bool? selectUniqueEntries,
            int? numDecimalPoints,
            string? frequencyArrayFields,
            string? exportFields)
        {
            JobRecord jobRecord = await GetJobRecord(jobRecordId);
            ++jobRecord.AttemptCount;
            jobRecord.Status = "In Progress";

            if (string.IsNullOrEmpty(jobRecord.DataFile))
            {
                jobRecord.DataFile = $@"{jobRecord.JobLabel.Replace(" ", "_").Trim()}_{Guid.NewGuid()}.csv";
                jobRecord.DownloadDataFileLink = downloadEndpoint + "?fileName=" + jobRecord.DataFile;
            }

            jobRecord.LastUpdated = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            try
            {
                string folderRoot = Path.Combine("App_Data");
                if (!(Directory.Exists(folderRoot)))
                    Directory.CreateDirectory(folderRoot);

                string outFile = Path.Combine(folderRoot, jobRecord.DataFile);
                string keysFile = outFile.Substring(0, outFile.LastIndexOf(".")) + "-keys.txt";

                List<string> uniqueKeys = new List<string>();
                if (jobRecord.AttemptCount > 1)
                {
                    //Loading previously found unique keys
                    if (File.Exists(keysFile))
                        uniqueKeys = (await File.ReadAllLinesAsync(keysFile)).ToList();
                }
                else
                    uniqueKeys = new List<string>();

                string[] frequencyArrayFieldList = string.IsNullOrEmpty(frequencyArrayFields) ? new string[0] : frequencyArrayFields.Split(',');
                bool[] freqFieldFlagsWithRespectToFullFieldList = new bool[] { };

                string[] unorderedExportFieldList = string.IsNullOrEmpty(exportFields) ? new string[0] : exportFields.Split(','); ;
                string[] exportFieldList = null;
                bool[] exportFieldFlagsWithRespectToFullFieldList = new bool[] { };

                Regex csvSplitRegx = new Regex("," + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                string[] fullFieldNameList = null;
                bool[] decimalFieldFlagsWithRespectToExportFieldList = new bool[] { };

                BackgroundProcessingStats stats = null;

                for (int offset = jobRecord.Offset; offset < jobRecord.ExpectedDataRows; offset += batchSize)
                {
                    var result = await ExecuteSolrSearch(solrCoreUrl, query, offset, batchSize, null, null, fieldList);

                    if (offset == jobRecord.Offset)
                    {
                        //First row in the result set returned by solr search represents the list of field names.
                        fullFieldNameList = result.Substring(0, result.IndexOf("\n")).Split(',');

                        //If no export field list have been provided as an input argument, we should export all fields.
                        if (unorderedExportFieldList.Length == 0)
                            exportFieldList = fullFieldNameList;
                        else
                            exportFieldList = fullFieldNameList.Where(fieldName => unorderedExportFieldList.Contains(fieldName)).ToArray();


                        exportFieldFlagsWithRespectToFullFieldList = fullFieldNameList.Select(fieldName => exportFieldList.Contains(fieldName)).ToArray();

                        decimalFieldFlagsWithRespectToExportFieldList = exportFieldList.Select(fieldName => fieldName.EndsWith("_d")).ToArray();

                        freqFieldFlagsWithRespectToFullFieldList = fullFieldNameList.Select(fieldName => frequencyArrayFieldList.Contains(fieldName)).ToArray();
                    }

                    //Skipping the header line
                    result = result.Substring(result.IndexOf("\n") + 1);

                    if (selectUniqueEntries.HasValue && selectUniqueEntries.Value)
                    {
                        if (stats == null)
                        {
                            stats = new BackgroundProcessingStats();
                            string statsFile = jobRecord.GetStatsFileName();
                            jobRecord.DownloadStatsFileLink = downloadEndpoint + "?fileName=" + statsFile;

                            if (jobRecord.AttemptCount > 1)
                            {
                                //Loading stats generated by the previous attempt
                                string statsOutFile = Path.Combine(folderRoot, jobRecord.GetStatsFileName());
                                if (File.Exists(statsOutFile))
                                {
                                    string[] statsLines = await File.ReadAllLinesAsync(statsOutFile);
                                    for(int statsLineIndex = 1; statsLineIndex < statsLines.Length; ++statsLineIndex)
                                    {
                                        string[] parts = statsLines[statsLineIndex].Split(',');
                                        stats.Frequencies.Add(int.Parse(parts[0]));
                                        if (statsLineIndex == 1)
                                        {
                                            stats.UniqueRecordCount= int.Parse(parts[1]);
                                            stats.TotalCount= int.Parse(parts[2]);
                                        }
                                    }
                                }
                            }
                        }

                        List<string> selectedLines = new List<string>();
                        List<string> newUnqueKeys = new List<string>();

                        string[] lines = result.Split(new char[] { '\n' });
                        foreach (var line in lines)
                        {
                            if (string.IsNullOrEmpty(line))
                                continue;

                            ++jobRecord.Offset;
                            ++jobRecord.ProcessedDataRows;

                            //Full list of field values represented in the result row.
                            string[] fieldValues = csvSplitRegx.Split(line);

                            //Extract the list of values that needs to be exported from the full field value list
                            string[] exportFieldValues = fieldValues.Where((str, index) => exportFieldFlagsWithRespectToFullFieldList[index]).ToArray();

                            //Calculate the frequency increment of the selected data row. If one or more frequency-array-fields have been
                            //specified, set the cumulative element count of those arrays as the frequency increment. Otherwise, set the 
                            //frequency increment to be 1.
                            int freqIncrement = 0;
                            if (!freqFieldFlagsWithRespectToFullFieldList.Where(x => x).Any())
                                freqIncrement = 1;
                            else
                            {
                                var freqArrayValues = fieldValues.Where((str, index) => freqFieldFlagsWithRespectToFullFieldList[index]).ToList();
                                foreach (var concatenatedArrayValue in freqArrayValues)
                                {
                                    freqIncrement += concatenatedArrayValue.Split(',').Select(s => s.Trim('"')).Where(s => s.Length > 0).Count();
                                }
                            }

                            //Calculate the unique key that represent the combination of export field values
                            string key = CalculateUniqueKey(exportFieldValues, numDecimalPoints, decimalFieldFlagsWithRespectToExportFieldList);
                            int idx = uniqueKeys.IndexOf(key);
                            if (idx < 0)
                            {
                                uniqueKeys.Add(key);
                                newUnqueKeys.Add(key);
                                selectedLines.Add(string.Join(',', exportFieldValues));
                                stats.Frequencies.Add(freqIncrement);
                                ++stats.UniqueRecordCount;
                            }
                            else
                            {
                                stats.Frequencies[idx] += freqIncrement;
                            }
                            stats.TotalCount += freqIncrement;
                        }


                        //Adding the names of the selected columns to the top of the output file
                        if (offset == 0)
                            await File.AppendAllLinesAsync(outFile, new string[] { string.Join(",", exportFieldList) });

                        if (selectedLines.Count > 0)
                        {
                            await File.AppendAllLinesAsync(outFile, selectedLines);
                            await File.AppendAllLinesAsync(keysFile, newUnqueKeys);
                            await SaveStatsFile(stats, jobRecord, folderRoot);
                            jobRecord.DataFileSize = new FileInfo(outFile).Length;
                        }
                        
                        jobRecord.LastUpdated = DateTime.UtcNow;
                        await _db.SaveChangesAsync();

                    }
                    else
                    {
                        //Adding the names of the selected columns to the top of the output file
                        if (offset == 0)
                            await File.AppendAllLinesAsync(outFile, new string[] { string.Join(",", fullFieldNameList!) });

                        await File.AppendAllTextAsync(outFile, result);

                        int numRecordsProcessed = result.Count(x => x == '\n');
                        jobRecord.Offset += numRecordsProcessed;
                        jobRecord.ProcessedDataRows += numRecordsProcessed;
                        jobRecord.LastUpdated = DateTime.UtcNow;
                        jobRecord.DataFileSize = new FileInfo(outFile).Length;

                        await _db.SaveChangesAsync();
                    }

                    
                }

                //////Saving the frequencies file.
                ////if (stats != null)
                ////{
                ////    string statsFile = jobRecord.DataFile.Substring(0, jobRecord.DataFile.Length - 4) + "-stats.csv";
                ////    string statsOutFile = Path.Combine(folderRoot, statsFile);

                ////    await File.WriteAllTextAsync(statsOutFile, "Record Frequencies,Unique Record Count,Total Record Count\n");
                ////    await File.AppendAllTextAsync(statsOutFile, $"{stats.Frequencies[0]},{stats.UniqueRecordCount},{stats.TotalCount}\n");
                ////    await File.AppendAllLinesAsync(statsOutFile, stats.Frequencies.GetRange(1, stats.Frequencies.Count - 1).Select(x => x.ToString()));

                ////    jobRecord.DownloadStatsFileLink = downloadEndpoint + "?fileName=" + statsFile; ;
                ////}

                jobRecord.ProcessedDataRows = jobRecord.ExpectedDataRows;
                jobRecord.Status = "Completed";
                jobRecord.LastUpdated = DateTime.UtcNow;
                jobRecord.Message = $"Processing time: {(jobRecord.LastUpdated - jobRecord.Started)}";
                await _db.SaveChangesAsync();
                if (!string.IsNullOrEmpty(notificationEmail))
                {
                    Email emailDto = new Email();
                    emailDto.Subject = "Background Job Completed";
                    emailDto.ToRecipientEmail = new List<string> { notificationEmail };
                    // emailDto.CcRecipientEmail = new List<string> { "arcrcg@ualberta.ca" };

                    emailDto.Body = $@"Your background-job <b>{jobRecord.JobLabel}</b> is done. You could download your data :<a href='{jobRecord.DownloadDataFileLink}' target='_blank'> {jobRecord.DataFile} </a>";
                    _email.SendEmail(emailDto);
                }
            }
            catch (Exception ex)
            {
                jobRecord.Status = "Failed";
                jobRecord.LastUpdated = DateTime.UtcNow;
                jobRecord.Message = $"{ex.Message}\n\n{ex.StackTrace}";
                await _db.SaveChangesAsync();

                if (!string.IsNullOrEmpty(notificationEmail))
                {
                    Email emailDto = new Email();
                    emailDto.Subject = "Background Job Failed";
                    emailDto.ToRecipientEmail = new List<string> { notificationEmail };
                    // emailDto.CcRecipientEmail = new List<string> { "arcrcg@ualberta.ca" };

                    emailDto.Body = $@"Sorry, your background-job <b>{jobRecord.JobLabel}</b> failed.";
                    _email.SendEmail(emailDto);
                }
            }

        }

        private async Task SaveStatsFile(BackgroundProcessingStats? stats, JobRecord jobRecord, string folderRoot)
        {
            if (stats != null)
            {
                string statsOutFile = Path.Combine(folderRoot, jobRecord.GetStatsFileName());

                await File.WriteAllTextAsync(statsOutFile, "Record Frequencies,Unique Record Count,Total Record Count\n");
                await File.AppendAllTextAsync(statsOutFile, $"{stats.Frequencies[0]},{stats.UniqueRecordCount},{stats.TotalCount}\n");
                await File.AppendAllLinesAsync(statsOutFile, stats.Frequencies.GetRange(1, stats.Frequencies.Count - 1).Select(x => x.ToString()));
            }
        }

        private string CalculateUniqueKey(string[] fieldValues, int? numDecimalPoints, bool[] decimalFieldIndices)
        {
            try
            {
                if (!numDecimalPoints.HasValue)
                    return string.Join(",", fieldValues);
                else
                {
                    string key = "";
                    for (int i = 0; i < fieldValues.Length; ++i)
                    {
                        if (decimalFieldIndices[i] && decimal.TryParse(fieldValues[i], out decimal decimalValue))
                            key = $"{key},{Math.Round(decimalValue, numDecimalPoints.Value)}";
                        else
                            key = $"{key},{fieldValues[i]}";
                    }
                    return key;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private T[] GetSubArray<T>(T[] source, bool[] selectIndicies) =>
            selectIndicies?.Length > 0 ? source.Where((val, index) => selectIndicies[index]).ToArray() : source;

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
        public async Task<JobRecord?> GetJobRecord(Guid jobId)
        {
            return await _db.JobRecords.FindAsync(jobId);
        }
        public async Task<JobRecord> CreateJobRecord(string label, int maxRow, string? user)
        {
            try
            {
                JobRecord jobRecord = new JobRecord()
                {
                    JobLabel = label,
                    Started = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow,
                    Status = "Pending",
                    ExpectedDataRows = maxRow,
                    User = user
                };

                _db.JobRecords.Add(jobRecord);
                await _db.SaveChangesAsync();

                return jobRecord;

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        public async Task UpdateJobRecordHangfireId(Guid jobId, string hangfireId)
        {
            try
            {
                JobRecord jRecord = await GetJobRecord(jobId);

                if (jRecord == null)
                    throw new Exception("Object not found");

                jRecord.JobId = hangfireId;

                _db.Entry(jRecord).State = EntityState.Modified;

                await _db.SaveChangesAsync();

                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
