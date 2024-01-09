using DataLayer;
using Models.ItemDir;
using System.Linq;

namespace Models
{
    public class Game
    {
        public Player Player { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Connection> Connections { get; set; }
        public int WinningStones { get; set; }
        public bool IsOver { get; set; }
        public string Path { get; set; }

        public void End()
        {
            IsOver = true;
        }

        public Connection? GetConnection(Direction direction, Room room)
        {
            return room == null ? throw new ArgumentNullException(nameof(room)) :
                   Connections.FirstOrDefault(c => c.connections.TryGetValue(direction, out var connectedRoom) &&
                                                  connectedRoom?.Id == room.Id);
        }
        public List<Connection> GetLadderConnections(Room room)
        {
            return Connections.Where(c => c.Ladder != null && c.Ladder.ContainsRoom(room)).ToList();
        }


        public Room? CanEnterConnection(Direction direction)
        {
            Connection? connection = GetConnection(direction.GetOppositeDirection(), Player.CurrentRoom);
            if (connection != null && connection.Interact(Player))
                return connection.GetRoom(direction);
            return null;
        }

        public void CanPlay()
        {
            if (Player.Lives <= 0 || Player.Items.Count(i => i is SankaraStone) == WinningStones )
                End();
        }

        internal void SetLadderPosition(Room room)
        {
            if (Connections.Where(c => c.Ladder != null && c.Ladder.ContainsRoom(room)).ToList() is List<Connection> connections)
                foreach(Connection conn in connections)
                {
                    conn.Ladder.SetPosition(room);

                }
        }
    }
}
