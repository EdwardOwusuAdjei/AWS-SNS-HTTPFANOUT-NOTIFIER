using System;
using System.Threading.Tasks;
using SNSRestApi.Data.Model;
using SNSRestApi.Observer;

namespace SNSRestApi.Service.SMSChannel
{
    public interface ISendMessage:IObserver<SNSRequest>
    {
        /// <summary>
        /// Send Text Message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        void Subscribe(IObservable<SNSRequest> provider);
    }
}