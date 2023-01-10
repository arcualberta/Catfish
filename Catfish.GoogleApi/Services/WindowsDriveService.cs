using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public class WindowsDriveService : IDriveService
    {
        public string Clone(string srcId, string outputFolderId, string cloneName)
        {
            string path = Path.Combine(outputFolderId, cloneName);
            File.Copy(srcId, path);
            return path;
        }

        public string CreateFolder(string parentFolderId, string childFolderName)
        {
            string path = Path.Combine(parentFolderId, childFolderName);
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
