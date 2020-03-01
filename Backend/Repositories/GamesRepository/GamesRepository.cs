using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Domain;

namespace Repositories.GamesRepository
{
    public class GamesRepository : IGamesRepository
    {
        private readonly IDictionary<Guid, GameModel> games;

        public GamesRepository()
        {
            games = new ConcurrentDictionary<Guid, GameModel>();
        }

        public GameModel CreateGame()
        {
            var newGame = new GameModel(GameType.ClassicalVariant);
            games.Add(newGame.Id, newGame);

            return newGame;
        }

        public void DeleteGame()
        {
            throw new NotImplementedException();
        }

        public GameModel GetGame(Guid gameId)
        {
            return games[gameId];
        }
    }
}
