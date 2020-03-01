using System;

namespace Domain
{
    // TODO (priority 3): change X to Row and Y to Col
    [Serializable]
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position Clone()
        {
            return new Position(X, Y);
        }

        public override string ToString()
        {
            return $"{X}/{Y}";
        }

        public static Position operator +(Position a, Transition b)
        {
            return b != null ? new Position(a.X + b.X, a.Y + b.Y) : a;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Position;

            if (item == null)
            {
                return false;
            }

            return X.Equals(item.X) && Y.Equals(item.Y);
        }

        public override int GetHashCode()
        {
            return $"{X}_{Y}".GetHashCode();
        }
    }
}
