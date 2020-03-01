using System;
using AutoMapper;
using Domain.ClientModels;
using Domain.Commands;
using Repositories.GamesRepository;
using Services.GameTypeDispatcherService;
using Services.Responses;

namespace Services.GamesManagerService
{
    public class GamesManagerService : IGamesManagerService
    {
        private readonly IGamesRepository gamesRepo;
        private readonly IGameTypeDispatcherService gameTypeDispatcher;
        private readonly IMapper mapper;

        public GamesManagerService(IGamesRepository gamesRepo, 
                                   IGameTypeDispatcherService gameTypeDispatcher, 
                                   IMapper mapper)
        {
            this.gamesRepo = gamesRepo;
            this.gameTypeDispatcher = gameTypeDispatcher;
            this.mapper = mapper;
        }

        public Guid CreateGame()
        {
            var game = gamesRepo.CreateGame();
            var variantService = gameTypeDispatcher.GetVariantService(game.GameType);

            variantService.InitializeBoard(game);
            return game.Id;
        }

        public GameResourcesResponse GetGameResources(Guid gameId)
        {
            var game = gamesRepo.GetGame(gameId);

            var gameResources = mapper.Map<GameResourcesResponse>(game);

            return gameResources;
        }

        public MoveResponse MakeMove(Guid gameId, Move move, string promotionType = null)
        {
            var game = gamesRepo.GetGame(gameId);
            var variantService = gameTypeDispatcher.GetVariantService(game.GameType);

            var moveCommand = variantService.MakeMove(game, move, promotionType);

            var moveResponse = new MoveResponse
            {
                GameStatusModel = game.GameStatusModel,
                MoveCommand = moveCommand
            };

            return moveResponse;
        }
    }
}
