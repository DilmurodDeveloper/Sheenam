﻿//=================================================
//Copyright (c) Coalition of Good-Hearted Engineers
//Free To Use To Find Comfort and Peace   
//=================================================

using System;
using Xeptions;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService
    {
        private delegate ValueTask<Guest> ReturningGuestFunction();
        private delegate IQueryable<Guest> ReturningGuestsFunction();

        private async ValueTask<Guest> TryCatch(ReturningGuestFunction returningGuestFunction)
        {
            try
            {
                return await returningGuestFunction();
            }
            catch (NullGuestException nullGuestException)
            {
                throw CreateAndLogValidationException(nullGuestException);
            }
            catch (InvalidGuestException invalidGuestException)
            {
                throw CreateAndLogValidationException(invalidGuestException);
            }
            catch (SqlException sqlException)
            {
                var failedGuestStorageException = 
                    new FailedGuestStorageException(sqlException);
                
                throw CreateAndLogCriticalDependencyException(failedGuestStorageException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var failedGuestStorageException = 
                    new FailedGuestStorageException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(failedGuestStorageException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedStorageGuestException =
                    new FailedGuestStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedStorageGuestException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistGuestException = 
                    new AlreadyExistGuestException(duplicateKeyException);
                
                throw CreateAndLogDependencyValidationException(alreadyExistGuestException);
            }
            catch (NotFoundGuestException notFoundGuestException)
            {
                throw CreateAndLogValidationException(notFoundGuestException);
            }
            catch (Exception exception)
            {
                var failedGuestServiceException = 
                    new FailedGuestServiceException(exception);
                
                throw CreateAndLogServiceException(failedGuestServiceException);
            }
        }

        private IQueryable<Guest> TryCatch(ReturningGuestsFunction returningGuestsFunction)
        {
            try
            {
                return returningGuestsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedGuestStorageException =
                    new FailedGuestStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedGuestStorageException);
            }
            catch (Exception exception)
            {
                var failedGuestServiceException =
                    new FailedGuestServiceException(exception);

                throw CreateAndLogServiceException(failedGuestServiceException);
            }
        }

        private GuestValidationException CreateAndLogValidationException(Xeption exception)
        {
            var guestValidationException = 
                new GuestValidationException(exception);
            
            this.loggingBroker.LogError(guestValidationException); 
            
            return guestValidationException;
        }

        private GuestDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var guestDependencyException = 
                new GuestDependencyException(exception);
            
            this.loggingBroker.LogError(guestDependencyException); 
            
            return guestDependencyException;
        }

        private GuestDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var guestDependencyValidationException =
                new GuestDependencyValidationException(exception);
            
            this.loggingBroker.LogError(guestDependencyValidationException); 
            
            return guestDependencyValidationException;
        }

        private GuestDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var guestDependencyException = 
                new GuestDependencyException(exception);
            
            this.loggingBroker.LogCritical(guestDependencyException);

            return guestDependencyException;
        }

        private GuestServiceException CreateAndLogServiceException(Xeption exception)
        {
            var guestServiceException = 
                new GuestServiceException(exception);
            
            this.loggingBroker.LogError(guestServiceException); 
            
            return guestServiceException;
        }
    }
}
