// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Microsoft.EntityFrameworkCore;
using Host = Sheenam.Api.Models.Foundations.Hosts.Host;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Host> Hosts { get; set; }

        public async ValueTask<Host> InsertHostAsync(Host host) =>
            await InsertAsync(host);

        public IQueryable<Host> SelectAllHosts() =>
            SelectAll<Host>().Include(host => host.Homes);

        public async ValueTask<Host> SelectHostByIdAsync(Guid hostId)
        {
            var hostWithHomes = Hosts
                .Include(host => host.Homes)
                .FirstOrDefault(host => host.Id == hostId);

            return await ValueTask.FromResult(hostWithHomes);
        }
    }
}
