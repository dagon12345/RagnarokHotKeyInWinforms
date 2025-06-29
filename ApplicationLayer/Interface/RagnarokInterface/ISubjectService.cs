using ApplicationLayer.Utilities;

namespace ApplicationLayer.Interface.RagnarokInterface
{
    public interface ISubjectService
    {
        void Attach(IObserverService observer);
        void Detach(IObserverService observer);
        void Notify(Message message);
    }
}
