using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Models.Validators
{
    public class AddCharityEventDtoValidator : AbstractValidator<AddCharityEventDto>
    {
        public AddCharityEventDtoValidator(CharityEventsDbContext dbContext)
        {
            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.OrganizerId)
                .NotEmpty()
                .Custom((value, context) =>
                {
                    var userExist = dbContext.Users.Any(u => u.IdUser == value);

                    if (!userExist)
                        context.AddFailure("OrganizerId", "Użytkownik o podanym id nie istnieje");
                });

            RuleFor(x => x.Image)
                .NotEmpty();
        }
    }
}
