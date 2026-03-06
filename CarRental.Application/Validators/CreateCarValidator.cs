using FluentValidation;
using CarRental.Application.DTOs.Car;

namespace CarRental.Application.Validators;

public class CreateCarValidator : AbstractValidator<CreateCarDto>
{
    private static readonly string[] ValidFuelTypes = ["Petrol", "Diesel", "Electric", "Hybrid"];
    private static readonly string[] ValidTransmissions = ["Manual", "Automatic"];
    private static readonly string[] ValidStatuses = ["Available", "Rented", "Maintenance"];

    public CreateCarValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("CategoryId must be greater than 0.");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Brand is required.")
            .MaximumLength(50).WithMessage("Brand must not exceed 50 characters.");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required.")
            .MaximumLength(50).WithMessage("Model must not exceed 50 characters.");

        RuleFor(x => x.Year)
            .InclusiveBetween(1900, DateTime.Now.Year + 1)
            .WithMessage($"Year must be between 1900 and {DateTime.Now.Year + 1}.");

        RuleFor(x => x.RegNr)
            .NotEmpty().WithMessage("Registration number is required.")
            .MaximumLength(20).WithMessage("Registration number must not exceed 20 characters.");

        RuleFor(x => x.Fuel)
            .NotEmpty().WithMessage("Fuel type is required.")
            .Must(f => ValidFuelTypes.Contains(f))
            .WithMessage($"Fuel must be one of: {string.Join(", ", ValidFuelTypes)}.");

        RuleFor(x => x.Transmission)
            .NotEmpty().WithMessage("Transmission is required.")
            .Must(t => ValidTransmissions.Contains(t))
            .WithMessage($"Transmission must be one of: {string.Join(", ", ValidTransmissions)}.");

        RuleFor(x => x.Seats)
            .InclusiveBetween(1, 12).WithMessage("Seats must be between 1 and 12.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(s => ValidStatuses.Contains(s))
            .WithMessage($"Status must be one of: {string.Join(", ", ValidStatuses)}.");
    }
}
