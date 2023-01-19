using BookStore.Domain.Models.Entities;
using FluentValidation;

namespace BookStore.Domain.Validators
{
    public class ContactPostValidator : AbstractValidator<ContactPost>
    {
        public ContactPostValidator()
        {
            RuleFor(entity => entity.FirstName)
                .NotEmpty()
                .WithMessage("This field is required to be filled in!")
                .MaximumLength(40)
                .WithMessage("Too long. The maximum length is 40!");

            RuleFor(entity => entity.LastName)
                .NotEmpty()
                .WithMessage("This field is required to be filled in!")
                .MaximumLength(50)
                .WithMessage("Too long. The maximum length is 50!");

            RuleFor(entity => entity.Email)
                .NotEmpty()
                .WithMessage("This field is required to be filled in!")
                .EmailAddress()
                .WithMessage("Invalid email address!");

            RuleFor(entity => entity.Comment)
                .NotEmpty()
                .WithMessage("This field is required to be filled in!");
        }
    }
}
