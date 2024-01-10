namespace Models.util
{
    public enum Direction
    {
        NORTH, SOUTH, EAST, WEST
    }

    public static class DirectionExtension
    {
        public static Direction GetOppositeDirection(this Direction direction)
        {
            switch (direction)
            {
                case Direction.NORTH: return Direction.SOUTH;
                case Direction.SOUTH: return Direction.NORTH;
                case Direction.EAST: return Direction.WEST;
                case Direction.WEST: return Direction.EAST;
                default: throw new ArgumentException("Invalid direction");
            }
        }
    }
}
