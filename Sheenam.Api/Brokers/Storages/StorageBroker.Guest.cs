// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Guest> Guests { get; set; }

        public async ValueTask<Guest> InsertGuestAsync(Guest guest)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Guest> guestEntitytry =
                await broker.Guests.AddAsync(guest);

            await broker.SaveChangesAsync();

            return guestEntitytry.Entity;
        }

        public IQueryable<Guest> SelectAllGuests()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.Guests.AsNoTracking();
        }

        public async ValueTask<Guest> SelectGuestByIdAsync(Guid guestId)
        {
            using var broker = new StorageBroker(this.configuration);

            return await broker.Guests.FindAsync(guestId);
        }

        public async ValueTask<Guest> UpdateGuestAsync(Guest guest)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.Entry(guest).State = EntityState.Modified;
            await broker.SaveChangesAsync();

            return guest;
        }
    }
}