using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.DTOs.Comments;
using ArticlesApp.Domain.Entities;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Queries.CommentQueries.GetComment
{
    internal class GetCommentQueryHandler(IBaseRepository<Comment> repository) : IRequestHandler<GetCommentQuery, Result<CommentResponseDTO>>
    {
        public async Task<Result<CommentResponseDTO>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
        {
            Guid commentId = request.Id;

            var commentResponseDto = await repository.GetByIdProjectedAsync<CommentResponseDTO>(commentId, cancellationToken);
                        
            if (commentResponseDto is null)
            {
                return Result<CommentResponseDTO>.Failure([CommentErrors.CommentNotFound(commentId)]);
            }            
            
            return Result<CommentResponseDTO>.Success(commentResponseDto);
        }
    }
}
