using Models.DoorDir;
using Models.ItemDir;
using Models.util;

namespace Models.Convert
{
    public class DoorFactory
    {
        public Models.DoorInteractable CreateDoor(DataLayer.Door[] doors, DataLayer.Connection c, DataToGameConverter converter)
        {
            DoorInteractable lastDoor = new Models.DoorDir.Door();
            foreach (var door in doors)
            {
                switch (door.type)
                {
                    case "open on odd":
                        lastDoor = new OpenOnOdd(lastDoor);
                        break;
                    case "colored":
                        lastDoor = new ColoredDoor(lastDoor, (Color)Enum.Parse(typeof(Color), door.color.ToUpper()));
                        break;
                    case "toggle":
                        var toggleDoor = new ToggleDoor(lastDoor);
                        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                        {
                            int roomId = GetRoomId(c, direction);
                            if (roomId != 0)
                            {
                                converter.AddDoorToRoom(toggleDoor, roomId);
                            }
                        }

                        lastDoor = toggleDoor;
                        break;
                    case "closing gate":
                        lastDoor = new ClosingGate(lastDoor);
                        break;
                    case "open on stones in room":
                        lastDoor = new OpenOnStonesInRoom(lastDoor, door.no_of_stones);
                        break;
                    default:
                        throw new Exception("Door type not found");
                }
            }

            return lastDoor;
        }

        private int GetRoomId(DataLayer.Connection c, Direction direction)
        {
            switch (direction)
            {
                case Direction.NORTH:
                    return c.NORTH;
                case Direction.SOUTH:
                    return c.SOUTH;
                case Direction.WEST:
                    return c.WEST;
                case Direction.EAST:
                    return c.EAST;
                default:
                    throw new ArgumentException("Invalid direction");
            }
        }
    }
}
