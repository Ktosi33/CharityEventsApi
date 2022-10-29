﻿namespace CharityEventsApi.Models.DataTransferObjects
{
    public class GetCharityFundrasingDto
    {
        //charityeventfundrasing
        public string FundTarget { get; set; } = null!;
        public decimal AmountOfMoneyToCollect { get; set; }
        public decimal AmountOfAlreadyCollectedMoney { get; set; }
        public DateTime CreatedEventDate { get; set; }
        public DateTime? EndEventDate { get; set; }
        public sbyte IsActive { get; set; }
        public sbyte isVerified { get; set; }
    }
}
