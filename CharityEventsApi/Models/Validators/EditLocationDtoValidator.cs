using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;

namespace CharityEventsApi.Models.Validators
{
    public class EditLocationDtoValidator : AbstractValidator<EditLocationDto>
    {
        public EditLocationDtoValidator()
        {
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
