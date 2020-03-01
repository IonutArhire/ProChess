namespace Domain.Directions
{
    public class Direction
    {
        public int X { get; }
        public int Y { get; }

        public Direction(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Direction operator +(Direction a, Direction b)
        {
            return new Direction(a.X + b.X, a.Y + b.Y);
        }
    }
}
