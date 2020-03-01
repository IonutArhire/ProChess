using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Domain.ClientModels;
using Services.GamesManagerService;

namespace ChessPOC.Hubs
{
    public class SinglePlayerHub : Hub
    {
        private readonly IGamesManagerService gamesManager;
         
        public SinglePlayerHub(IGamesManagerService gamesManager)
        {
            this.gamesManager = gamesManager;
        }

        public override async Task OnConnectedAsync()
        {
            var gameId = gamesManager.CreateGame();
            await Clients.Caller.SendAsync("Connected", gameId);
        }

        public async Task SendResources(Guid gameId)
        {
            await Clients.Caller.SendAsync("InitialBoardReceived", gamesManager.GetGameResources(gameId));
        }

        public async Task MakeMove(Guid gameId, Move move)
        {
            await Clients.Caller.SendAsync("MoveValidated", gamesManager.MakeMove(gameId, move));
        }

        public async Task PromotePawn(Guid gameId, Move move, string promotionType)
        {
            await Clients.Caller.SendAsync("PawnPromotionValidated", gamesManager.MakeMove(gameId, move, promotionType));
        }
    }
}
