using System;
using System.Collections.Generic;
using static Domain.Directions.Directions;

namespace Domain.Pieces
{
    [Serializable]
    public class Queen : IChessPiece
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public IList<Transition> AvailableTransitions { get; set; }
        public bool IsTaken { get; set; }

        private void InitializeQueen()
        {
            Name = "queen";
            AvailableTransitions = new List<Transition>
            {
                new Transition(Up),
                new Transition(Up + Right),
                new Transition(Right),
                new Transition(Down + Right),
                new Transition(Down),
                new Transition(Down + Left),
                new Transition(Left),
                new Transition(Up + Left),
            };
        }

        public Queen(int x, int y)
        {
            Position = new Position(x, y);
            InitializeQueen();
        }

        public Queen(Position position)
        {
            Position = position;
            InitializeQueen();
        }
    }
}
