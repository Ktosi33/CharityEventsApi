﻿namespace CharityEventsApi.Models.DataTransferObjects
{
    public class GetVolunteerDto
    {
        public int IdUser { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public GetAllPersonalDataDto? allPersonalData { get; set; }

    }
}
