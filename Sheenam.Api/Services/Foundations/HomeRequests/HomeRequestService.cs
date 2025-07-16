// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using EFxceptions.Models.Exceptions;
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

        public async ValueTask<HomeRequest> AddHomeRequestAsync(HomeRequest homeRequest)
        {
            try
            {
                ValidateHomeRequestOnAdd(homeRequest);

                return await this.storageBroker.InsertHomeRequestAsync(homeRequest);
            }
            catch (NullHomeRequestException nullHomeRequestException)
            {
                var homeRequestValidationException =
                    new HomeRequestValidationException(nullHomeRequestException);

                this.loggingBroker.LogError(homeRequestValidationException);

                throw homeRequestValidationException;
            }
            catch (InvalidHomeRequestException invalidHomeRequestException)
            {
                var homeRequestValidationException =
                    new HomeRequestValidationException(invalidHomeRequestException);

                this.loggingBroker.LogError(homeRequestValidationException);

                throw homeRequestValidationException;
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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistHomeRequestException =
                    new AlreadyExistHomeRequestException(duplicateKeyException);

                var homeRequestDependencyValidationException =
                    new HomeRequestDependencyValidationException(alreadyExistHomeRequestException);

                this.loggingBroker.LogError(homeRequestDependencyValidationException);

                throw homeRequestDependencyValidationException;
            }
        }
    }
}
