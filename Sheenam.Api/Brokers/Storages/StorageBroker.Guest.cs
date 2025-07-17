// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Guest> Guests { get; set; }

        public async ValueTask<Guest> InsertGuestAsync(Guest guest) =>
            await InsertAsync(guest);

        public IQueryable<Guest> SelectAllGuests()
        {
            var guests = SelectAll<Guest>().Include(guest => guest.HomeRequests);

            return guests;
        }

        public async ValueTask<Guest> SelectGuestByIdAsync(Guid guestId)
        {
            var guestWithHomeRequests = Guests
                .Include(guest => guest.HomeRequests)
                .FirstOrDefault(guest => guest.Id == guestId);

            return await ValueTask.FromResult(guestWithHomeRequests);
        }

        public async ValueTask<Guest> UpdateGuestAsync(Guest guest) =>
            await UpdateAsync(guest);

        public async ValueTask<Guest> DeleteGuestAsync(Guest guest) =>
            await DeleteAsync(guest);
    }
}