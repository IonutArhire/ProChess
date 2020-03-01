using System;
using Domain.ClientModels;
using Domain.Commands;
using Services.Responses;

namespace Services.GamesManagerService
{
    public interface IGamesManagerService
    {
        Guid CreateGame();
        GameResourcesResponse GetGameResources(Guid gameId);
        MoveResponse MakeMove(Guid gameId, Move move, string promotionType = null);
    }
}