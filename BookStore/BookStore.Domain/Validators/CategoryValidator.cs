using BookStore.Domain.Models.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(entity => entity.Name)
                .NotEmpty()
                .WithMessage("This field is required to be filled in!")
                .MaximumLength(100)
                .WithMessage("Too long. The maximum length is 40!");
        }
    }
}
