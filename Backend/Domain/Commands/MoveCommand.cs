using System;
using Domain.ClientModels;
using Domain.Pieces;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Domain.Commands
{
    [Serializable]
    public class MoveCommand
    {
        public IChessPiece MovedPiece { get; }
        public IChessPiece TakenPiece { get; }
        public Move Move { get; }
        public string Name { get; set; }

        public MoveCommand(Move move, IChessPiece movedPiece, IChessPiece takenPiece = null)
        {
            MovedPiece = movedPiece;
            TakenPiece = takenPiece;

            Move = move;
        }

        public virtual void Do()
        {
            MovedPiece.Position = Move.To;

            if (TakenPiece != null)
            {
                TakenPiece.IsTaken = true;
            }
        }

        public virtual void Undo()
        {
            MovedPiece.Position = Move.From;

            if (TakenPiece != null)
            {
                TakenPiece.IsTaken = false;
            }
        }
    }
}
