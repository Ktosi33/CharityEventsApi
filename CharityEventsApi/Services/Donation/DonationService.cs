using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.Donation
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
            var donation = new Entities.Donation
            {
                AmountOfDonation = addDonationDto.AmountOfDonation,
                CharityFundraisingIdCharityFundraising = addDonationDto.CharityFundraisingIdCharityFundraising,
                UserIdUser = addDonationDto.UserIdUser,
                Description = addDonationDto.Description,
                DonationDate = addDonationDto.DonationDate
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
    }
}
