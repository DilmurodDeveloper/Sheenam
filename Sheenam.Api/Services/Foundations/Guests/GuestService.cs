// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using System;
using Microsoft.Data.SqlClient;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService : IGuestService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public GuestService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Guest> AddGuestAsync(Guest guest) =>
        TryCatch(async () =>
        {
            ValidateGuestOnAdd(guest);

            return await this.storageBroker.InsertGuestAsync(guest);
        });

        public IQueryable<Guest> RetrieveAllGuests() =>
            TryCatch(() => this.storageBroker.SelectAllGuests());

        public async ValueTask<Guest> RetrieveGuestByIdAsync(Guid guestId)
        {
            try
            {
                ValidateGuestId(guestId);

                Guest maybeGuest =
                    await this.storageBroker.SelectGuestByIdAsync(guestId);

                ValidateStorageGuest(maybeGuest, guestId);

                return maybeGuest;
            }
            catch (InvalidGuestException invalidGuestException)
            {
                var guestValidationException =
                    new GuestValidationException(invalidGuestException);

                this.loggingBroker.LogError(guestValidationException);

                throw guestValidationException;
            }
            catch (NotFoundGuestException notFoundGuestException)
            {
                var guestValidationException =
                    new GuestValidationException(notFoundGuestException);

                this.loggingBroker.LogError(guestValidationException);

                throw guestValidationException;
            }
            catch (SqlException sqlException)
            {
                var failedGuestStorageException =
                    new FailedGuestStorageException(sqlException);

                var guestDependencyException =
                    new GuestDependencyException(failedGuestStorageException);

                this.loggingBroker.LogCritical(guestDependencyException);

                throw guestDependencyException;
            }
        }
    }
}
