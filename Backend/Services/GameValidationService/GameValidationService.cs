using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.ClientModels;
using Domain.Commands;
using Domain.Directions;
using Domain.Pieces;
using Services.MoveProcessorService;

namespace Services.GameValidationService
{
    public class GameValidationService : IGameValidationService
    {
        private IMoveProcessorService moveProcessorService;

        public GameValidationService(IMoveProcessorService moveProcessorService)
        {
            this.moveProcessorService = moveProcessorService;
        }

        // If the king is in check then only he can move or a another piece can block the check
        private bool IsLegalMoveWhileInCheck(GameModel game, MoveCommand moveCommand)
        {
            var king = game.MovingPlayer.Pieces.King();

            if (IsSquareAttackedByOpponent(game, king.Position))
            {
                var movingPiece = game.MovingPlayer.Pieces.PieceAtPosition(moveCommand.Move.From);

                if (movingPiece == king)
                {
                    return true;
                }

                moveCommand.Do();
                var isKingInCheckAfterMove = IsSquareAttackedByOpponent(game, king.Position);
                moveCommand.Undo();

                return !isKingInCheckAfterMove;
            }
            else
            {
                return true;
            }
        }

        private bool IsPieceMovingIntoCheck(GameModel game, MoveCommand moveCommand)
        {
            var king = game.MovingPlayer.Pieces.King();

            moveCommand.Do();
            var isKingInCheckAfterMove = IsSquareAttackedByOpponent(game, king.Position);
            moveCommand.Undo();

            return isKingInCheckAfterMove;
        }

        private bool CanPawnMoveForward(GameModel game,
                                        IPawn movingPawn,
                                        Position destination,
                                        Position forwardOne,
                                        Position forwardTwo)
        {
            if (game.MovingPlayer.Pieces.PieceAtPosition(forwardOne) != null ||
                game.WaitingPlayer.Pieces.PieceAtPosition(forwardOne) != null)
            {
                return false;
            }

            if (forwardTwo.Equals(destination))
            {
                if (game.MovesHistory.Any(m => m.MovedPiece == movingPawn))
                {
                    return false;
                }

                if (game.MovingPlayer.Pieces.PieceAtPosition(forwardTwo) != null ||
                    game.WaitingPlayer.Pieces.PieceAtPosition(forwardTwo) != null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CanPawnMoveOnDiagonals(GameModel game, MoveCommand moveCommand)
        {
            var movingPiece = game.MovingPlayer.Pieces.PieceAtPosition(moveCommand.Move.From);

            if (!(movingPiece is IPawn))
            {
                return true;
            }

            if (moveCommand is EnPassantCommand)
            {
                return game.MovesHistory.Last().MovedPiece == moveCommand.TakenPiece;
            }

            return moveCommand.TakenPiece != null;
        }

        private bool IsLegalMoveWithPawn(GameModel game, MoveCommand moveCommand)
        {
            var movingPiece = game.MovingPlayer.Pieces.PieceAtPosition(moveCommand.Move.From);

            if (movingPiece is IPawn movingPawn)
            {
                var forwardOne = movingPawn.Position + movingPawn.GetForwardOne();
                var forwardTwo = movingPawn.Position + movingPawn.GetForwardTwo();

                if (forwardOne.Equals(moveCommand.Move.To) ||
                    forwardTwo.Equals(moveCommand.Move.To))
                {
                    return CanPawnMoveForward(game, movingPawn, moveCommand.Move.To, forwardOne,
                        forwardTwo);
                }

                return CanPawnMoveOnDiagonals(game, moveCommand);
            }

            return true;
        }

        private bool IsPathBlockedByAnyPiece(GameModel game, IChessPiece movingPiece, Position destination)
        {
            var transition = movingPiece.GetTransitionToDestination(destination);

            var nextPosition = movingPiece.Position + transition;

            while (!nextPosition.Equals(destination))
            {
                if (game.MovingPlayer.Pieces.PieceAtPosition(nextPosition) != null ||
                    game.WaitingPlayer.Pieces.PieceAtPosition(nextPosition) != null)
                {
                    return true;
                }

                nextPosition += transition;
            }

            return false;
        }

        private bool IsPathBetweenKingAndRookClear(GameModel game, King king, Rook rook, Direction direction)
        {
            var pathBetweenKingAndRook = new List<Position>();

            for (var kingPos = king.Position.Clone() + new Transition(direction);
                !kingPos.Equals(rook.Position);
                kingPos += new Transition(direction))
            {
                pathBetweenKingAndRook.Add(kingPos.Clone());
            }

            return pathBetweenKingAndRook.All(pos => game.MovingPlayer.Pieces.PieceAtPosition(pos) == null);
        }

        private bool IsKingCastlingPathSafe(GameModel game, King king, Direction direction)
        {
            var kingPath = new List<Position>
            {
                king.Position,
                king.Position + new Transition(direction),
                king.Position + new Transition(direction + direction)
            };

            return kingPath.All(kp => !IsSquareAttackedByOpponent(game, kp));
        }

        private bool IsLegalCastlingMove(GameModel game, MoveCommand moveCommand)
        {
            if (!(moveCommand is CastlingCommand castlingCommand))
            {
                return true;
            }

            var direction = castlingCommand is CastlingLongCommand ? Directions.Left : Directions.Right;

            var king = (King)castlingCommand.MovedPiece;
            var rook = castlingCommand.MovedRook;

            if (game.MovesHistory.Any(m => m.MovedPiece == king) ||
                game.MovesHistory.Any(m => m.MovedPiece == rook))
            {
                return false;
            }

            return IsPathBetweenKingAndRookClear(game, king, rook, direction) && IsKingCastlingPathSafe(game, king, direction);
        }

        private bool CanReach(GameModel game, MoveCommand moveCommand)
        {
            var movingPiece = game.MovingPlayer.Pieces.PieceAtPosition(moveCommand.Move.From);
            var isDestinationOnTrajectory = movingPiece.GetTransitionToDestination(moveCommand.Move.To) != null;

            if (isDestinationOnTrajectory &&
                !IsPathBlockedByAnyPiece(game, movingPiece, moveCommand.Move.To))
            {
                if (movingPiece is IPawn)
                {
                    return IsLegalMoveWithPawn(game, moveCommand);
                }

                return true;
            }

            return false;
        }

        public bool CanReach(GameModel game, Move move)
        {
            var moveCommand = moveProcessorService.Process(game, move);
            return CanReach(game, moveCommand);
        }

        public bool IsSquareAttackedByOpponent(GameModel game, Position square)
        {
            return game.SwitchPlayerPerspective(() =>
            {
                foreach (var chessPiece in game.MovingPlayer.Pieces.Where(p => !p.IsTaken))
                {
                    var move = new Move(chessPiece.Position, square);
                    var moveCommand = moveProcessorService.Process(game, move);

                    if (CanReach(game, moveCommand))
                    {
                        return true;
                    }
                }

                return false;
            });
        }

        public MoveCommand ValidateMove(GameModel game, Move move)
        {
            var sourcePosition = move.From;
            var destination    = move.To;

            var isMoveInBounds                = sourcePosition.IsInBounds() && destination.IsInBounds();
            var isMoveWithOwnedPiece          = game.MovingPlayer.Pieces.PieceAtPosition(sourcePosition) != null;
            var isDestinationOccupiedByFriend = game.MovingPlayer.Pieces.PieceAtPosition(destination) != null;

            if (!isMoveInBounds ||
                !isMoveWithOwnedPiece ||
                isDestinationOccupiedByFriend)
            {
                return null;
            }

            var moveCommand = moveProcessorService.Process(game, move);

            if (moveCommand is CastlingCommand)
            {
                return IsLegalCastlingMove(game, moveCommand) ? moveCommand : null;
            }
            else
            {
                var isMoveValid = CanReach(game, moveCommand) &&
                                  IsLegalMoveWhileInCheck(game, moveCommand) &&
                                  !IsPieceMovingIntoCheck(game, moveCommand);

                return isMoveValid ? moveCommand : null;
            }
        }
    }
}
