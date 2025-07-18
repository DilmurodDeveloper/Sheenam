// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        public ValueTask<Home> RetrieveHomeByIdAsync(Guid homeId) =>
        TryCatch(async () =>
        {
            ValidateHomeId(homeId, "Id");
            Home maybeHome = await this.storageBroker.SelectHomeByIdAsync(homeId);
            ValidateStorageHome(maybeHome, homeId);

            return maybeHome;
        });

        public async ValueTask<Home> ModifyHomeAsync(Home home)
        {
            try
            {
                ValidateHomeOnModify(home);

                Home maybeHome =
                await this.storageBroker.SelectHomeByIdAsync(home.Id);

                ValidateAgainstStorageHomeOnModify(home, maybeHome);

                return await this.storageBroker.UpdateHomeAsync(home);
            }
            catch (NullHomeException nullHomeException)
            {
                var homeValidationException =
                    new HomeValidationException(nullHomeException);

                this.loggingBroker.LogError(homeValidationException);

                throw homeValidationException;
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
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedHomeException =
                new LockedHomeException(dbUpdateConcurrencyException);

                var homeDependencyValidationException =
                    new HomeDependencyValidationException(lockedHomeException);

                this.loggingBroker.LogError(homeDependencyValidationException);

                throw homeDependencyValidationException;
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedHomeStorageException =
                    new FailedHomeStorageException(dbUpdateException);

                var homeDependencyException =
                    new HomeDependencyException(failedHomeStorageException);

                this.loggingBroker.LogError(homeDependencyException);

                throw homeDependencyException;
            }
            catch (Exception exception)
            {
                var failedHomeServiceException =
                new FailedHomeServiceException(exception);

                var homeServiceException =
                    new HomeServiceException(failedHomeServiceException);

                this.loggingBroker.LogError(homeServiceException);

                throw homeServiceException;
            }
        }
    }
}
