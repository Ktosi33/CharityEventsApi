using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;

namespace CharityEventsApi.Models.Validators
{
    public class EditLocationDtoValidator : AbstractValidator<EditLocationDto>
    {
        public EditLocationDtoValidator()
        {
            RuleFor(x => x.PostalCode)
                .NotEmpty();

            RuleFor(x => x.Town)
                .NotEmpty();

            RuleFor(x => x.Street)
                .NotEmpty();
        }
    }
}
