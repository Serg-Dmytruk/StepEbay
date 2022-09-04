using FluentValidation;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Common.Services.DataValidationServices
{
    class ProductValidator : AbstractValidator<ProductDto>, IProductValidator
    {
        public ProductValidator()
        {
            RuleFor(data => data.Id).NotNull();
            //Обов'язкові поля
            RuleFor(data => data.Title).NotEmpty().WithMessage("Заголовок пустий");
            RuleFor(data => data.Description).NotEmpty().WithMessage("Опис пусте");
            RuleFor(data => data.Price).GreaterThan(0).WithMessage("Ціна має бути більше 0").NotEmpty().WithMessage("Ціна пуста");
            RuleFor(data => data.CategoryId).NotEqual(0).WithMessage("Категорія неправельна").NotEmpty().WithMessage("Категорія неправельна");
            RuleFor(data => data.StateId).NotEqual(0).WithMessage("Стан неправельний").NotEmpty().WithMessage("Стан неправельний");
            RuleFor(data => data.OwnerId).NotEqual(0).WithMessage("Помилка з власником").NotEmpty().WithMessage("Помилка з власником");
            RuleFor(data => data.PurchaseTypeId).NotEqual(0).WithMessage("Тип покупки неправельний").NotEmpty().WithMessage("Тип покупки неправельний");
            //Довжина поля
            RuleFor(data => data.Title).Length(2, 40).WithMessage("Довжина заголовока замала, або завелика");
            RuleFor(data => data.Description).Length(20, 300).WithMessage("Довжина опису замала, або завелика");
        }
    }
}
