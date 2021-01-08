using System.Threading.Tasks;
using SNSRestApi.Data.Model;

namespace SNSRestApi.Service
{
    public interface IReceiver
    {
        /// <summary>
        /// Accept SNS Payload
        /// </summary>
        /// <param name="payload">SNSRequest from AWS</param>
        /// <returns></returns>
        Task Accept(SNSRequest payload);
        /// <summary>
        /// Start Reader for channel and set workers.
        /// </summary>
        /// <returns></returns>
        Task<bool> StartReader();
    }
}