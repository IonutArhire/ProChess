using System.Collections.Generic;
using Domain;
using Domain.ClientModels;
using Domain.Commands;
using Domain.Pieces;

namespace Services.GameValidationService
{
    public interface IGameValidationService
    {
        bool IsSquareAttackedByOpponent(GameModel game, Position square);

        bool CanReach(GameModel game, Move move);

        MoveCommand ValidateMove(GameModel game, Move move);
    }
}