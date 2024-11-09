//=================================================
//Copyright (c) Coalition of Good-Hearted Engineers
//Free To Use To Find Comfort and Peace   
//=================================================

using System;
using System.Threading.Tasks;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public class GuestService : IGuestService
    {
        private readonly IStorageBroker storageBroker;

        public GuestService(IStorageBroker storageBroker) => 
            this.storageBroker = storageBroker;

        public async ValueTask<Guest> AddGuestAsync(Guest guest)
        {
            if (guest == null)
            {
                throw new GuestValidationException(new NullGuestException());
            }

            return await this.storageBroker.InsertGuestAsync(guest);
        }
    }
}
