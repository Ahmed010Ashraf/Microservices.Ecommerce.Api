using FluentValidation;
using Ordering.Application.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Validators
{
    public class CheckOutOrderCommandValidationV2 : AbstractValidator<CheckOutOrderCommandV2>
    {
        public CheckOutOrderCommandValidationV2()
        {
            RuleFor(o => o.UserName).NotEmpty().WithMessage("{UserName} is required")
    .NotNull().MaximumLength(70).WithMessage("{UserName} cannot be more than 70 char");

            RuleFor(o => o.TotalPrice).NotEmpty().WithMessage("{TotalPrice} is required")
                .NotNull().GreaterThan(-1).WithMessage("{TotalPrice} should be greater than zero");
        }
    }
}
