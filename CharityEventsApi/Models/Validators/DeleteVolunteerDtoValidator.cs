using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Models.Validators
{
    public class DeleteVolunteerDtoValidator : AbstractValidator<DeleteVolunteerDto>
    {
        public DeleteVolunteerDtoValidator(CharityEventsDbContext dbContext)
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
                   var volunteeringExist = dbContext.Volunteerings.Any(v => v.IdVolunteering == value);

                   if (!volunteeringExist)
                       context.AddFailure("IdVolunteering", "Akcja wolontariacka o podanym ID nie istnieje");
               });

            RuleFor(x => x)
                .Custom((value, context) =>
                {
                    var volunteering = dbContext.Volunteerings
                    .Include(v => v.UserIdUsers)
                    .FirstOrDefault(v => v.IdVolunteering == value.IdVolunteering);
                   
                    var user = dbContext.Users.FirstOrDefault(u => u.IdUser == value.IdUser);

                    if (volunteering != null && user != null)
                        if (!volunteering.UserIdUsers.Contains(user))
                            context.AddFailure("IdUser&IdVolunteering", "Użytkownik o podanym ID nie jest przypisany do akcji o podanym ID");

                });
        }
    }
}
