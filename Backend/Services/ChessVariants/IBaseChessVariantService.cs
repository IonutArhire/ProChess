using Domain;
using Domain.ClientModels;
using Domain.Commands;

namespace Services.ChessVariants
{
    public interface IBaseChessVariantService
    {
        GameType GameType { get; }
        void InitializeBoard(GameModel game);
        MoveCommand MakeMove(GameModel game, Move move, string promotionType = null);
    }
}