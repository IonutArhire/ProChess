using System;
using System.Collections.Generic;
using static Domain.Directions.Directions;

namespace Domain.Pieces
{
    [Serializable]
    public class Knight : IChessPiece
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public IList<Transition> AvailableTransitions { get; set; }
        public bool IsTaken { get; set; }

        private void InitializeKnight()
        {
            Name = "knight";
            AvailableTransitions = new List<Transition>
            {
                new Transition(Up + Up + Left, true),
                new Transition(Up + Up + Right, true),
                new Transition(Right + Right + Up, true),
                new Transition(Right + Right + Down, true),
                new Transition(Down + Down + Right, true),
                new Transition(Down + Down + Left, true),
                new Transition(Left + Left + Down, true),
                new Transition(Left + Left + Up, true),
            };
        }

        public Knight(int x, int y)
        {
            Position = new Position(x, y);
            InitializeKnight();
        }

        public Knight(Position position)
        {
            Position = position;
            InitializeKnight();
        }
    }
}
