using FluentValidation;
using Ordering.Application.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Validators
{
    public class UpdateOrderCommandValidator:AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(o=>o.Id).NotEmpty().WithMessage("{Id} is required")
                .NotNull().GreaterThan(0).WithMessage("{Id} should be greater than zero");


            RuleFor(o => o.UserName).NotEmpty().WithMessage("{UserName} is required")
              .NotNull().MaximumLength(70).WithMessage("{UserName} cannot be more than 70 char");

            RuleFor(o => o.TotalPrice).NotEmpty().WithMessage("{TotalPrice} is required")
                .NotNull().GreaterThan(-1).WithMessage("{TotalPrice} should be greater than zero");


            RuleFor(o => o.Email).NotEmpty().WithMessage("{EmailAddress} is required")
                .NotNull().WithMessage("{EmailAddress} cannot be more than 50 char");

            RuleFor(o => o.FirstName).NotEmpty().WithMessage("{FirstName} is required")
                .NotNull().WithMessage("{FirstName} cannot be more than 50 char");


            RuleFor(o => o.LastName).NotEmpty().WithMessage("{LastName} is required")
           .NotNull().WithMessage("{LastName} cannot be more than 50 char");
        }
    }
}
