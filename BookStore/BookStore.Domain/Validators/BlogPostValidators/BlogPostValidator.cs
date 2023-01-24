using BookStore.Domain.Models.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Validators.BlogPostValidators
{
    public class BlogPostValidator : AbstractValidator<BlogPost>
    {
        public BlogPostValidator()
        {
            RuleFor(entity => entity.Title)
                .NotEmpty()
                .WithMessage("This field is required to be filled in!")
                .MaximumLength(150)
                .WithMessage("Too long. The maximum length is 150!");

            RuleFor(entity => entity.Body)
                .NotEmpty()
                .WithMessage("This field is required to be filled in!")
                .MinimumLength(120)
                .WithMessage("Too short. The minimum length is 120!");

            RuleFor(entity => entity.ImagePath)
                .NotEmpty()
                .WithMessage("This field is required to be filled in!");
        }
    }
}
