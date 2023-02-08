using BookStore.Domain.Models.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Validators
{
    public class AuthorValidator : AbstractValidator<Author>
    {
        public AuthorValidator()
        {
            RuleFor(entity => entity.Name)
                .NotEmpty()
                .WithMessage("This field is required to be filled in!");

            RuleFor(entity => entity.Biography)
               .NotEmpty()
               .WithMessage("This field is required to be filled in!");

            RuleFor(entity => entity.ImagePath)
               .NotEmpty()
               .WithMessage("A photo should be uploaded!");
        }
    }
}
