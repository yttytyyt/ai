using Models.DoorDir;
using Models.game;
using Models.ItemDir;
using Models.util;

namespace ConsoleIO
{
    public class IOConsole
    {
        #region type symbols and colors
        private static readonly Dictionary<int, ConsoleColor> enemyColors = new Dictionary<int, ConsoleColor>()
        {
            { 1, ConsoleColor.Red },
            { 2, ConsoleColor.Yellow },
            { 3, ConsoleColor.Green },
        };
        private static readonly Dictionary<Type, string> SymbolDictionary = new Dictionary<Type, string>()
        {
            { typeof(DisappearingBoobytrap), "@" },
            { typeof(Boobytrap), "O" },
            { typeof(SankaraStone), "S" },
            { typeof(Key), "K" },
            { typeof(PressurePlate), "T" },
            { typeof(Ice), "~" },
            { typeof(ToggleDoor), "_" },
            { typeof(ClosingGate), "n" },
            { typeof(ColoredDoor), "|" },
            { typeof(OpenOnOdd), "|" },
            { typeof(OpenOnStonesInRoom), "|" },
            { typeof(Door), " " },
        };
        private static readonly string PlayerSymbol = "X";
        private static readonly string LadderSymbol = "L";
        #endregion
        #region keyaction mapping
        private static readonly Dictionary<ConsoleKey, MovementAction> KeyActionMap = new Dictionary<ConsoleKey, MovementAction>()
        {
            { ConsoleKey.UpArrow, MovementAction.MoveNorth },
            { ConsoleKey.DownArrow, MovementAction.MoveSouth },
            { ConsoleKey.LeftArrow, MovementAction.MoveWest },
            { ConsoleKey.RightArrow, MovementAction.MoveEast },
            { ConsoleKey.Spacebar, MovementAction.DealDamage }
        };
        #endregion
        public void DrawGame(Game Game)
        {
            Console.Clear();
            Room displayedRoom = Game.Player.CurrentRoom;
            Position playerPosition = Game.Player.CurrentPosition;
            List<Item> totalItems = new List<Item>();
            totalItems.AddRange(displayedRoom.Items);
            totalItems.AddRange(displayedRoom.SpecialItems);
            RoomGraph RoomGraph = new RoomGraph(displayedRoom);

            RoomGraph.AddItemsToGraph(totalItems, SymbolDictionary);
            RoomGraph.AddEnemyToGraph(RoomGraph, displayedRoom.Enemies, enemyColors);
            RoomGraph.AddDoorsToGraph(Game, displayedRoom, SymbolDictionary);
            RoomGraph.AddLaddersToGraph(Game, displayedRoom, LadderSymbol);
            RoomGraph.AddPlayer(playerPosition, PlayerSymbol);


            WriteLine("Welcome to Temple of Doom!");
            WriteLine("Press any arrow key to move!");
            WriteLine("----------------------------");
            WriteLine("----------------------------");
            DrawRoomGraph(RoomGraph.Graph);
            WriteLine("");

            DisplayPlayerInventory(Game);
            if (Game.IsOver)
                WriteLine(Game.Player.Lives <= 0 ? "You have no more lives left... You've lost the game!" : "You've collected all the Sankara Stones! You've won!");
            else
                ReadConsole(Game);
        }

        #region draw room methods
        private void DisplayPlayerInventory(Game game)
        {
            var GroupedItems = game.Player.Items.GroupBy(item => item.GetType().Name)
                                .Select(group => new { ItemType = group.Key, Count = group.Count() });

            foreach (var Item in GroupedItems)
            {
                WriteLine($"{Item.Count}x Item of type {Item.ItemType}");
            }
            WriteLine($"Player Lives: {game.Player.Lives}");
        }

        private void DrawRoomGraph(List<List<VisualGameObject>> roomGraph)
        {
            for (int y = 0; y < roomGraph.Count; y++)
            {
                Console.Write("  ");
                for (int x = 0; x < roomGraph[y].Count; x++)
                {
                    var obj = roomGraph[y][x];
                    Console.ForegroundColor = obj.color;
                    Console.Write(obj.type + " ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
        #endregion
        #region read methods
        void ReadConsole(Game game)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            MovementAction action = MapKeyToAction(keyInfo.Key);
            game.Player.PerformAction(action);
            game.CanPlay();
            DrawGame(game);
        }

        private MovementAction MapKeyToAction(ConsoleKey key)
        {
            if (KeyActionMap.TryGetValue(key, out MovementAction action))
            {
                return action;
            }

            return MovementAction.None;
        }
        #endregion

        #region IO methods
        public void WriteLine(string String)
        {
            Console.WriteLine(String);
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }
        #endregion
    }
}