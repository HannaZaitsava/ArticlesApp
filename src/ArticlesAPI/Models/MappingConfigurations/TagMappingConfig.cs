using ArticlesApp.Application.CQRS.Commands.TagCommands.UpdateTag;
using ArticlesApp.Application.CQRS.Queries.TagQueries.GetAllTags;
using ArticlesApp.ArticlesAPI.Models.Requests;
using Mapster;

namespace ArticlesApp.ArticlesAPI.Models.MappingConfigurations
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
