using Models;
using Models.DoorDir;
using Models.ItemDir;


namespace ConsoleIO
{
    public class RoomGraph
    {
        public List<List<VisualGameObject>> Graph { get; set; }
        private readonly string Errorsymbol = "?";

        public RoomGraph(Room room)
        {
            Graph = new List<List<VisualGameObject>>();

            for (int y = 0; y < room.Height; y++)
            {
                var Row = new List<VisualGameObject>();
                for (int x = 0; x < room.Width; x++)
                {
                    Row.Add((x == 0 || x == room.Width - 1 || y == 0 || y == room.Height - 1) ? new VisualGameObject("#", ConsoleColor.Yellow) : new VisualGameObject(" ", ConsoleColor.White));
                }
                Graph.Add(Row);
            }
        }

        public void AddItemsToGraph(List<Models.ItemDir.Item> items, Dictionary<Type, string> symboldictionary)
        {
            foreach (var Item in items)
            {
                Position Pos = Item.CurrentPosition;
                Graph[Pos.CoordinateY][Pos.CoordinateX] = new VisualGameObject(GetItemSymbol(Item, symboldictionary), GetItemColor(Item));
            }
        }

        public void AddEnemyToGraph(RoomGraph roomGraph, List<EnemyAdapter> enemies, Dictionary<int, ConsoleColor> enemyColors)
        {
            foreach (var enemy in enemies)
            {
                Position Pos = enemy.CurrentPosition;
                if (enemyColors.ContainsKey(enemy.Lives))
                    Graph[Pos.CoordinateY][Pos.CoordinateX] = new VisualGameObject("E", enemyColors[enemy.Lives]);
            }
        }

        public void AddPlayer(Position playerPosition, string playerSymbol)
        {
            Graph[playerPosition.CoordinateY][playerPosition.CoordinateX] = new VisualGameObject(playerSymbol, ConsoleColor.White);
        }

        public void AddDoorsToGraph(Game game, Room room, Dictionary<Type, string> symboldictionary)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Connection connection = game.GetConnection(direction, room);
                if (connection != null)
                {
                    List<DoorInteractable> doors = connection.GetDoors();
                    Position doorPosition = room.GetDoorPosition(direction);
                    string doorSymbol = GetDoorSymbol(doors.First(), direction, symboldictionary);
                    ConsoleColor doorColor = ColorToConsoleColor(((ColoredDoor)doors.FirstOrDefault(d => d is ColoredDoor))?.Color ?? null);
                    VisualGameObject visualDoor = new VisualGameObject(doorSymbol, doorColor);
                Graph[doorPosition.CoordinateY][doorPosition.CoordinateX] = visualDoor;
                }
            }
        }

        public void AddLaddersToGraph(Game game, Room room, string ladderSymbol)
        {
            List<Connection> ladderConnections = game.GetLadderConnections(room);
            foreach (var connection in ladderConnections)
            {
                Position ladderCoordinate = connection.Ladder.GetPosition(room);
                Graph[ladderCoordinate.CoordinateY][ladderCoordinate.CoordinateX] = new VisualGameObject(ladderSymbol, ConsoleColor.White);
            }
        }



        #region symbol methods
        private string GetItemSymbol(Item item, Dictionary<Type, string> symboldictionary)
        {
            if (symboldictionary.TryGetValue(item.GetType(), out string symbol))
            {
                return symbol;
            }

            return Errorsymbol;
        }

        private string GetDoorSymbol(DoorInteractable door, Direction direction, Dictionary<Type, string> symboldictionary)
        {
            if (symboldictionary.TryGetValue(door.GetType(), out string symbol))
            {
                if (door is ColoredDoor || door is OpenOnOdd || door is OpenOnStonesInRoom)
                {
                    return (direction == Direction.EAST || direction == Direction.WEST) ? "|" : "=";
                }
                return symbol;
            }

            return Errorsymbol;
        }
        #endregion

        #region color methods
        private ConsoleColor ColorToConsoleColor(Color? Obj)
        {
            switch (Obj)
            {
                case Color.RED:
                    return ConsoleColor.DarkRed;
                case Color.GREEN:
                    return ConsoleColor.DarkGreen;
                default:
                    return ConsoleColor.White;
            }
        }

        private ConsoleColor GetItemColor(Item item)
        {
            switch (item)
            {
                case SankaraStone:
                    return ConsoleColor.DarkYellow;
                case Key:
                    return ColorToConsoleColor(((Key)item).Color);
                default:
                    return Console.ForegroundColor;
            }
        }
        #endregion
    }
}
