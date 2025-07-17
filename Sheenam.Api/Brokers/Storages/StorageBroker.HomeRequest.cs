// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sheenam.Api.Models.Foundations.HomeRequests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<HomeRequest> HomeRequests { get; set; }

        public async ValueTask<HomeRequest> InsertHomeRequestAsync(HomeRequest homeRequest)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<HomeRequest> homeRequestEntityEntry =
                await broker.HomeRequests.AddAsync(homeRequest);

            await broker.SaveChangesAsync();

            return homeRequestEntityEntry.Entity;
        }

        public IQueryable<HomeRequest> SelectAllHomeRequests()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.HomeRequests.AsNoTracking();
        }

        public async ValueTask<HomeRequest> SelectHomeRequestByIdAsync(Guid homeRequestId)
        {
            using var broker = new StorageBroker(this.configuration);

            return await broker.HomeRequests.FindAsync(homeRequestId);
        }

        public async ValueTask<HomeRequest> UpdateHomeRequestAsync(HomeRequest homeRequest)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.Entry(homeRequest).State = EntityState.Modified;
            await broker.SaveChangesAsync();

            return homeRequest;
        }

        public async ValueTask<HomeRequest> DeleteHomeRequestAsync(HomeRequest homeRequest)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<HomeRequest> homeRequestEntityEntry =
                broker.HomeRequests.Remove(homeRequest);

            await broker.SaveChangesAsync();

            return homeRequestEntityEntry.Entity;
        }
    }
}