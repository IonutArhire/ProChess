using System;
using System.Collections.Generic;
using static Domain.Directions.Directions;

namespace Domain.Pieces
{
    /*
    The black pieces are always on the upper side of the board and so
    moving "forward" means moving downwards relative to the board.
     */
    [Serializable]
    public class BlackPawn : IPawn
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public IList<Transition> AvailableTransitions { get; set; }
        public bool IsTaken { get; set; }

        private void InitializeBlackPawn()
        {
            Name = "pawn";
            AvailableTransitions = new List<Transition>
            {
                new Transition(Down, true),
                new Transition(Down + Down, true),
                new Transition(Down + Right, true),
                new Transition(Down + Left, true)
            };
        }

        public BlackPawn(int x, int y)
        {
            Position = new Position(x, y);
            InitializeBlackPawn();
        }

        public BlackPawn(Position position)
        {
            Position = position;
            InitializeBlackPawn();
        }

        public Transition GetForwardOne()
        {
            return new Transition(Down, true);
        }

        public Transition GetForwardTwo()
        {
            return new Transition(Down + Down, true);
        }

        public (Transition, Transition) GetDiagonals()
        {
            var diagonalLeft  = new Transition(Down + Right, true);
            var diagonalRight = new Transition(Down + Left, true);

            return (diagonalLeft, diagonalRight);
        }

        public (Transition, Transition) GetSides()
        {
            var leftSide = new Transition(Right, true);
            var rightSide = new Transition(Left, true);

            return (leftSide, rightSide);
        }
    }
}
