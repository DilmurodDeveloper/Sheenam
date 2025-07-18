// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Microsoft.Data.SqlClient;
using Sheenam.Api.Brokers.DateTimes;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Homes;
using Sheenam.Api.Models.Foundations.Homes.Exceptions;

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

        public async ValueTask<Home> RetrieveHomeByIdAsync(Guid homeId)
        {
            try
            {
                ValidateHomeId(homeId, "Id");

                Home maybeHome = await this.storageBroker.SelectHomeByIdAsync(homeId);

                ValidateStorageHome(maybeHome, homeId);

                return maybeHome;
            }
            catch (InvalidHomeException invalidHomeException)
            {
                var homeValidationException =
                    new HomeValidationException(invalidHomeException);

                this.loggingBroker.LogError(homeValidationException);

                throw homeValidationException;
            }
            catch (NotFoundHomeException notFoundHomeException)
            {
                var homeValidationException =
                    new HomeValidationException(notFoundHomeException);

                this.loggingBroker.LogError(homeValidationException);

                throw homeValidationException;
            }
            catch (SqlException sqlException)
            {
                var failedHomeStorageException =
                    new FailedHomeStorageException(sqlException);

                var homeDependencyException =
                    new HomeDependencyException(failedHomeStorageException);

                this.loggingBroker.LogCritical(homeDependencyException);

                throw homeDependencyException;
            }
        }
    }
}
