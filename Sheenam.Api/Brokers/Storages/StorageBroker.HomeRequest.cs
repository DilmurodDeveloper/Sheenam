// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.HomeRequests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<HomeRequest> HomeRequests { get; set; }

        public async ValueTask<HomeRequest> InsertHomeRequestAsync(HomeRequest homeRequest) =>
            await InsertAsync(homeRequest);

        public IQueryable<HomeRequest> SelectAllHomeRequests()
        {
            var homeRequests = SelectAll<HomeRequest>()
                .Include(homeRequest => homeRequest.Guest)
                .Include(homeRequest => homeRequest.Home)
                    .ThenInclude(home => home.Host);

            return homeRequests;
        }

        public async ValueTask<HomeRequest> SelectHomeRequestByIdAsync(Guid homeRequestId)
        {
            var homeRequestWithGuestAndHome = HomeRequests
                .Include(homeRequest => homeRequest.Guest)
                .Include(homeRequest => homeRequest.Home)
                    .ThenInclude(home => home.Host)
                .FirstOrDefault(homeRequest => homeRequest.Id == homeRequestId);

            return await ValueTask.FromResult(homeRequestWithGuestAndHome);
        }

        public async ValueTask<HomeRequest> UpdateHomeRequestAsync(HomeRequest homeRequest) =>
            await UpdateAsync(homeRequest);

        public async ValueTask<HomeRequest> DeleteHomeRequestAsync(HomeRequest homeRequest) =>
            await DeleteAsync(homeRequest);
    }
}