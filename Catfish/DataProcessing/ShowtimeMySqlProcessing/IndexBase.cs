using Catfish.Test.Helpers;
using CliWrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    public class IndexBase
    {
        protected readonly TestHelper _testHelper = new ();
        public delegate Task<bool> FileProcessingDelegate(string fileName, string? outputFolder = null, int? fileIndex = null, string? errorLogFile = null);


        protected async Task<bool> ExportTextFromInserts(string sqlFile, string modelName, string outFile)
        {
            int lineCount = 0;

            try
            {
                using (StreamReader sr = File.OpenText(sqlFile))
                {
                    using (StreamWriter writer = File.CreateText(outFile))
                    {
                        string line = string.Empty;
                        while ((line = sr.ReadLine()) != null)
                        {
                            ++lineCount;

                            if (!line.StartsWith($"INSERT INTO `{modelName}` VALUES "))
                                continue;

                            var valueStr = line.Substring(line.IndexOf("VALUES (") + 8);
                            valueStr = valueStr.TrimEnd(new char[] { ')', ';' });
                            string[] values = valueStr.Split("),(");

                            foreach (var val in values)
                                await writer.WriteLineAsync(val);
                        }

                        writer.Close();
                    }

                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Export Text Error on line {lineCount} in {sqlFile}\n", ex);
            }

            return true;
        }

        protected async Task IngestTextDataFile(string txtDataFile, string database)
        {
            string host = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:Host").Value;
            string user = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:User").Value;
            string password = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:Password").Value;
            string port = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:Port").Value;

            try
            {
                await Cli.Wrap("mysqlimport.exe")
                    .WithArguments(new[] {
                    "--protocol=tcp",
                    $"--host={host}",
                    $"--user={user}",
                    $"--password={password}",
                    $"--port={port}",
                    "--default-character-set=utf8",
                    "--fields-optionally-enclosed-by='",
                    "--fields-terminated-by=,",
                    "--lines-terminated-by=\\r\\n",
                    database,
                    txtDataFile
                    })
                    .ExecuteAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
