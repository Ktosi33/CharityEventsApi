﻿using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;

namespace CharityEventsApi.Models.Validators
{
    public class AddDonationDtoValidator : AbstractValidator<AddDonationDto>
    {
        public AddDonationDtoValidator(CharityEventsDbContext dbContext)
        {
            RuleFor(x => x.AmountOfDonation)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.UserIdUser)
                .NotEmpty()
                .Custom((value, context) =>
                {
                    var userExist = dbContext.Users.Any(u => u.IdUser == value);

                    if (!userExist)
                        context.AddFailure("UserIdUser", "Użytkownik o podanym ID nie istnieje");
                });

            RuleFor(x => x.CharityFundraisingIdCharityFundraising)
                .NotEmpty()
                .Custom((value, context) =>
                {
                    var fundrasingExist = dbContext.Charityfundraisings.Any(c => c.IdCharityFundraising == value);

                    if (!fundrasingExist)
                        context.AddFailure("CharityFundraisingIdCharityFundraising", "Nie istnieje zbiórka pieniędzy o podanym ID");
                });
        }
    }
}