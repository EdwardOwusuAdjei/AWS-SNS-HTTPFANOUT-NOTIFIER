using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SNSRestApi.Data.Model;
using SNSRestApi.Data.Model.Configuration;
using SNSRestApi.DependencyResolver;

namespace SNSRestApi.Service.SMSChannel
{
    public class SendMessage:ISendMessage
    {
        private readonly IOptions<Notification> _options;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SendMessage> _logger;
        public SendMessage(HttpClient httpClient, ILogger<SendMessage> logger)
        {
            var options = ServiceDependency.InternalServiceProvider
                .GetService<IOptions<Notification>>();
            _options = options;
            _httpClient = httpClient;
            _logger = logger;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="snsRequest"></param>
        /// <returns></returns>
        public async Task<bool> Send(SNSRequest snsRequest)
        {
            foreach (var number in _options.Value.MessageRecipients)
            {
                var url = new StringBuilder(_options.Value.SmsEndpoint)
                    .Replace("$to",$"{number}")
                    .Replace("$content",$"{snsRequest.Message}");
                var response =
                    await _httpClient.GetAsync(url.ToString());
                if(response.IsSuccessStatusCode)
                    _logger.LogInformation($"sent message to {number} with {snsRequest.Message}");
                else
                {
                    _logger.LogInformation($"could not send message to {number} with {snsRequest.Message}");
                }
                    
            }
            

            return true;
        }
    }
}