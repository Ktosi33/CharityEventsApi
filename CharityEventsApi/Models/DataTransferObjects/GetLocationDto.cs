﻿using CharityEventsApi.Entities;

namespace CharityEventsApi.Models.DataTransferObjects
{
    public class GetLocationDto
    {
        public string PostalCode { get; set; } = null!;
        public string Town { get; set; } = null!;
        public string Street { get; set; } = null!;
    }
}