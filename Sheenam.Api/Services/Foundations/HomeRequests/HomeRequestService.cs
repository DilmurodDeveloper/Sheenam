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

        public IQueryable<HomeRequest> RetrieveAllHomeRequests() =>
            TryCatch(() => this.storageBroker.SelectAllHomeRequests());

        public async ValueTask<HomeRequest> RetrieveHomeRequestByIdAsync(Guid homeRequestId)
        {
            try
            {
                ValidateHomeRequestId(homeRequestId);

                HomeRequest maybeHomeRequest =
                    await this.storageBroker.SelectHomeRequestByIdAsync(homeRequestId);

                ValidateStorageHomeRequestIsNotNull(maybeHomeRequest, homeRequestId);

                return maybeHomeRequest;
            }
            catch (InvalidHomeRequestException invalidHomeRequestException)
            {
                var homeRequestValidationException =
                    new HomeRequestValidationException(invalidHomeRequestException);

                this.loggingBroker.LogError(homeRequestValidationException);

                throw homeRequestValidationException;
            }
            catch (NotFoundHomeRequestException notFoundHomeRequestException)
            {
                var homeRequestValidationException =
                    new HomeRequestValidationException(notFoundHomeRequestException);

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
            catch (Exception exception)
            {
                var failedHomeRequestServiceException =
                    new FailedHomeRequestServiceException(exception);

                var homeRequestServiceException =
                    new HomeRequestServiceException(failedHomeRequestServiceException);

                this.loggingBroker.LogError(homeRequestServiceException);

                throw homeRequestServiceException;
            }
        }
    }
}
