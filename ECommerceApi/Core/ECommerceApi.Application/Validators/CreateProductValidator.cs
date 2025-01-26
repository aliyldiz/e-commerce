using ECommerceApi.Application.Features.Commands.Product.CreateProduct;
using FluentValidation;

namespace ECommerceApi.Application.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductCommandRequest>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .NotNull()
                .WithMessage("Product name is required")
            .MinimumLength(3)
            .MaximumLength(50)
                .WithMessage("Product name must be between 3 and 50 characters");
        RuleFor(p => p.Stock)
            .NotEmpty()
            .NotNull()
                .WithMessage("Product stock is required")
            .GreaterThan(0)
                .WithMessage("Product stock must be greater than 0");
        RuleFor(p => p.Price)
            .NotEmpty()
            .NotNull()
                .WithMessage("Product price is required")
            .GreaterThan(0)
                .WithMessage("Product price must be greater than 0");
    }
}