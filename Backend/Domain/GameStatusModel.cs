using Domain.Players;

namespace Domain
{
    public class GameStatusModel
    {
        public Player Winner { get; set; }
        public GameStatusType GameStatusType { get; set; }

        public GameStatusModel(GameStatusType gameStatusType, Player winner = null)
        {
            Winner = winner;
            GameStatusType = gameStatusType;
        }
    }
}