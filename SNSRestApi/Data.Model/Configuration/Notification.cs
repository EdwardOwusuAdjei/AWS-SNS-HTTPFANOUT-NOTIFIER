using System.Collections.Generic;
using NUnit.Framework;

namespace SNSRestApi.Data.Model.Configuration
{
    public class Notification
    {
        /// <summary>
        /// Configuration detail for payload recipients 
        /// </summary>
        public List<string> MessageRecipients { get; set; }
        public List<string> EmailRecipients { get; set; }
        public string SmsEndpoint { get; set; }
    }
}