using System;
using System.Collections.Generic;
using static Domain.Directions.Directions;

namespace Domain.Pieces
{
    [Serializable]
    public class Bishop : IChessPiece
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public IList<Transition> AvailableTransitions { get; set; }
        public bool IsTaken { get; set; }

        private void InitializeBishop()
        {
            Name = "bishop";
            AvailableTransitions = new List<Transition>()
            {
                new Transition(Up + Right),
                new Transition(Down + Right),
                new Transition(Down + Left),
                new Transition(Up + Left)
            };
        }

        public Bishop(int x, int y)
        {
            Position = new Position(x, y);
            InitializeBishop();
        }

        public Bishop(Position position)
        {
            Position = position;
            InitializeBishop();
        }
    }
}
