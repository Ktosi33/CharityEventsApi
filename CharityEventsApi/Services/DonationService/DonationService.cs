﻿using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.DonationService
{
    public class DonationService: IDonationService
    {

        private readonly CharityEventsDbContext dbContext;

        public DonationService(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void addDonation(AddDonationDto addDonationDto)
        {
            var donation = new Donation
            {
                AmountOfDonation = addDonationDto.AmountOfDonation,
                CharityFundraisingIdCharityFundraising = addDonationDto.CharityFundraisingIdCharityFundraising,
                UserIdUser = addDonationDto.UserIdUser,
                Description = addDonationDto.Description,
                DonationDate = DateTime.Now
            };

            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                dbContext.Donations.Add(donation);
                dbContext.SaveChanges();

                var charityFundrasing = dbContext.Charityfundraisings.FirstOrDefault(a => a.IdCharityFundraising == donation.CharityFundraisingIdCharityFundraising);

                if (charityFundrasing != null)
                    charityFundrasing.AmountOfAlreadyCollectedMoney += addDonationDto.AmountOfDonation;

                dbContext.SaveChanges();


                transaction.Commit();
            }             
        }

        public GetDonationDto getDonationById(int donationId)
        {
            var donation = dbContext.Donations.FirstOrDefault(d => d.IdDonations == donationId);
            

            if (donation is null)
                throw new NotFoundException("Donation about this id does not exist");

            var user = dbContext.Users.FirstOrDefault(u => u.IdUser == donation.UserIdUser);

            var don = new GetDonationDto
            {
                DonationId = donationId,
                AmountOfDonation = donation.AmountOfDonation,
                Description = donation.Description,
                CharityFundraisingIdCharityFundraising = donation.CharityFundraisingIdCharityFundraising,
                DonationDate = donation.DonationDate,
                UserIdUser = donation.UserIdUser,
            };

            if (user != null)
                don.User = new GetUserDto
                {
                    IdUser = user.IdUser,
                    Email = user.Email,
                    Login = user.Login
                };

            return don;         
        }

        public List<GetDonationDto> getDonationsByCharityFundraisingId(int fundraisingId)
        {
            var charityFundraising = dbContext.Charityfundraisings
                .Include(c => c.Donations)
                .FirstOrDefault(c => c.IdCharityFundraising == fundraisingId);
            
            if(charityFundraising is null)
                throw new NotFoundException("Charity Fundraising about this id does not exist");

            var donations = charityFundraising.Donations; 

            List<GetDonationDto> donationsList = new List<GetDonationDto>();

            foreach (var donation in donations)
            {
                donationsList.Add(getDonationById(donation.IdDonations));
            }

            return donationsList;

        }
    }
}
