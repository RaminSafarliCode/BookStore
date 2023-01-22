using BookStore.Domain.Models.Entities;
using FluentValidation;

namespace BookStore.Domain.Validators
{
    public class FaqValidator : AbstractValidator<Faq>
    {
        public FaqValidator()
        {
            RuleFor(entity => entity.Question)
                .NotEmpty()
                .WithMessage("This field is required to be filled in!")
                .MinimumLength(10)
                .WithMessage("Too short. The minimum length is 10!");

            RuleFor(entity => entity.Answer)
                .NotEmpty()
                .WithMessage("This field is required to be filled in!")
                .MinimumLength(10)
                .WithMessage("Too short. The minimum length is 10!");
        }
    }
}
