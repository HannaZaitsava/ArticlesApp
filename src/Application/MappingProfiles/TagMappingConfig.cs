using ArticlesApp.Application.CQRS.Commands.TagCommands.UpdateTag;
using ArticlesApp.Domain.Entities;
using Mapster;

namespace ArticlesApp.Application.MappingProfiles
{  
    public class TagMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UpdateTagCommand, Tag>()
               .IgnoreNullValues(true)
               .Ignore(dest => dest.Id);
        }
    }
}