using System.Collections.Generic;

namespace Domain.Pieces
{
    public interface IChessPiece
    {
        string Name { get; set; }
        Position Position { get; set; }
        IList<Transition> AvailableTransitions { get; }
        bool IsTaken { get; set; }
    }
}
