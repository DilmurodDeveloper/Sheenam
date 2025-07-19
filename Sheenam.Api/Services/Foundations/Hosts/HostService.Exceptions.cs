// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;
using Xeptions;
using Host = Sheenam.Api.Models.Foundations.Hosts.Host;

namespace Sheenam.Api.Services.Foundations.Hosts
{
    public partial class HostService
    {
        private delegate ValueTask<Host> ReturningHostFunction();
        private delegate IQueryable<Host> ReturningHostsFunction();

        private async ValueTask<Host> TryCatch(ReturningHostFunction returningHostFunction)
        {
            try
            {
                return await returningHostFunction();
            }
            catch (NullHostException nullHostException)
            {
                throw CreateAndLogValidationException(nullHostException);
            }
            catch (InvalidHostException invalidHostException)
            {
                throw CreateAndLogValidationException(invalidHostException);
            }
            catch (SqlException sqlException)
            {
                var failedHostStorageException =
                    new FailedHostStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedHostStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistHostException =
                    new AlreadyExistHostException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistHostException);
            }
            catch (Exception exception)
            {
                var failedHostServiceException =
                    new FailedHostServiceException(exception);

                throw CreateAndLogServiceException(failedHostServiceException);
            }

        }

        private IQueryable<Host> TryCatch(ReturningHostsFunction returningHostsFunction)
        {
            try
            {
                return returningHostsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedHostStorageException =
                    new FailedHostStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedHostStorageException);
            }
            catch (Exception exception)
            {
                var failedHostServiceException =
                    new FailedHostServiceException(exception);

                throw CreateAndLogServiceException(failedHostServiceException);
            }
        }

        private HostValidationException CreateAndLogValidationException(Xeption exception)
        {
            var hostValidationException = new HostValidationException(exception);
            this.loggingBroker.LogError(hostValidationException);

            return hostValidationException;
        }

        private HostDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var hostDependencyException = new HostDependencyException(exception);
            this.loggingBroker.LogCritical(hostDependencyException);

            return hostDependencyException;
        }

        private HostDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var hostDependencyValidationException =
                new HostDependencyValidationException(exception);

            this.loggingBroker.LogError(hostDependencyValidationException);

            return hostDependencyValidationException;
        }

        private HostServiceException CreateAndLogServiceException(Xeption exception)
        {
            var hostServiceException = new HostServiceException(exception);
            this.loggingBroker.LogError(hostServiceException);

            return hostServiceException;
        }
    }
}
