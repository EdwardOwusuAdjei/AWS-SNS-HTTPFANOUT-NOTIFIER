using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
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
        private IDisposable _unsubscribe;
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
       

        public void OnCompleted()
        {
            _logger.LogInformation($"sent messages");
        }

        public void OnError(Exception error)
        {
            _logger.LogInformation($"error occurred messages {error.Message}");
        }

        public async void OnNext(SNSRequest snsRequest)
        {
            foreach (var number in _options.Value.MessageRecipients)
            {
                var url = new StringBuilder(_options.Value.SmsEndpoint)
                    .Replace("$to",$"{number.Trim()}")
                    .Replace("$content",$"{snsRequest.Subject.Trim()} - {snsRequest.Message.Trim()}");
                _logger.LogInformation(url.ToString());
                var response =
                    await _httpClient.GetAsync(url.ToString());
                if(response.IsSuccessStatusCode)
                    _logger.LogInformation($"sent message to {number} with {snsRequest.Message}");
                else
                {
                    _logger.LogInformation($"could not send message to {number} with {snsRequest.Message}");
                }
                    
            }
        }

        public void Subscribe(IObservable<SNSRequest> provider)
        {
            if (provider != null)
                _unsubscribe = provider.Subscribe(this);
        }
    }
}