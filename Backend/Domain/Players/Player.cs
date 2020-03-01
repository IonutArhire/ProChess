using System.Collections.Generic;
using Domain.Pieces;

namespace Domain.Players
{
    public class Player
    {
        public IList<IChessPiece> Pieces { get; set; }

        public PlayerColor Color { get; }
        public int Wins { get; set; }

        public Player(PlayerColor color)
        {
            Color = color;

            Wins = 0;
            Pieces = new List<IChessPiece>();
        }
    }
}
