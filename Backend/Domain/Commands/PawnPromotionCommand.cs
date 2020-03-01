using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.ClientModels;
using Domain.Pieces;

namespace Domain.Commands
{
    [Serializable]
    public class PawnPromotionCommand : MoveCommand
    {
        public IList<IChessPiece> Pieces { get; set; }
        public string PromotionType { get; set; }
        public IChessPiece PromotedPiece { get; set; }

        public PawnPromotionCommand(IList<IChessPiece> pieces, Move move, IPawn movedPawn, IChessPiece takenPiece = null, string promotionType = null) 
            : base(move, movedPawn, takenPiece)
        {
            Name = "PawnPromotion";
            Pieces = pieces;
            PromotionType = promotionType;
        }

        public override void Do()
        {
            base.Do();
            MovedPiece.IsTaken = true;

            var promotedPiecePos = new Position(MovedPiece.Position.X, MovedPiece.Position.Y);

            switch (PromotionType)
            {
                case "queen":
                    PromotedPiece = new Queen(promotedPiecePos);
                    break;
                case "rook":
                    PromotedPiece = new Rook(promotedPiecePos);
                    break;
                case "bishop":
                    PromotedPiece = new Bishop(promotedPiecePos);
                    break;
                case "knight":
                    PromotedPiece = new Knight(promotedPiecePos);
                    break;
            }

            Pieces.Add(PromotedPiece);
        }

        public override void Undo()
        {
            Pieces.Remove(PromotedPiece);
            MovedPiece.IsTaken = false;
            base.Undo();
        }
    }
}
