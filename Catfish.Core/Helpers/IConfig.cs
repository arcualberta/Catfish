using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Helpers
{
    public interface IConfig
    {
        string GetSmtpServer();
        string GetSmtpEmail();
    }
    public class ReadConfiguration : IConfig
    {
        private readonly IConfiguration _configuration;
        public ReadConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetSmtpEmail()
        {
            return _configuration["EmailServer:Email"];
        }

        public string GetSmtpServer()
        {
            return _configuration["EmailServer:SmtpServer"];
        }
    }
}
