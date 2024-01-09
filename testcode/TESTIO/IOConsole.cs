using Models;
using Models.DoorDir;
using Models.ItemDir;

namespace ConsoleIO
{
    public class VisualGameObject
    {
        public VisualGameObject(string type, ConsoleColor consoleColor)
        {
            this.type = type;
            this.color = consoleColor;
        }

        public string type { get; set; }
        public ConsoleColor color { get; set; }
    }

    public class IOConsole
    {
        private static readonly Dictionary<int, ConsoleColor> enemyColors = new Dictionary<int, ConsoleColor>()
        {
            { 1, ConsoleColor.Red },
            { 2, ConsoleColor.Yellow },
            { 3, ConsoleColor.Green },
        };
        private Room DisplayedRoom { get; set; }
        public void DrawGame(Game Game)
        {
            Console.Clear();
            DisplayedRoom = Game.Player.CurrentRoom;
            Position playerPosition = Game.Player.CurrentPosition;
            List<Item> totalItems = new List<Item>();
            totalItems.AddRange(DisplayedRoom.Items);
            totalItems.AddRange(DisplayedRoom.SpecialItems);
            List<List<VisualGameObject>> RoomGraph = CreateRoomGraph();

            AddItemsToGraph(RoomGraph, totalItems);
            AddEnemyToGraph(RoomGraph, DisplayedRoom.Enemies);
            AddConnectionsToGraph(Game, RoomGraph);
            RoomGraph[playerPosition.CoordinateY][playerPosition.CoordinateX] = new VisualGameObject("X", ConsoleColor.White);


            WriteLine("Welcome to Temple of Doom!");
            WriteLine("Press any arrow key to move!");
            WriteLine("----------------------------");
            WriteLine("----------------------------");
            DrawRoomGraph(RoomGraph);
            WriteLine("");

            DisplayPlayerInventory(Game);
            if (Game.IsOver)
            {
                WriteLine(Game.Player.Lives <= 0 ? "You have no more lives left... You've lost the game!" : "You've collected all the Sankara Stones! You've won!");
            }
            else
            {
                ReadConsole(Game);
            }
        }

        private List<List<VisualGameObject>> CreateRoomGraph()
        {
            var RoomGraph = new List<List<VisualGameObject>>();
            for (int y = 0; y < DisplayedRoom.Height; y++)
            {
                var Row = new List<VisualGameObject>();
                for (int x = 0; x < DisplayedRoom.Width; x++)
                {
                    Row.Add((x == 0 || x == DisplayedRoom.Width - 1 || y == 0 || y == DisplayedRoom.Height - 1) ? new VisualGameObject("#", ConsoleColor.Yellow) : new VisualGameObject(" ", ConsoleColor.White));
                }
                RoomGraph.Add(Row);
            }
            return RoomGraph;
        }

        private void AddItemsToGraph(List<List<VisualGameObject>> RoomGraph, List<Item> Items)
        {
            foreach (var Item in Items)
            {
                Position Pos = Item.CurrentPosition;
                RoomGraph[Pos.CoordinateY][Pos.CoordinateX] = new VisualGameObject(GetItemSymbol(Item), GetItemColor(Item));
            }
        }

        private void AddEnemyToGraph(List<List<VisualGameObject>> RoomGraph, List<EnemyAdapter> Enemies)
        {
            foreach (var enemy in Enemies)
            {
                Position Pos = enemy.CurrentPosition;
                if (enemyColors.ContainsKey(enemy.Lives))
                    RoomGraph[Pos.CoordinateY][Pos.CoordinateX] = new VisualGameObject("E", enemyColors[enemy.Lives]);
            }
        }

        #region working section
        // Presentation Layer
        private void AddConnectionsToGraph(Game game, List<List<VisualGameObject>> roomGraph)
        {
            AddDoorsToGraph(game, roomGraph); // Presentation Logic
            AddLaddersToGraph(game, roomGraph); // Presentation Logic
        }

        // Presentation Layer
        private void AddDoorsToGraph(Game game, List<List<VisualGameObject>> roomGraph)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Connection connection = game.GetConnection(direction, DisplayedRoom); // Domain Logic
                if (connection != null)
                {
                    List<DoorInteractable> doors = connection.GetDoors(); // Domain Logic
                    Position doorPosition = DisplayedRoom.GetDoorPosition(direction);
                    roomGraph[doorPosition.CoordinateY][doorPosition.CoordinateX] = CreateDoorVisualGameObject(doors, direction); // Presentation Logic
                }
            }
        }


        // Presentation Layer
        private VisualGameObject CreateDoorVisualGameObject(List<DoorInteractable> doors, Direction direction)
        {
            return new VisualGameObject(GetDoorSymbol(doors.First(), direction), ColorToConsoleColor(((ColoredDoor)doors.FirstOrDefault(d => d is ColoredDoor))?.Color ?? null)); // Presentation Logic
        }

        // Presentation Layer
        private void AddLaddersToGraph(Game game, List<List<VisualGameObject>> roomGraph)
        {
            List<Connection> ladderConnections = game.GetLadderConnections(DisplayedRoom);
            foreach (var connection in ladderConnections)
            {
                Position ladderCoordinate = connection.Ladder.GetPosition(DisplayedRoom); // Domain Logic
                roomGraph[ladderCoordinate.CoordinateY][ladderCoordinate.CoordinateX] = new VisualGameObject("L", ConsoleColor.White); // Presentation Logic
            }
        }


        #endregion

        private void DisplayPlayerInventory(Game Game)
        {
            var GroupedItems = Game.Player.Items.GroupBy(item => item.GetType().Name)
                                .Select(group => new { ItemType = group.Key, Count = group.Count() });

            foreach (var Item in GroupedItems)
            {
                WriteLine($"{Item.Count}x Item of type {Item.ItemType}");
            }
            WriteLine($"Player Lives: {Game.Player.Lives}");
        }

        // Presentation Layer
        void ReadConsole(Game game)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            MovementAction action = MapKeyToAction(keyInfo.Key); // Map the key press to an action
            game.Player.PerformAction(action); // Perform the action
            game.CanPlay();
            DrawGame(game);
        }

        private MovementAction MapKeyToAction(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    return MovementAction.MoveNorth;
                case ConsoleKey.DownArrow:
                    return MovementAction.MoveSouth;
                case ConsoleKey.LeftArrow:
                    return MovementAction.MoveWest;
                case ConsoleKey.RightArrow:
                    return MovementAction.MoveEast;
                case ConsoleKey.Spacebar:
                    return MovementAction.DealDamage;
                default:
                    return MovementAction.None; // You might want to add a 'None' action to handle invalid keys
            }
        }


        #region symbol methods
        private string GetItemSymbol(Item Item)
        {
            switch (Item)
            {
                case DisappearingBoobytrap _:
                    return "@";
                case Boobytrap _:
                    return "O";
                case SankaraStone _:
                    return "S";
                case Key _:
                    return "K";
                case PressurePlate _:
                    return "T";
                case Ice _:
                    return "~";
                default:
                    return "?";
            }
        }

        private string GetDoorSymbol(DoorInteractable door, Direction Direction)
        {
            switch (door)
            {
                case ToggleDoor _:
                    return "_";
                case ClosingGate _:
                    return "n";
                case ColoredDoor _:
                case OpenOnOdd _:
                case OpenOnStonesInRoom _: return (Direction == Direction.EAST || Direction == Direction.WEST) ? "|" : "=";
                case Door:
                    return " ";
                default:
                    return "?";
            }
        }
#endregion

        #region color methods
        private ConsoleColor ColorToConsoleColor(Color? Obj) //works with the ConsoleColor, should it still be in here or the Color enum?
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

        private ConsoleColor GetItemColor(Item item) //again save color in model when its purely for visualization?
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
        private void DrawRoomGraph(List<List<VisualGameObject>> RoomGraph)
        {
            for (int y = 0; y < RoomGraph.Count; y++)
            {
                Console.Write("  ");
                for (int x = 0; x < RoomGraph[y].Count; x++)
                {
                    var obj = RoomGraph[y][x];
                    Console.ForegroundColor = obj.color;
                    Console.Write(obj.type + " ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
        #endregion

        public void WriteLine(string String)
        {
            Console.WriteLine(String);
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }
    }
}