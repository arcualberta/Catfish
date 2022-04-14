using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public interface IDocsService
    {
        public string CreateDoc(string parentFolderId, string docName, List<string> content);
    }
}
