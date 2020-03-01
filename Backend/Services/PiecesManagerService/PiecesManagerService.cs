using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.ClientModels;
using Domain.Commands;
using Domain.Pieces;
using Services.GameValidationService;

namespace Services.PiecesManagerService
{
    public class PiecesManagerService : IPiecesManagerService
    {
        private IGameValidationService gameValidationService;

        public PiecesManagerService(IGameValidationService gameValidationService)
        {
            this.gameValidationService = gameValidationService;
        }

        private List<IChessPiece> GetAttackersOfTarget(GameModel game,
                                                       IChessPiece target)
        {
            var attackers = new List<IChessPiece>();

            game.SwitchPlayerPerspective(() =>
            {
                foreach (var chessPiece in game.MovingPlayer.Pieces)
                {
                    var move = new Move(chessPiece.Position, target.Position);

                    if (gameValidationService.CanReach(game, move))
                    {
                        attackers.Add(chessPiece);
                    }
                }
            });

            return attackers;
        }

        private bool CanTakePiece(GameModel game,
                                 IChessPiece target)
        {
            if (target is King)
            {
                return false;
            }

            foreach (var chessPiece in game.MovingPlayer.Pieces)
            {
                if (gameValidationService.ValidateMove(game, new Move(chessPiece.Position, target.Position)) != null)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CanBlockAttack(GameModel game, IChessPiece target, IChessPiece attacker)
        {
            var positionsToBlock = attacker.GetPathToDestination(target.Position);

            if (positionsToBlock is null)
            {
                return false;
            }

            foreach (var positionToBlock in positionsToBlock)
            {
                foreach (var chessPiece in game.MovingPlayer.Pieces)
                {
                    var move = new Move(chessPiece.Position, positionToBlock);

                    if (!(chessPiece is King) &&
                        gameValidationService.ValidateMove(game, move) != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CanPieceMove(GameModel game, IChessPiece piece)
        {
            foreach (var transition in piece.AvailableTransitions)
            {
                if (transition.IsOneTimeOnly)
                {
                    var move = new Move(piece.Position, piece.Position + transition);

                    if (gameValidationService.ValidateMove(game, move) != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var nextPosition = piece.Position + transition;

                    while (nextPosition.IsInBounds())
                    {
                        var move = new Move(piece.Position, nextPosition);

                        if (gameValidationService.ValidateMove(game, move) != null)
                        {
                            return true;
                        }

                        nextPosition += transition;
                    }
                }
            }

            return false;
        }

        public bool CanKingMove(GameModel game, King king)
        {
            foreach (var move in king.AvailableTransitions)
            {
                var dest = king.Position + move;

                if (gameValidationService.ValidateMove(game, new Move(king.Position, dest)) != null)
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdatePieces(GameModel game, MoveCommand moveCommand)
        {
            moveCommand.Do();
        }

        public bool IsKingCaptured(GameModel game)
        {
            var king = game.MovingPlayer.Pieces.King();

            if (gameValidationService.IsSquareAttackedByOpponent(game, king.Position))
            {
                if (CanKingMove(game, king))
                {
                    return false;
                }

                var attackers = GetAttackersOfTarget(game, king);

                if (attackers.Count >= 2)
                {
                    return true;
                }

                var attacker = attackers.Single();

                if (CanTakePiece(game, attacker) ||
                    CanBlockAttack(game, attacker, king))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public bool IsPlayerUnableToMove(GameModel game)
        {
            return game.MovingPlayer.Pieces.All(piece => !CanPieceMove(game, piece));
        }
    }
}
