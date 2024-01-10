using Models.DoorDir;
using Models.entities;
using Models.util;

namespace Models.ItemDir
{
    public class PressurePlate : Item, IObservable
    {
        private List<IObserver> _Observers = new List<IObserver>();

        public override void Interact(IEntity entity)
        {
            if (entity is Player)
            {
                Notify();
            }
        }

        public void Attach(IObserver observer)
        {
            _Observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _Observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (IObserver observer in _Observers)
            {
                observer.Update();
            }
        }
    }
}
