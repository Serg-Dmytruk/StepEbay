using FluentValidation;
using StepEbay.Main.Common.Models.Auth;

namespace StepEbay.Main.Api.Common.Services.DataValidationServices
{
    class AuthValidator : AbstractValidator<SignUpRequestDto>, IAuthValidator
    {
        public AuthValidator()
        {
            RuleFor(data => data.Id).NotNull();
            RuleFor(data => data.FullName).NotEmpty().WithMessage("Обов'язкове поле: Повне ім'я");
            RuleFor(data => data.NickName).Length(5, 16).WithMessage("Нікнейм повинен складатись з 5 - 16 символів");
            RuleFor(data => data.Password).MinimumLength(8).Matches("[A-Z]").Matches("[a-z]").Matches("[0-9]").WithMessage("Пароль повинен мати мінімум 8 символів (мінімум 1 велика, 1 маленька літера і 1 цифра)");
            RuleFor(data => data.Email).EmailAddress().WithMessage("Невірний електронний адрес");
            RuleFor(data => data.CopyPassword).Equal(data => data.Password).WithMessage("Паролі не співпадають");
        }
    }
}
