using ArticlesApp.Application.CQRS.Validators;
using ArticlesApp.Application.Common.Constants;
using ArticlesApp.Application.CQRS.Queries.TagQueries.GetAllTags;
using FluentValidation;

namespace ArticlesApp.Application.CQRS.Validators
{    
    public class GetAllTagsQueryValidator : AbstractValidator<GetAllTagsQuery>
    {
        public GetAllTagsQueryValidator()
        {
            RuleFor(x => x.PaginationParameters)
                .NotEmpty()
                // Передаем фабрику, которая создает и настраивает валидатор в рантайме
                .SetValidator(_ => new OffsetPaginationParametersValidator()
                    .Configure(PaginationConstants.TagsDefaultPageSize));
        }
    }
}
