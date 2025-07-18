// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Homes;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Home> Homes { get; set; }

        public async ValueTask<Home> InsertHomeAsync(Home home) =>
            await InsertAsync(home);

        public IQueryable<Home> SelectAllHomes() =>
            SelectAll<Home>().Include(home => home.Host);

        public async ValueTask<Home> SelectHomeByIdAsync(Guid homeId)
        {
            var hostWithHomes = Homes
                .Include(home => home.Host)
                .FirstOrDefault(home => home.Id == homeId);

            return await ValueTask.FromResult(hostWithHomes);
        }
    }
}
