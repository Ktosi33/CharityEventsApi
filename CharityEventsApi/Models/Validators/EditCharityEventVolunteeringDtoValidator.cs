using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;

namespace CharityEventsApi.Models.Validators
{
    public class EditCharityEventVolunteeringDtoValidator : AbstractValidator<EditCharityEventVolunteeringDto>
    {
        public EditCharityEventVolunteeringDtoValidator()
        {
            RuleFor(x => x.AmountOfNeededVolunteers)
                .GreaterThan(0);
        }
    }
}
