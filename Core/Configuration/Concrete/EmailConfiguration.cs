﻿using Core.Configuration.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration.Concrete
{
    public class EmailConfiguration : IEmailConfiguration
    {
        private readonly IConfiguration _configuration;

        public EmailConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Name => _configuration["EmailSettings:Name"];
        public string Email => _configuration["EmailSettings:Email"];
        public string Password => _configuration["EmailSettings:Password"];
        public string SmtpServer => _configuration["EmailSettings:SmtpServer"];
        public int Port => Convert.ToInt32(_configuration["EmailSettings:Port"]);
    }
}
