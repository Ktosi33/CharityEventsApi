using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;

namespace CharityEventsApi.Models.Validators
{
    public class AddVolunteerDtoValidator : AbstractValidator<AddVolunteerDto>
    {
        public AddVolunteerDtoValidator(CharityEventsDbContext dbContext)
        {
            RuleFor(x => x.IdUser)
                .NotEmpty()
                .Custom((value, context) =>
                {
                    var userExist = dbContext.Users.Any(u => u.IdUser == value);

                    if (!userExist)
                        context.AddFailure("IdUser", "Użytkownik o podanym ID nie istnieje");
                });

            RuleFor(x => x.IdVolunteering)
               .NotEmpty()
               .Custom((value, context) =>
               {
                   var volunteeringExist = dbContext.CharityVolunteerings.Any(v => v.IdCharityVolunteering == value);

                   if (!volunteeringExist)
                       context.AddFailure("IdVolunteering", "Akcja wolontariacka o podanym ID nie istnieje");

                   var volunteering = dbContext.CharityVolunteerings.FirstOrDefault(v => v.IdCharityVolunteering == value);

                   if (volunteering != null && (volunteering.IsVerified == 0 || volunteering.IsActive == 0))
                       context.AddFailure("IdVolunteering", "Akcja wolontariacka musi być aktywna i zweryfikowana");
               });


        }
    }
}
