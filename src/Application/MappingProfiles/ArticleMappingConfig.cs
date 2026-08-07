using Application.CQRS.Commands.ArticleCommands.CreateArticle;
using Application.CQRS.Commands.ArticleCommands.UpdateArticle;
using Application.DTOs.Articles;
using Domain.Entities;
using Mapster;

namespace Application.MappingProfiles
{
    public class ArticleMappingConfig : IRegister
    {

        public void Register(TypeAdapterConfig config)
        {            
            config.NewConfig<Article, ArticleShortInfoResponseDTO>()
                .Map(dest => dest.CommentsTotalCount, src => src.Comments.Count);                     

            config.NewConfig<UpdateArticleCommand, Article>()
                .Ignore(dest => dest.Tags) // коллекции обновляются вручную в команде
                .Ignore(dest => dest.Categories)
                .Ignore(dest => dest.Comments)
                .Ignore(dest => dest.PublicationDate)
                .IgnoreNullValues(true);

            config.NewConfig<(Article article, string creatorName), ArticleResponseDTO>()
               .Map(dest => dest.CreatorName, src => src.creatorName)
               .Map(dest => dest, src => src.article);
        } 
    }
}
