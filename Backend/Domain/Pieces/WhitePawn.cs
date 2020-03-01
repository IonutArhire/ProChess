using System;
using System.Collections.Generic;
using static Domain.Directions.Directions;

namespace Domain.Pieces
{
    [Serializable]
    public class WhitePawn : IPawn
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public IList<Transition> AvailableTransitions { get; set; }
        public bool IsTaken { get; set; }

        private void InitializeWhitePawn()
        {
            Name = "pawn";
            AvailableTransitions = new List<Transition>
            {
                new Transition(Up, true),
                new Transition(Up + Up, true),
                new Transition(Up + Right, true),
                new Transition(Up + Left, true)
            };
        }

        public WhitePawn(int x, int y)
        {
            Position = new Position(x, y);
            InitializeWhitePawn();
        }

        public WhitePawn(Position position)
        {
            Position = position;
            InitializeWhitePawn();
        }

        public Transition GetForwardOne()
        {
            return new Transition(Up, true);
        }

        public Transition GetForwardTwo()
        {
            return new Transition(Up + Up, true);
        }
        public (Transition, Transition) GetDiagonals()
        {
            var diagonalLeft  = new Transition(Up + Left, true);
            var diagonalRight = new Transition(Up + Right, true);

            return (diagonalLeft, diagonalRight);
        }

        public (Transition, Transition) GetSides()
        {
            var leftSide  = new Transition(Left, true);
            var rightSide = new Transition(Right, true);

            return (leftSide, rightSide);
        }
    }
}
