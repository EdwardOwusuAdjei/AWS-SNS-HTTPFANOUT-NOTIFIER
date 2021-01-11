using System;
using SNSRestApi.Data.Model;

namespace SNSRestApi.Observer
{
    public interface IEventObserver:IObservable<SNSRequest>
    {
        void EventMessage(SNSRequest snsRequest);
    }
}