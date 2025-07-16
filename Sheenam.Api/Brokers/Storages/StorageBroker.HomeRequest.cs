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
    }
}