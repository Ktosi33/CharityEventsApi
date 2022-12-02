using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CharityEventsApi.Models.Validators
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserDtoValidator(CharityEventsDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            RuleFor(x => x.LoginOrEmail)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .Custom((value, context) =>
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.Email == value.LoginOrEmail);
                    user ??= dbContext.Users.FirstOrDefault(u => u.Login == value.LoginOrEmail);

                    if(user != null)
                    {
                        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, value.Password);
                        if (passwordVerificationResult == PasswordVerificationResult.Failed)
                            context.AddFailure("Password", "Niepoprawne hasło");
                    }                  
                    else
                    {
                        context.AddFailure("LoginOrEmail", "Użytkownik o podanym loginie/emailu nie istnieje.");
                    }
                });
        }

    }
}
