using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;

namespace CharityEventsApi.Models.Validators
{
    public class EditCharityEventDtoValidator : AbstractValidator<EditCharityEventDto>
    {
        public EditCharityEventDtoValidator(CharityEventsDbContext dbContext)
        {
            RuleFor(x => x.OrganizerId)
                .Custom((value, context) =>
                {
                    var userExist = dbContext.Users.Any(u => u.IdUser == value);

                    if (!userExist && value != null)
                        context.AddFailure("OrganizerId", "Użytkownik o podanym id nie istnieje");
                });
            
            RuleFor(x => x.ImageId)
                .Custom((value, context) =>
                {
                    var imageExist = dbContext.Images.Any(i => i.IdImages == value);

                    if (!imageExist && value != null)
                        context.AddFailure("ImageId", "Image o podanym id nie istnieje");
                });

        }
    }
}
