using Models.entities;
using Models.ItemDir;
using Models.util;

namespace Models.game
{
    public class Connection
    {
        public Dictionary<Direction, Room> connections { get; set; }
        public Ladder? Ladder { get; set; }
        public DoorInteractable Door { get; set; }

        public List<DoorInteractable> GetDoors()
        {
            List<DoorInteractable> doors = new List<DoorInteractable>();
            for (var door = Door; door != null; door = door.Component)
            {
                doors.Add(door);
            }
            return doors.OrderByDescending(door => door.GetDoorPriority()).ToList();
        }

        public Room GetRoom(Direction direction)
        {
            return connections[direction];
        }

        public bool Interact(Player player)
        {
            if (Door != null)
                return Door.Interact(player);

            return true;
        }


    }
}
