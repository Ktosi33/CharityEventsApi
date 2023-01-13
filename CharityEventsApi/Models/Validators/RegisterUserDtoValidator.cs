using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;

namespace CharityEventsApi.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(CharityEventsDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);

                    if (emailInUse)
                        context.AddFailure("Email", "Podany email jest już przypisany do konta");
                });

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(e => e.Password)
                .WithMessage("Hasła muszą być takie same");

            RuleFor(x => x.Login)
                .NotEmpty()
                .Custom((value, context) =>
                {
                    if (value.Contains("@"))
                        context.AddFailure("Login", "Login nie może zawierać @");

                    var loginInUse = dbContext.Users.Any(u => u.Login == value);

                    if (loginInUse)
                        context.AddFailure("Login", "Użytkownik o podanym loginie istnieje");
                });          
        }
    }
}
