using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;
using System.Numerics;

namespace CharityEventsApi.Models.Validators
{
    public class AddAllCharityEventsDtoValidator : AbstractValidator<AddAllCharityEventsDto>
    {
        public AddAllCharityEventsDtoValidator(CharityEventsDbContext dbContext)
        {
            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.IdOrganizer)
                .NotEmpty()
                .Custom((value, context) =>
                {
                    var userExist = dbContext.Users.Any(u => u.IdUser == value);

                    if(!userExist)
                        context.AddFailure("OrganizerId", "Użytkownik o podanym id nie istnieje");
                });

            RuleFor(x => x.ImageCharityEvent)
                .NotEmpty();

            RuleFor(x => x.AmountOfMoneyToCollect)
                .GreaterThan(0);

            RuleFor(x => x.AmountOfNeededVolunteers)
                .GreaterThan(0);

            RuleFor(x => x)
                .Custom((value, context) =>
                {
                    if (value.IsVolunteering == false && value.IsFundraising == false)
                        context.AddFailure("IsVolunteeringOrIsFundraising", "Wydarzenie musi być zbiórką pieniędzy lub akcją wolontariacką");

                    if (value.IsVolunteering == true)
                    {
                        if (value.AmountOfNeededVolunteers == null)
                            context.AddFailure("AmountOfNeededVolunteers", "Jeśli wydarzenie jest akcją wolontariacką, należy podać liczbę potrzebnych wolontariuszy");
                    }

                    if (value.IsFundraising == true)
                    {
                        if (String.IsNullOrEmpty(value.FundTarget))
                            context.AddFailure("FundTarget", "Jeśli wydarzenie jest zbiórką pieniędzy, należy podać cel zbiórki");

                        if (value.AmountOfMoneyToCollect == null)
                            context.AddFailure("AmountOfMoneyToCollect", "Jeśli wydarzenie jest zbiórką pieniędzy, należy podać docelową kwotę");
                    }
                });
        }
    }
}
