using FluentValidation;
using CarRental.Application.DTOs.Rental;

namespace CarRental.Application.Validators;

public class CreateRentalValidator : AbstractValidator<CreateRentalDto>
{
    public CreateRentalValidator()
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage("Car ID must be greater than 0.");

        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Start date must be in the future.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after the start date.");

        RuleFor(x => x.EndDate)
            .Must((dto, endDate) =>
            {
                var duration = endDate - dto.StartDate;
                return duration.TotalDays <= 365; // Max 1 year
            })
            .WithMessage("Rental period cannot exceed 365 days.");
    }
}
