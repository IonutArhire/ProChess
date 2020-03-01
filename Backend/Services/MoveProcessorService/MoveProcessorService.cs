using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using Domain.ClientModels;
using Domain.Commands;
using Domain.Directions;
using Domain.Pieces;
using Domain.Players;
using Services.GameValidationService;

namespace Services.MoveProcessorService
{
    public class MoveProcessorService : IMoveProcessorService
    {
        private IPawn GetTakenPawnEnPassant(GameModel game, Move move, Position diagonal, Position side)
        {
            if (diagonal.Equals(move.To) &&
                game.WaitingPlayer.Pieces.PieceAtPosition(diagonal) == null)
            {
                var piece = game.WaitingPlayer.Pieces.PieceAtPosition(side);
                if (piece is IPawn pawnToTake)
                {
                    return pawnToTake;
                }
            }

            return null;
        }

        private EnPassantCommand ProcessEnPassant(GameModel game, Move move)
        {
            var movedPiece = game.MovingPlayer.Pieces.PieceAtPosition(move.From);

            if (movedPiece is IPawn movedPawn)
            {
                var diagonals = movedPawn.GetDiagonals();
                var diagonalLeftPos = movedPawn.Position + diagonals.Item1;
                var diagonalRightPos = movedPawn.Position + diagonals.Item2;

                var sides = movedPawn.GetSides();
                var leftSidePos = movedPawn.Position + sides.Item1;
                var rightSidePos = movedPawn.Position + sides.Item2;

                IPawn takenPawn;

                takenPawn = GetTakenPawnEnPassant(game, move, diagonalLeftPos, leftSidePos);

                if (takenPawn != null)
                {
                    return new EnPassantCommand(move, movedPawn, takenPawn);
                }

                takenPawn = GetTakenPawnEnPassant(game, move, diagonalRightPos, rightSidePos);

                if (takenPawn != null)
                {
                    return new EnPassantCommand(move, movedPawn, takenPawn);
                }
            }

            return null;
        }

        // TODO (priority 3): see if you can reduce the number of parameters
        private CastlingCommand GetCastlingCommand(GameModel game, Move move, 
                                                       Position kingStartingPos, 
                                                       Position rookStartingPos, 
                                                       Direction castlingDirection)
        {
            if (!move.From.Equals(kingStartingPos) ||
                !(game.MovingPlayer.Pieces.PieceAtPosition(move.From) is King king))
            {
                return null;
            }

            if (!move.To.Equals(kingStartingPos + new Transition(castlingDirection + castlingDirection)))
            {
                return null;
            }

            if (!(game.MovingPlayer.Pieces.PieceAtPosition(rookStartingPos) is Rook rook))
            {
                return null;
            }

            if (castlingDirection == Directions.Left)
            {
                return new CastlingLongCommand(move, king, rook);
            }

            return new CastlingShortCommand(move, king, rook);
        }

        private CastlingLongCommand ProcessCastlingLong(GameModel game, Move move)
        {
            Position kingStartingPos;
            Position rookStartingPos;

            if (game.MovingPlayer.Color == PlayerColor.White)
            {
                kingStartingPos = new Position(GameConstants.BackRowWhite, GameConstants.KingStartColumn);
                rookStartingPos = new Position(GameConstants.BackRowWhite, GameConstants.RookLeftStartColumn);
            }
            else
            {
                kingStartingPos = new Position(GameConstants.BackRowBlack, GameConstants.KingStartColumn);
                rookStartingPos = new Position(GameConstants.BackRowBlack, GameConstants.RookLeftStartColumn);
            }

            return GetCastlingCommand(game, move, kingStartingPos, rookStartingPos, Directions.Left) as CastlingLongCommand;
        }

        // TODO (priority 3): see if you can reduce the duplicate code
        private CastlingShortCommand ProcessCastlingShort(GameModel game, Move move)
        {
            Position kingStartingPos;
            Position rookStartingPos;

            if (game.MovingPlayer.Color == PlayerColor.White)
            {
                kingStartingPos = new Position(GameConstants.BackRowWhite, GameConstants.KingStartColumn);
                rookStartingPos = new Position(GameConstants.BackRowWhite, GameConstants.RookRightStartColumn);
            }
            else
            {
                kingStartingPos = new Position(GameConstants.BackRowBlack, GameConstants.KingStartColumn);
                rookStartingPos = new Position(GameConstants.BackRowBlack, GameConstants.RookRightStartColumn);
            }

            return GetCastlingCommand(game, move, kingStartingPos, rookStartingPos, Directions.Right) as CastlingShortCommand;
        }

        private PawnPromotionCommand ProcessPawnPromotion(GameModel game, Move move)
        {
            var movedPiece = game.MovingPlayer.Pieces.PieceAtPosition(move.From);
            if (movedPiece is IPawn)
            {
                switch (game.MovingPlayer.Color)
                {
                    case PlayerColor.White when move.To.X == GameConstants.BackRowBlack:
                    case PlayerColor.Black when move.To.X == GameConstants.BackRowWhite:
                    {
                        var takenPiece = game.WaitingPlayer.Pieces.PieceAtPosition(move.To);
                        return new PawnPromotionCommand(game.MovingPlayer.Pieces, move, (IPawn)movedPiece, takenPiece);
                    }
                }
            }

            return null;
        }

        public MoveCommand Process(GameModel game, Move move)
        {
            MoveCommand moveCommand;

            moveCommand = ProcessEnPassant(game, move);

            if (moveCommand != null)
            {
                return moveCommand;
            }

            moveCommand = ProcessCastlingLong(game, move);

            if (moveCommand != null)
            {
                return moveCommand;
            }

            moveCommand = ProcessCastlingShort(game, move);

            if (moveCommand != null)
            {
                return moveCommand;
            }

            moveCommand = ProcessPawnPromotion(game, move);

            if (moveCommand != null)
            {
                return moveCommand;
            }

            var movedPiece = game.MovingPlayer.Pieces.PieceAtPosition(move.From);
            var attackedPiece = game.WaitingPlayer.Pieces.PieceAtPosition(move.To);

            return new MoveCommand(move, movedPiece, attackedPiece);
        }
    }
}
