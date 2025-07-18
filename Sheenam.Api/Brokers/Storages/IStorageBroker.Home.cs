// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Sheenam.Api.Models.Foundations.Homes;

namespace Sheenam.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Home> InsertHomeAsync(Home home);
        IQueryable<Home> SelectAllHomes();
        ValueTask<Home> SelectHomeByIdAsync(Guid homeId);
        ValueTask<Home> UpdateHomeAsync(Home home);
    }
}
