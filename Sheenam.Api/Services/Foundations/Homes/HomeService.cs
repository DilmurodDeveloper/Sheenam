﻿// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Sheenam.Api.Brokers.DateTimes;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Homes;

namespace Sheenam.Api.Services.Foundations.Homes
{
    public partial class HomeService : IHomeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public HomeService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Home> AddHomeAsync(Home home) =>
        TryCatch(async () =>
        {
            ValidateHomeOnAdd(home);

            return await this.storageBroker.InsertHomeAsync(home);
        });

        IQueryable<Home> IHomeService.RetrieveAllHomes() =>
            TryCatch(() => this.storageBroker.SelectAllHomes());

        public ValueTask<Home> RetrieveHomeByIdAsync(Guid homeId) =>
        TryCatch(async () =>
        {
            ValidateHomeId(homeId, "Id");
            Home maybeHome = await this.storageBroker.SelectHomeByIdAsync(homeId);
            ValidateStorageHome(maybeHome, homeId);

            return maybeHome;
        });

        public ValueTask<Home> ModifyHomeAsync(Home home) =>
        TryCatch(async () =>
        {
            ValidateHomeOnModify(home);

            Home maybeHome =
            await this.storageBroker.SelectHomeByIdAsync(home.Id);

            ValidateAgainstStorageHomeOnModify(home, maybeHome);

            return await this.storageBroker.UpdateHomeAsync(home);
        });

        public ValueTask<Home> RemoveHomeByIdAsync(Guid homeId) =>
        TryCatch(async () =>
        {
            ValidateHomeId(homeId, "Id");

            Home maybeHome =
                await this.storageBroker.SelectHomeByIdAsync(homeId);

            ValidateStorageHome(maybeHome, homeId);

            return await this.storageBroker.DeleteHomeAsync(maybeHome);
        });
    }
}
