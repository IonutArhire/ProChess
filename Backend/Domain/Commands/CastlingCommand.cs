using System;
using System.Collections.Generic;
using System.Text;
using Domain.ClientModels;
using Domain.Pieces;

namespace Domain.Commands
{
    [Serializable]
    public class CastlingCommand : MoveCommand
    {
        public Rook MovedRook { get; set; }

        public CastlingCommand(Move move, King movedKing, Rook movedRook) 
            : base(move, movedKing)
        {
            MovedRook = movedRook;
        }
    }
}
