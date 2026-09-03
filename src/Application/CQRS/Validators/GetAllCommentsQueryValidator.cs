using ArticlesApp.Application.Common.Constants;
using ArticlesApp.Application.CQRS.Queries.CommentQueries.GetCommentsOffsetPagedQuery;
using FluentValidation;

namespace ArticlesApp.Application.CQRS.Validators
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
