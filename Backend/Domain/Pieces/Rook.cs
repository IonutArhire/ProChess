using System;
using System.Collections.Generic;
using static Domain.Directions.Directions;

namespace Domain.Pieces
{
    [Serializable]
    public class Rook : IChessPiece
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public IList<Transition> AvailableTransitions { get; set; }
        public bool IsTaken { get; set; }

        private void InitializeRook()
        {
            Name = "rook";
            AvailableTransitions = new List<Transition>()
            {
                new Transition(Up),
                new Transition(Right),
                new Transition(Down),
                new Transition(Left)
            };
        }

        public Rook(int x, int y)
        {
            Position = new Position(x, y);
            InitializeRook();
        }

        public Rook(Position position)
        {
            Position = position;
            InitializeRook();
        }
    }
}
