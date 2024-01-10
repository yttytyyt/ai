using CODE_TempleOfDoom_DownloadableContent;
using DataLayer;
using Models.DoorDir;
using Models.entities;
using Models.game;
using Models.ItemDir;
using Models.util;

namespace Models.Convert
{
    public class DataToGameConverter
    {
        private Dictionary<int, List<ToggleDoor>> RoomWithDoors = new Dictionary<int, List<ToggleDoor>>();

        public Game CreateGame(GameData gameData)
        {

            var itemFactory = new ItemFactory();
            var doorFactory = new DoorFactory();

            var currRoomItems = gameData.rooms.SingleOrDefault(r => r.id == gameData.player.startRoomId)?.items?.ToList() ?? new List<DataLayer.Item>();
            var dataRooms = gameData.rooms.Select(room => CreateRoom(room)).ToList();
            var roomDictionary = dataRooms.ToDictionary(r => r.Id, r => r);
            var startRoom = gameData.rooms.SingleOrDefault(r => r.id == gameData.player.startRoomId);
            var initialPlayerRoom = CreateRoom(startRoom);

            var initialPosition = new Position(gameData.player.startX, gameData.player.startY);

            var player = new Models.entities.Player()
            {
                CurrentRoom = initialPlayerRoom,
                CurrentPosition = initialPosition,
                Lives = gameData.player.lives,
                Items = currRoomItems.Select(i => itemFactory.CreateItem(i)).ToList(),
                CurrentDirection = Direction.NORTH
            };

            var connections = gameData.connections.Select(c => new Models.game.Connection()
            {
                connections = new Dictionary<Direction, game.Room>
                {
                    { Direction.NORTH, roomDictionary.GetValueOrDefault(c.NORTH) },
                    { Direction.SOUTH, roomDictionary.GetValueOrDefault(c.SOUTH) },
                    { Direction.WEST, roomDictionary.GetValueOrDefault(c.WEST) },
                    { Direction.EAST, roomDictionary.GetValueOrDefault(c.EAST) }
                },
                Door = doorFactory.CreateDoor(c.doors, c, this),
                Ladder = c.ladder != null ? CreateLadder(c.ladder, (dataRooms.SingleOrDefault(r => r.Id == c.UPPER)), (dataRooms.SingleOrDefault(r => r.Id == c.LOWER))) : null
            }).ToList();
            Game game = new Game()
            {
                Player = player,
                Rooms = dataRooms,
                Connections = connections,
                WinningStones = 5,
            };
            game.Player.Game = game;
            game.SetLadderPosition(player.CurrentRoom);

            foreach (var room in game.Rooms)
            {
                if (RoomWithDoors.TryGetValue(room.Id, out var toggleDoors))
                {
                    foreach (var pressurePlate in room.Items.OfType<PressurePlate>())
                    {
                        toggleDoors.ForEach(toggleDoor => pressurePlate.Attach(toggleDoor));
                    }
                }
            }


            return game;
        }

        private ItemDir.Ladder CreateLadder(DataLayer.Ladder ladder, Models.game.Room upperRoom, Models.game.Room lowerRoom)
        {

            return new ItemDir.Ladder()
            {
                LadderPosition = new Dictionary<Models.game.Room, Position>()
                {
                    { upperRoom, new Position(ladder.upperX, ladder.upperY) },
                    { lowerRoom, new Position(ladder.lowerX, ladder.lowerY) }
                },
            };
        }

        private game.Room CreateRoom(DataLayer.Room room)
        {
            var itemFactory = new ItemFactory();
            var enemyFactory = new EnemyFactory();
            var items = room.items?.Select(i => itemFactory.CreateItem(i)).ToList() ?? new List<Models.ItemDir.Item>();
            var specialItems = room.specialFloorTiles?.Select(i => itemFactory.CreateItem(i)).ToList() ?? new List<Models.ItemDir.Item>();

            var newRoom = new Models.game.Room()
            {
                Id = room.id,
                Height = room.height,
                Width = room.width,
                Type = room.type,
                Items = items,
                SpecialItems = specialItems,
            };

            newRoom.Enemies = room.enemies?.Select(e => new EnemyAdapter(enemyFactory.CreateEnemy(e), newRoom)).ToList() ?? new List<EnemyAdapter>();

            return newRoom;
        }



        public void AddDoorToRoom(ToggleDoor door, int roomId)
        {
            if (!RoomWithDoors.ContainsKey(roomId))
            {
                RoomWithDoors[roomId] = new List<ToggleDoor>();
            }
            RoomWithDoors[roomId].Add(door);
        }

    }

    public class EnemyFactory
    {


        public CODE_TempleOfDoom_DownloadableContent.Enemy CreateEnemy(DataLayer.Enemy enemy)
        {
            switch (enemy.type)
            {
                case "horizontal":
                    return new HorizontallyMovingEnemy(3, enemy.x, enemy.y, enemy.minX, enemy.maxX); //TODO: magic number
                case "vertical":
                    return new VerticallyMovingEnemy(3, enemy.x, enemy.y, enemy.minY, enemy.maxY); //TODO: magic number
                default:
                    throw new ArgumentException("Invalid type");
            }
        }
    }
}
