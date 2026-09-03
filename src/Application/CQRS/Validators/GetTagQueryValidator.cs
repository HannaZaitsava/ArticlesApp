using ArticlesApp.Application.CQRS.Queries.TagQueries.GetTag;
using FluentValidation;

namespace ArticlesApp.Application.CQRS.Validators
{
    public sealed class GetTagQueryValidator : AbstractValidator<GetTagQuery>
    {
        public GetTagQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Tag Id is required");
        }
    }
}
