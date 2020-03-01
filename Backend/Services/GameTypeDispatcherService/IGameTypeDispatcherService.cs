using Domain;
using Services.ChessVariants;

namespace Services.GameTypeDispatcherService
{
    public interface IGameTypeDispatcherService
    {
        IBaseChessVariantService GetVariantService(GameType gameType);
    }
}