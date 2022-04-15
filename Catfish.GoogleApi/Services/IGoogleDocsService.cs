using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public interface IGoogleDocsService : IDocsService
    {
        public void Init(UserCredential credential, string appName);
    }
}
