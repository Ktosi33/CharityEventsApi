using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Models.Validators
{
    public class AddAllPersonalDataDtoValidator : AbstractValidator<AddAllPersonalDataDto>
    {
        public AddAllPersonalDataDtoValidator(CharityEventsDbContext dbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Surname)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.PersonalData.Any(p => p.Email == value);

                    if (emailInUse)
                        context.AddFailure("Email", "Podany email jest już użyty przez innego użytkownika");
                });

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches("^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\\s\\./0-9]*$");

            RuleFor(x => x.PostalCode)
                .NotEmpty();

            RuleFor(x => x.Town)
                .NotEmpty();

            RuleFor(x => x.Street)
                .NotEmpty();

            RuleFor(x => x.HouseNumber)
                .NotEmpty();

            RuleFor(x => x.FlatNumber);
                        
        }
    }
}
