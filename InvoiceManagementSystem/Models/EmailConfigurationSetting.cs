using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class EmailConfigurationSetting
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromMail { get; set; }
        public int Port { get; set; }
    }
}