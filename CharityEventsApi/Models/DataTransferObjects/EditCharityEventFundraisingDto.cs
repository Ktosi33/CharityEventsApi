﻿namespace CharityEventsApi.Models.DataTransferObjects
{
    public class EditCharityEventFundraisingDto
    {
        public string? FundTarget { get; set; } = null!;
        public decimal? AmountOfMoneyToCollect { get; set; }
    }
}
