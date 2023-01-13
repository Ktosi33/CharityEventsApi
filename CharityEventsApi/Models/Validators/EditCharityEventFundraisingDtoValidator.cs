using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;

namespace CharityEventsApi.Models.Validators
{
    public class EditCharityEventFundraisingDtoValidator : AbstractValidator<EditCharityEventFundraisingDto>
    {
        public EditCharityEventFundraisingDtoValidator()
        {
            RuleFor(x => x.AmountOfMoneyToCollect)
                .GreaterThan(0);
        }
    }
}
