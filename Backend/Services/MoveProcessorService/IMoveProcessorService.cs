using Domain;
using Domain.ClientModels;
using Domain.Commands;

namespace Services.MoveProcessorService
{
    public interface IMoveProcessorService
    {
        MoveCommand Process(GameModel game, Move move);
    }
}