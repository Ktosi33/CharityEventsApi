﻿using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Services.CharityEventService;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.VolunteeringService
{
    public class VolunteeringActivation : BooleanCharityEventQueryBase
    {
        private readonly CharityEventsDbContext dbContext;

        public VolunteeringActivation(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override void setTrue(int VolunteeringId)
        {
            var volunteering = dbContext.CharityVolunteerings.Include(ce => ce.CharityEvents).FirstOrDefault(v => v.IdCharityVolunteering == VolunteeringId);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }

            if (volunteering.IsVerified == 0)
            {
                throw new BadRequestException("You cant active fundraising while charity event isn't verified");
            }
            volunteering.IsActive = 1;
            dbContext.SaveChanges();
        }

        protected override void setFalse(int VolunteeringId)
        {
            var volunteering = dbContext.CharityVolunteerings.Include(ce => ce.CharityEvents).FirstOrDefault(v => v.IdCharityVolunteering == VolunteeringId);
            if (volunteering == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            volunteering.EndEventDate = DateTime.Now;
            volunteering.IsActive = 0;

            var charityevent = volunteering.CharityEvents.FirstOrDefault();
            if (charityevent == null)
            {
                throw new BadRequestException("CharityEventVolunteering dont have charity event.");
            }

            if (charityevent.IdCharityFundraising == null)
            {
                charityevent.IsActive = 0;
            }
            else
            {
                var cf = dbContext.CharityFundraisings.FirstOrDefault(cf => cf.IdCharityFundraising == charityevent.IdCharityFundraising);
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
