using ApplicationLayer.Interface.RagnarokInterface;
using ApplicationLayer.Utilities;
using System;
using System.Collections.Generic;

namespace ApplicationLayer.Service.RagnarokService
{
    public class SubjectService : ISubjectService
    {
        public Message Message { get; set; } = new Message();
        private List<IObserverService> _observers = new List<IObserverService>();

        public void Attach(IObserverService observer)
        {
            Console.WriteLine("Subject: Attached an observer.");
            this._observers.Add(observer);
        }

        public void Detach(IObserverService observer)
        {
            this._observers.Remove(observer);
            Console.WriteLine("Subject: Detached an observer.");
        }

        public void Notify(Message message)
        {
            Console.WriteLine("Subject: Notifying observers...");
            this.Message = message;
            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }
    }
}
