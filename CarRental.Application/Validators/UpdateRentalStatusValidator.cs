using FluentValidation;
using CarRental.Application.DTOs.Rental;

namespace CarRental.Application.Validators;

public class UpdateRentalStatusValidator : AbstractValidator<UpdateRentalStatusDto>
{
    private static readonly string[] ValidStatuses = { "Pending", "Confirmed", "In Progress", "Completed", "Cancelled" };

    public UpdateRentalStatusValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .Must(status => ValidStatuses.Contains(status))
            .WithMessage($"Status must be one of: {string.Join(", ", ValidStatuses)}.");
    }
}
