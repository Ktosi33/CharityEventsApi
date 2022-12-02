using CharityEventsApi.Entities;
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


            return new GetDonationDto
            {
                AmountOfDonation = donation.AmountOfDonation,
                Description = donation.Description,
                CharityFundraisingIdCharityFundraising = donation.CharityFundraisingIdCharityFundraising,
                DonationDate = donation.DonationDate,
                UserIdUser = donation.UserIdUser
            };
        }
    }
}
