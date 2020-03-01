using System.Collections.Generic;
using System.Linq;
using Domain;
using Services.ChessVariants;

namespace Services.GameTypeDispatcherService
{
    public class GameTypeDispatcherService : IGameTypeDispatcherService
    {
        private readonly IEnumerable<IBaseChessVariantService> baseVariantServices;

        public GameTypeDispatcherService(IEnumerable<IBaseChessVariantService> baseVariantServices)
        {
            this.baseVariantServices = baseVariantServices;
        }

        public IBaseChessVariantService GetVariantService(GameType gameType)
        {
            return baseVariantServices.FirstOrDefault(service => service.GameType == gameType);
        }
    }
}
