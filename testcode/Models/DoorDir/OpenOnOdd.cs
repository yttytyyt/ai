using Models.entities;

namespace Models.DoorDir
{
    public class OpenOnOdd : DoorDecorator
    {
        public OpenOnOdd(DoorInteractable component) : base(component)
        {
        }

        public override bool Interact(Player player)
        {
            if (base.Interact(player) && AddedBehavior(player))
            {
                return true;
            }
            return false;
        }

        private bool AddedBehavior(Player player)
        {
            if (player.Lives % 2 != 0)
            {
                return true;
            }
            return false;

        }
    }
}
