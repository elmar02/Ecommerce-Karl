﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration.Abstract
{
    public interface IEmailConfiguration
    {
        string Name { get; }
        string Email { get; }
        string Password { get; }
        string SmtpServer { get; }
        int Port { get; }
    }
}
