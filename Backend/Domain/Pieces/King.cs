using System;
using System.Collections.Generic;
using static Domain.Directions.Directions;

namespace Domain.Pieces
{
    [Serializable]
    public class King : IChessPiece
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public IList<Transition> AvailableTransitions { get; set; }
        public bool IsTaken { get; set; }

        private void InitializeKing()
        {
            Name = "king";
            AvailableTransitions = new List<Transition>
            {
                new Transition(Up, true),
                new Transition(Up + Right, true),
                new Transition(Right, true),
                new Transition(Down + Right, true),
                new Transition(Down, true),
                new Transition(Down + Left, true),
                new Transition(Left, true),
                new Transition(Up + Left, true)
            };
        }

        public King(int x, int y)
        {
            Position = new Position(x, y);
            InitializeKing();
        }

        public King(Position position)
        {
            Position = position;
            InitializeKing();
        }
    }
}
