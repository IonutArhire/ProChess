using System;
using Domain.Directions;

namespace Domain
{
    [Serializable]
    public class Transition
    {
        public int X { get; }
        public int Y { get; }
        public bool IsOneTimeOnly { get; }

        public Transition(Direction direction, bool isOneTimeOnly = false)
        {
            X = direction.X;
            Y = direction.Y;
            IsOneTimeOnly = isOneTimeOnly;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Transition;

            if (item == null)
            {
                return false;
            }

            return X.Equals(item.X) && Y.Equals(item.Y) && IsOneTimeOnly.Equals(item.IsOneTimeOnly);
        }

        public override int GetHashCode()
        {
            return $"{X}_{Y}_{IsOneTimeOnly}".GetHashCode();
        }
    }
}
