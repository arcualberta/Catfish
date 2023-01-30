using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// Creates or updates a zip file by adding the given data into a a new zip entry in the archive
        /// </summary>
        /// <param name="data">Data to be added to the zip entry.</param>
        /// <param name="zipEntryFileName">File name of the zip entry.</param>
        /// <param name="zipfile">Full path name of the zip file.</param>
        /// <param name="overwriteIfExists">Whether to overwrite or append, if the zip file already exists.</param>
        /// <param name="compressionLevel">The compression level</param>
        /// <returns></returns>
        public async static Task SaveToZipFile(string data, string zipEntryFileName, string zipfile, bool overwriteIfExists, CompressionLevel compressionLevel = CompressionLevel.Optimal)
            => await SaveToZipFile(new string[] { data }, zipEntryFileName, zipfile, overwriteIfExists, compressionLevel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">Data to be added to the zip entry. Each array element is added to a new line in the zip entry.</param>
        /// <param name="zipEntryFileName">File name of the zip entry.</param>
        /// <param name="zipfile">Full path name of the zip file.</param>
        /// <param name="overwriteIfExists">Whether to overwrite or append, if the zip file already exists.</param>
        /// <param name="compressionLevel">The compression level</param>
        /// <returns></returns>
        public async static Task SaveToZipFile(string[] data, string zipEntryFileName, string zipfile, bool overwriteIfExists, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            ZipArchiveMode mode = overwriteIfExists ? ZipArchiveMode.Create : ZipArchiveMode.Update;
            if (overwriteIfExists && File.Exists(zipfile))
                    File.Delete(zipfile);

            using (ZipArchive archive = ZipFile.Open(zipfile, mode))
            {
                ZipArchiveEntry entry = archive.CreateEntry(zipEntryFileName, compressionLevel);
                var output_stream = entry.Open();

                //Writing the data
                foreach (var d in data)
                    await output_stream.WriteAsync(new UTF8Encoding(true).GetBytes($"{d}{Environment.NewLine}"));

                //Closing the stream
                output_stream.Close();
            }
        }
    }
}
