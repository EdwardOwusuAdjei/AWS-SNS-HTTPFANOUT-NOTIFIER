using System.Threading.Tasks;
using SNSRestApi.Data.Model;

namespace SNSRestApi.Service.SMSChannel
{
    public interface ISendMessage
    {
        /// <summary>
        /// Send Text Message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        Task<bool> Send(SNSRequest snsRequest);
    }
}