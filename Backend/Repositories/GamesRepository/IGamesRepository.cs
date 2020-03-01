using System;
using Domain;

namespace Repositories.GamesRepository
{
    public interface IGamesRepository
    {
        GameModel CreateGame();
        void DeleteGame();
        GameModel GetGame(Guid gameId);
    }
}