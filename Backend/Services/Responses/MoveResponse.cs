using Domain;
using Domain.Commands;

namespace Services.Responses
{
    public class MoveResponse
    {
        public MoveCommand MoveCommand { get; set; }
        public GameStatusModel GameStatusModel { get; set; }
    }
}
