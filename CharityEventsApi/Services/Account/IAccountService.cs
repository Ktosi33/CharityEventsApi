﻿using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.Account
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginUserDto dto);
    }
}