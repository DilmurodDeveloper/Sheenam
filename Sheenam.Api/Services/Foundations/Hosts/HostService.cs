// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Brokers.DateTimes;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;
using Host = Sheenam.Api.Models.Foundations.Hosts.Host;

namespace Sheenam.Api.Services.Foundations.Hosts
{
    public partial class HostService : IHostService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public HostService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Host> AddHostAsync(Host host) =>
        TryCatch(async () =>
        {
            ValidateHostOnAdd(host);

            return await this.storageBroker.InsertHostAsync(host);
        });

        public IQueryable<Host> RetrieveAllHosts() =>
            TryCatch(() => this.storageBroker.SelectAllHosts());

        public ValueTask<Host> RetrieveHostByIdAsync(Guid hostId) =>
        TryCatch(async () =>
        {
            ValidateHostId(hostId);

            Host maybeHost =
                await this.storageBroker.SelectHostByIdAsync(hostId);

            ValidateStorageHost(maybeHost, hostId);

            return maybeHost;
        });

        public ValueTask<Host> ModifyHostAsync(Host host) =>
        TryCatch(async () =>
        {
            ValidateHostOnModify(host);

            Host maybeHost =
                await this.storageBroker.SelectHostByIdAsync(host.Id);

            ValidateAgainstStorageHostOnModify(host, maybeHost);

            return await this.storageBroker.UpdateHostAsync(host);
        });

        public async ValueTask<Host> RemoveHostByIdAsync(Guid hostId)
        {
            try
            {
                ValidateHostId(hostId);

                Host maybeHost =
                    await this.storageBroker.SelectHostByIdAsync(hostId);

                ValidateStorageHost(maybeHost, hostId);

                return await this.storageBroker.DeleteHostAsync(maybeHost);
            }
            catch (InvalidHostException invalidHostException)
            {
                var hostValidationException =
                    new HostValidationException(invalidHostException);

                this.loggingBroker.LogError(hostValidationException);

                throw hostValidationException;
            }
            catch (NotFoundHostException notFoundHostException)
            {
                var hostValidationException =
                    new HostValidationException(notFoundHostException);

                this.loggingBroker.LogError(hostValidationException);

                throw hostValidationException;
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedHostException =
                    new LockedHostException(dbUpdateConcurrencyException);

                var hostDependencyValidationException =
                    new HostDependencyValidationException(lockedHostException);

                this.loggingBroker.LogError(hostDependencyValidationException);

                throw hostDependencyValidationException;
            }
            catch (SqlException sqlException)
            {
                var failedHostStorageException =
                    new FailedHostStorageException(sqlException);

                var hostDependencyException =
                    new HostDependencyException(failedHostStorageException);

                this.loggingBroker.LogCritical(hostDependencyException);

                throw hostDependencyException;
            }
        }
    }
}
