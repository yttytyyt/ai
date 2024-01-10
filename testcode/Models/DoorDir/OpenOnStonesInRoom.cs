using Models.entities;
using Models.game;
using Models.ItemDir;

namespace Models.DoorDir
{
    public class OpenOnStonesInRoom : DoorDecorator
    {
        public int No_of_stones { get; set; }

        public OpenOnStonesInRoom(DoorInteractable component, int numberOfStones) : base(component)
        {
            No_of_stones = numberOfStones;
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
            Room currentRoom = player.CurrentRoom;

            int stonesInRoom = 0;
            foreach (var item in currentRoom.Items)
            {
                if (item is SankaraStone)
                {
                    stonesInRoom++;
                }
            }
            return stonesInRoom == No_of_stones;
        }
    }
}
