using Application.Common.Constants;
using Application.CQRS.Queries.CommentQueries.GetCommentsOffsetPagedQuery;
using FluentValidation;

namespace Application.CQRS.Validators
{    
    public class GetCommentsOffsetPagedQueryValidator : AbstractValidator<GetCommentsOffsetPagedQuery>
    {
        public GetCommentsOffsetPagedQueryValidator()
        {
            RuleFor(x => x.PaginationParameters)
                .NotEmpty()
                // Передаем фабрику, которая создает и настраивает валидатор в рантайме
                .SetValidator(_ => new OffsetPaginationParametersValidator()
                    .Configure(PaginationConstants.CommentsDefaultPageSize));
        }
    }
}
