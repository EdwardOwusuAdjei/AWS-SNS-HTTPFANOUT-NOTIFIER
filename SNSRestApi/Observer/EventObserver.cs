using System;
using System.Collections.Generic;
using SNSRestApi.Data.Model;

namespace SNSRestApi.Observer
{
    public class EventObserver:IEventObserver
    {
        //list of observers
        private List<IObserver<SNSRequest>> _observers;

        public EventObserver()
        {
            _observers = new List<IObserver<SNSRequest>>();
        }
        
        public IDisposable Subscribe(IObserver<SNSRequest> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }
        // Define Unsubscriber class
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<SNSRequest>> _observers;
            private IObserver<SNSRequest> _observer;

            public Unsubscriber(List<IObserver<SNSRequest>> observers,
                IObserver<SNSRequest> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null) _observers.Remove(_observer);
            }
        }


        // Notify observers when event occurs
        public void EventMessage(SNSRequest snsRequest)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(snsRequest);
            }
        }
    }
}