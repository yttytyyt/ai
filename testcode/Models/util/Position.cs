namespace Models.util
{
    public class Position
    {
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }

        public Position(int x, int y)
        {
            CoordinateX = x;
            CoordinateY = y;
        }

        public bool CompareTo(Position position)
        {
            if (position != null && position.CoordinateX == CoordinateX && position.CoordinateY == CoordinateY)
            {
                return true;
            }
            return false;
        }
        public bool CompareTo(int x, int y)
        {
            if (x == CoordinateX && y == CoordinateY)
            {
                return true;
            }
            return false;
        }

        public Position GetNeighbour(Direction direction)
        {
            switch (direction)
            {
                case Direction.NORTH:
                    return new Position(CoordinateX, CoordinateY - 1);
                case Direction.EAST:
                    return new Position(CoordinateX + 1, CoordinateY);
                case Direction.SOUTH:
                    return new Position(CoordinateX, CoordinateY + 1);
                case Direction.WEST:
                    return new Position(CoordinateX - 1, CoordinateY);
                    throw new ArgumentException("Invalid direction");
            }
            return null;
        }

        public List<Position> GetNeighbours()
        {
            return new List<Position>()
            {
                GetNeighbour(Direction.NORTH),
                GetNeighbour(Direction.EAST),
                GetNeighbour(Direction.SOUTH),
                GetNeighbour(Direction.WEST)
            };
        }


        // Override Equals and GetHashCode to make sure Posisitons can be compared and found in a dictionary.
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Position P = (Position)obj;
            return CoordinateX == P.CoordinateX && CoordinateY == P.CoordinateY;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int Hash = 17;
                Hash = Hash * 23 + CoordinateX.GetHashCode();
                Hash = Hash * 23 + CoordinateY.GetHashCode();
                return Hash;
            }
        }


    }
}