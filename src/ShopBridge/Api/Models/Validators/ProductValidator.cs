using FluentValidation;

namespace ShopBridge.Api.Models.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(m => m.Id)
                .NotNull()
                .WithMessage("{PropertyName} must not be null.")
                .NotEmpty()
                .WithMessage("{PropertyName} must not be empty.");

            RuleFor(m => m.Name)
                .NotNull()
                .WithMessage("{PropertyName} must not be null.")
                .NotEmpty()
                .WithMessage("{PropertyName} must not be empty.");

            RuleFor(m => m.Description)
                .NotNull()
                .WithMessage("{PropertyName} must not be null.")
                .NotEmpty()
                .WithMessage("{PropertyName} must not be empty.");

            RuleFor(m => m.Price)
                .NotNull()
                .WithMessage("{PropertyName} must not be null.")
                .NotEmpty()
                .WithMessage("{PropertyName} must not be null or empty.");
        }
    }
}
