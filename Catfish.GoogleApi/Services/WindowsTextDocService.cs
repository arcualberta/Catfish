using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public class WindowsTextDocService : IDocsService
    {
        public string CreateDoc(string parentFolderId, string docName, List<string> content)
        {
            var path = Path.Combine(parentFolderId, docName);
            File.WriteAllText(path, string.Join("\n", content));
            return path;
        }
    }
}
