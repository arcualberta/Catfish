using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public interface IDriveService
    {
        public string CreateFolder(string parentFolderId, string childFolderName);
        public string Clone(string srcId, string outputFolderId, string cloneName);
    }
}
