using Domain;
using Services.GameValidationService;
using Services.PiecesManagerService;

namespace Services.ChessVariants
{
    public class ClassicalChessVariantService : BaseChessVariantService
    {
        public ClassicalChessVariantService(IPiecesManagerService piecesManagerService, IGameValidationService gameValidationService) 
            : base(piecesManagerService, gameValidationService)
        {
            GameType = GameType.ClassicalVariant;
        }
    }
}
