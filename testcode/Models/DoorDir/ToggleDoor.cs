using Models.entities;
using Models.util;

namespace Models.DoorDir
{
    public class ToggleDoor : DoorDecorator, IObserver
    {
        public ToggleDoor(DoorInteractable component) : base(component)
        {
            IsOpen = false;
        }
        public override bool Interact(Player player)
        {
            if (base.Interact(player) && AddedBehavior())
            {
                return true;
            }
            return false;
        }

        public bool AddedBehavior()
        {
            return IsOpen;
        }

        public void Update()
        {
            IsOpen = !IsOpen;
        }
    }
}
