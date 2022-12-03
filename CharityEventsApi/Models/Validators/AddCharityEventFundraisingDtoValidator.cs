using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;

namespace CharityEventsApi.Models.Validators
{
    public class AddCharityEventFundraisingDtoValidator : AbstractValidator<AddCharityEventFundraisingDto>
    {
        public AddCharityEventFundraisingDtoValidator(CharityEventsDbContext dbContext)
        {
            RuleFor(x => x.CharityEventId)
                .NotEmpty()
                .Custom((value, context) =>
                {
                    var charityEventExist = dbContext.Charityevents.Any(c => c.IdCharityEvent == value);

                    if (!charityEventExist)
                        context.AddFailure("CharityEventId", "Nie istnieje wydarzenie o podanym id");
                });

            RuleFor(x => x.AmountOfMoneyToCollect)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.FundTarget)
                .NotEmpty();

        }
    }
}
