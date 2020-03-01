using System;
using System.Collections.Generic;
using System.Text;
using Domain.ClientModels;
using Domain.Pieces;

namespace Domain.Commands
{
    [Serializable]
    public class EnPassantCommand : MoveCommand
    {
        public EnPassantCommand(Move move, IChessPiece movedPiece, IChessPiece takenPiece = null) 
            : base(move, movedPiece, takenPiece)
        {
            Name = "EnPassant";
        }
    }
}
