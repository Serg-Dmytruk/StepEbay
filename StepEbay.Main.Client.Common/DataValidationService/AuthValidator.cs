using FluentValidation;
using StepEbay.Main.Common.Models.Auth;

namespace StepEbay.Main.Client.Common.DataValidationServices
{
    public class AuthValidator : AbstractValidator<SignUpRequestDto>, IAuthValidator
    {
        public AuthValidator()
        {
            RuleFor(data => data.Id).NotNull();
            //Обов'язкові поля
            RuleFor(data => data.FullName).NotEmpty().WithMessage("EmptyFullName");
            RuleFor(data => data.NickName).NotEmpty().WithMessage("EmptyNickName");
            RuleFor(data => data.Password).NotEmpty().WithMessage("EmptyPassword");
            RuleFor(data => data.Email).NotEmpty().WithMessage("EmptyMail");
            //Довжина поля
            RuleFor(data => data.FullName).Length(3, 30).WithMessage("LengthFullName");
            RuleFor(data => data.NickName).Length(5, 30).WithMessage("LengthNickName");
            RuleFor(data => data.Password).MinimumLength(8).WithMessage("LengthPassword");
            //Ключові символи
            RuleFor(data => data.Password).Matches("[A-Z]").WithMessage("AgreementsPassword");
            RuleFor(data => data.Password).Matches("[a-z]").WithMessage("AgreementsPassword");
            RuleFor(data => data.Password).Matches("[0-9]").WithMessage("AgreementsPassword");
            RuleFor(data => data.Email).EmailAddress().WithMessage("AgreementsMail");
            //Копія
            RuleFor(data => data.CopyPassword).Equal(data => data.Password).WithMessage("CopyPassword");
        }
    }
}
