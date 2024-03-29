﻿namespace CharityEventsApi.Models.DataTransferObjects
{
    public class EditAllPersonalDataDto
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Town { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; }
        public string? FlatNumber { get; set; }
    }
}
