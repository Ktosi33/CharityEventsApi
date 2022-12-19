using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;

namespace CharityEventsApi.Models.Validators
{
    public class AddLocationDtoValidator : AbstractValidator<AddLocationDto>
    {
        public AddLocationDtoValidator(CharityEventsDbContext dbContext)
        {
            RuleFor(x => x.IdVolunteering)
                .NotEmpty()
                .Custom((value, context) =>
                {
                    var volunteeringExist = dbContext.Volunteerings.Any(v => v.IdVolunteering == value);

                    if (!volunteeringExist)
                        context.AddFailure("IdVolunteering", "Akcja wolontariacka o podanym ID nie istnieje");
                });

            RuleFor(x => x.PostalCode)
                .MaximumLength(10)
                .NotEmpty();

            RuleFor(x => x.Town)
                .NotEmpty()
                .Matches("^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$");

            RuleFor(x => x.Street)
                .NotEmpty();
        }
    }
}
