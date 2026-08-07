using Application.CQRS.Commands.CommentCommands.CreateComment;
using Application.CQRS.Commands.CommentCommands.UpdateComment;
using Application.CQRS.Queries.CommentQueries.GetCommentsCursorPagedQuery;
using Application.CQRS.Queries.CommentQueries.GetCommentsOffsetPagedQuery;
using Application.DTOs.Comments;
using ArticlesAPI.Models.Requests;
using ArticlesAPI.Models.Responses;
using Domain.Entities;
using Mapster;

namespace ArticlesAPI.Models.MappingConfigurations
{
    public class CommentMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(Guid id, CommenApiRequest requestBody), CreateCommentCommand>()
               .Map(dest => dest.ArticleId, src => src.id)
               .Map(dest => dest, src => src.requestBody);

            config.NewConfig<(Guid id, CommentUpdateApiRequest requestBody), UpdateCommentCommand>()
               .Map(dest => dest.Id, src => src.id)
               .Map(dest => dest, src => src.requestBody);         

            config.NewConfig<Comment,(Guid Id, string Text)>()
              .Map(dest => dest.Id, src => src.Id)
              .Map(dest => dest.Text, src => src.Text);            

            //config.NewConfig<Comment, CommentResponseDTO>()            
            //    .Ignore(dest => dest.Replies); // Игнорируем рекурсивное свойство для проекций в БД, т.к. сборка дерева ответов будет собрана в отдельно алгоритме

            config.NewConfig<CommentResponseDTO, CommentApiResponse>()
                .Map(d => d.Text, s => s.IsDeleted ? "This comment has been deleted" : s.Text);
                        
            config.NewConfig<(Guid id, GetArticleCommentsOffsetPaginatedApiRequest requestBody), GetCommentsOffsetPagedQuery>()
              .Map(dest => dest.ArticleId, src => src.id)
              .Map(dest => dest.PaginationParameters.PageIndex, src => src.requestBody.PageIndex)
              .Map(dest => dest.PaginationParameters.PageSize, src => src.requestBody.PageSize); 
            
            //config.NewConfig<(Guid id, GetArticleCommentsCursorPaginatedApiRequest requestBody), GetCommentsCursorQuery>()
            //  .Map(dest => dest.ArticleId, src => src.id)
            //  .Map(dest => dest, src => src.requestBody);

            config.NewConfig<(Guid id, GetArticleCommentsCursorPaginatedApiRequest requestBody), GetCommentsCursorQuery>()
                .Map(dest => dest.ArticleId, src => src.id)
                .Map(dest => dest.PaginationParameters.Cursor, src => src.requestBody.Cursor)
                .Map(dest => dest.PaginationParameters.PageSize, src => src.requestBody.PageSize)
                .Map(dest => dest.PaginationParameters.Direction, src => src.requestBody.Direction);
        }
    }
}
