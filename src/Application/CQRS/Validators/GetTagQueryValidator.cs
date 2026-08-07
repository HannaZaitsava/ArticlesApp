using Application.CQRS.Queries.TagQueries.GetTag;
using FluentValidation;

namespace Application.CQRS.Validators
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
