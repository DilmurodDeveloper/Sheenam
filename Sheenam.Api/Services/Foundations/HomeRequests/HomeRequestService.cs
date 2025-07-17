// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Microsoft.Data.SqlClient;
using Sheenam.Api.Brokers.DateTimes;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.HomeRequests;
using Sheenam.Api.Models.Foundations.HomeRequests.Exceptions;

namespace Sheenam.Api.Services.Foundations.HomeRequests
{
    public partial class HomeRequestService : IHomeRequestService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public HomeRequestService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<HomeRequest> AddHomeRequestAsync(HomeRequest homeRequest) =>
        TryCatch(async () =>
        {
            ValidateHomeRequestOnAdd(homeRequest);

            return await this.storageBroker.InsertHomeRequestAsync(homeRequest);
        });

        public IQueryable<HomeRequest> RetrieveAllHomeRequests()
        {
            try
            {
                return this.storageBroker.SelectAllHomeRequests();
            }
            catch (SqlException sqlException)
            {
                var failedHomeRequestStorageException =
                    new FailedHomeRequestStorageException(sqlException);

                var homeRequestDependencyException =
                    new HomeRequestDependencyException(failedHomeRequestStorageException);

                this.loggingBroker.LogCritical(homeRequestDependencyException);

                throw homeRequestDependencyException;
            }
        }
    }
}
