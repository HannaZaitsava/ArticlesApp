using Application.CQRS.Commands.TagCommands.UpdateTag;
using Application.CQRS.Queries.TagQueries.GetAllTags;
using ArticlesAPI.Models.Requests;
using Mapster;

namespace ArticlesAPI.Models.MappingConfigurations
{
    public class TagMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(Guid id, TagApiRequest requestBody), UpdateTagCommand>()
               .Map(dest => dest.Id, src => src.id) 
               .Map(dest => dest, src => src.requestBody);

            config.NewConfig<GetAllTagsPaginatedApiRequest, GetAllTagsQuery>()
               .Map(dest => dest.PaginationParameters.PageIndex, src => src.PageIndex)
               .Map(dest => dest.PaginationParameters.PageSize, src => src.PageSize);
        }
    }
}
