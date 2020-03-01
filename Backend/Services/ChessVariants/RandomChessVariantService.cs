using System;
using Domain;
using Services.GameValidationService;
using Services.PiecesManagerService;

namespace Services.ChessVariants
{
    public class RandomChessVariantService : BaseChessVariantService
    {
        public RandomChessVariantService(IPiecesManagerService piecesManagerService, IGameValidationService gameValidationService)
            : base(piecesManagerService, gameValidationService)
        {
            GameType = GameType.RandomVariant;
        }

        public override void InitializeBoard(GameModel game)
        {
            throw new NotImplementedException();
        }
    }
}
