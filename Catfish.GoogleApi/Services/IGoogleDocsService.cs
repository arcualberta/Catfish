using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public interface IGoogleDocsService
    {
        public void Init(UserCredential credential, string appName);
        public Google.Apis.Docs.v1.Data.Document CreateDoc(string parentFolderId, string docName, List<string> content);
    }
}
