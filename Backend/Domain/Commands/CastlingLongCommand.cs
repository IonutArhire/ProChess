using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Domain.ClientModels;
using Domain.Pieces;

namespace Domain.Commands
{
    [Serializable]
    public class CastlingLongCommand : CastlingCommand
    {
        public CastlingLongCommand(Move move, King movedKing, Rook movedRook)
            : base(move, movedKing, movedRook)
        {
            Name = "CastlingLong";
        }

        public override void Do()
        {
            base.Do();
            MovedRook.Position.Y = MovedPiece.Position.Y + 1;
        }

        public override void Undo()
        {
            base.Undo();
            MovedRook.Position.Y = GameConstants.RookLeftStartColumn;
        }
    }
}
