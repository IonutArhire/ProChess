using Domain;
using Services.GameValidationService;
using Services.PiecesManagerService;

namespace Services.ChessVariants
{
    public class KingOfTheHillChessVariantService : BaseChessVariantService
    {
        public KingOfTheHillChessVariantService(IPiecesManagerService piecesManagerService, IGameValidationService gameValidationService) 
            : base(piecesManagerService, gameValidationService)
        {
            GameType = GameType.KingOfTheHillVariant;
        }
    }
}
