﻿using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEvent
{
    public class VolunteeringActivation : ActivationBase
    {
        private readonly CharityEventsDbContext dbContext;

        public VolunteeringActivation(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override void active(int VolunteeringId)
        {
            var volunteering = dbContext.Volunteerings.Include(ce => ce.Charityevents).FirstOrDefault(v => v.IdVolunteering == VolunteeringId);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            var charityevent = volunteering.Charityevents.FirstOrDefault();
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventVolunteering doesn't have charity event.");
            }

            if (charityevent.IsActive == 0 || charityevent.IsVerified == 0 || volunteering.IsVerified == 0)
            {
                throw new BadRequestException("You cant active fundraising while charity event isn't active or verified");
            }
            volunteering.IsActive = 1;
            dbContext.SaveChanges();
        }

        protected override void disactive(int VolunteeringId)
        {
            var volunteering = dbContext.Volunteerings.Include(ce => ce.Charityevents).FirstOrDefault(v => v.IdVolunteering == VolunteeringId);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            volunteering.EndEventDate = DateTime.Now;
            volunteering.IsActive = 0;

            var charityevent = volunteering.Charityevents.FirstOrDefault();
            if (charityevent == null)
            {
                throw new BadRequestException("CharityEventVolunteering dont have charity event.");
            }

            if (charityevent.CharityFundraisingIdCharityFundraising == null)
            {
                charityevent.IsActive = 0;
            }
            else
            {
                var cf = dbContext.Charityfundraisings.FirstOrDefault(cf => cf.IdCharityFundraising == charityevent.CharityFundraisingIdCharityFundraising);
                if (cf != null)
                {
                    if (cf.EndEventDate != null)
                    {
                        charityevent.IsActive = 0;
                    }
                }
            }

            dbContext.SaveChanges();

        }
    }
}