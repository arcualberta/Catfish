using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public interface IGoogleDriveService : IDriveService
    {
        public void Init(UserCredential credential, string appName);
    }
}
