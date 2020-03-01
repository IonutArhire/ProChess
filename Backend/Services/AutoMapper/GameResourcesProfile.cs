using AutoMapper;
using Domain;
using Services.Responses;

namespace Services.AutoMapper
{
    public class GameResourcesProfile : Profile
    {
        public GameResourcesProfile()
        {
            CreateMap<GameModel, GameResourcesResponse>();
        }
    }
}
