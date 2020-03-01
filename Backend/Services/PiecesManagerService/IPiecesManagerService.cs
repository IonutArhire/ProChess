using System.Collections.Generic;
using Domain;
using Domain.ClientModels;
using Domain.Commands;
using Domain.Pieces;
using Services.GameValidationService;

namespace Services.PiecesManagerService
{
    public interface IPiecesManagerService
    {
        void UpdatePieces(GameModel game, MoveCommand moveCommand);

        bool IsKingCaptured(GameModel game);
        bool IsPlayerUnableToMove(GameModel game);
    }
}