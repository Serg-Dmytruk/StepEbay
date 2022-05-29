using FluentValidation;
using StepEbay.Main.Common.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Main.Api.Common.Services.DataValidationServices
{
    class AuthValidator : AbstractValidator<SignUpRequestDto>, IAuthValidator
    {
        public AuthValidator()
        {
            RuleFor(data => data.Id).NotNull();
            RuleFor(data => data.FullName).NotEmpty().WithMessage("Name is required");
            RuleFor(data => data.NickName).Length(5, 16).WithMessage("NickName must be 5 - 16 symbols");
            RuleFor(data => data.Password).MinimumLength(8).Matches("[A-Z]").Matches("[a-z]").Matches("[0-9]").WithMessage("Password must be min 8 symbols. And contains 1 Upper letter, 1 Lower letter, 1 number");
            RuleFor(data => data.Email).EmailAddress().WithMessage("Wrong Email address");
            RuleFor(data => data.CopyPassword).Equal(data => data.Password).WithMessage("Passwords not the same");
        }
    }
}
