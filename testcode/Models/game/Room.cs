using CODE_TempleOfDoom_DownloadableContent;
using Models.entities;
using Models.ItemDir;
using Models.util;
using System.Xml.Linq;

namespace Models.game
{
    public class Room
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Item> Items { get; set; }
        public List<Item> SpecialItems { get; set; }
        public List<EnemyAdapter> Enemies { get; set; }


        public bool IsValidMove(Position position)
        {
            bool isWithinBounds = position.CoordinateX > 0 && position.CoordinateX < Width - 1 &&
                                  position.CoordinateY > 0 && position.CoordinateY < Height - 1;

            return isWithinBounds;
        }


        public bool IsAtConnection(Position position)
        {
            int middleOfHeight = Height / 2;
            int middleOfWidth = Width / 2;

            bool isAtMiddleOfWall = position.CoordinateY == 0 && position.CoordinateX == middleOfWidth ||
                                    position.CoordinateY == Height - 1 && position.CoordinateX == middleOfWidth ||
                                    position.CoordinateY == middleOfHeight && position.CoordinateX == 0 ||
                                    position.CoordinateY == middleOfHeight && position.CoordinateX == Width - 1;

            return isAtMiddleOfWall;
        }

        public Position GetStartPosition(Direction direction)
        {
            int x, y;

            switch (direction)
            {
                case Direction.NORTH:
                    x = Width / 2;
                    y = 0;
                    break;
                case Direction.SOUTH:
                    x = Width / 2;
                    y = Height - 1;
                    break;
                case Direction.EAST:
                    x = Width - 1;
                    y = Height / 2;
                    break;
                case Direction.WEST:
                    x = 0;
                    y = Height / 2;
                    break;
                default:
                    throw new ArgumentException("Invalid direction");
            }

            return new Position(x, y);
        }
        #region overridden hashmap methods
        public override int GetHashCode()
        {
            if (Id == null) return 0;
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Room other = obj as Room;
            return other != null && other.Id == Id;
        }

        public Position GetDoorPosition(Direction direction)
        {
            int wallCenterX = Width / 2;
            int wallCenterY = Height / 2;

            return direction switch
            {
                Direction.NORTH => new Position(wallCenterX, Height - 1),
                Direction.SOUTH => new Position(wallCenterX, 0),
                Direction.EAST => new Position(0, wallCenterY),
                Direction.WEST => new Position(Width - 1, wallCenterY),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Invalid direction: {direction}")
            };
        }
        #endregion
    }
}
