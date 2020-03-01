using Domain.Players;

namespace Services.Responses
{
    public class GameResourcesResponse
    {
        public Player WhitePlayer { get; set; }
        public Player BlackPlayer { get; set; }
    }
}
