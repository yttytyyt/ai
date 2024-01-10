using Models.entities;

namespace Models.DoorDir
{
    public class Door : DoorInteractable
    {
        public string state { get; set; }

        public Door()
        {
            base.Component = null;
        }

        public override bool Interact(Player player)
        {
            return true;
        }
    }
}
