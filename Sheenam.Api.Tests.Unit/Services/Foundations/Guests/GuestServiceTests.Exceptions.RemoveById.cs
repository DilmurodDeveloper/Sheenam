// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveByIdIdDependencyConccurencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someGuestId = Guid.NewGuid();
            Guid inputGuestId = someGuestId;
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedGuestException =
                new LockedGuestException(dbUpdateConcurrencyException);

            var expectedGuestDependencyValidationException =
                new GuestDependencyValidationException(lockedGuestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<Guest> removeGuestByIdTask =
                this.guestService.RemoveGuestByIdAsync(inputGuestId);

            GuestDependencyValidationException actualGuestDependencyValidationException =
                    await Assert.ThrowsAsync<GuestDependencyValidationException>(() =>
                        removeGuestByIdTask.AsTask());

            // then
            actualGuestDependencyValidationException.Should()
                .BeEquivalentTo(expectedGuestDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuestAsync(It.IsAny<Guest>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someGuestId = Guid.NewGuid();
            Guid inputGuestId = someGuestId;
            SqlException sqlException = GetSqlError();

            var failedGuestStorageException =
                new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedGuestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Guest> removeGuestByIdTask =
                this.guestService.RemoveGuestByIdAsync(inputGuestId);

            GuestDependencyException actualGuestDependencyException =
                await Assert.ThrowsAsync<GuestDependencyException>(() =>
                    removeGuestByIdTask.AsTask());

            // then
            actualGuestDependencyException.Should()
                .BeEquivalentTo(expectedGuestDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuestDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someGuestId = Guid.NewGuid();
            Guid inputGuestId = someGuestId;
            var serviceException = new Exception();

            var failedGuestServiceException =
                new FailedGuestServiceException(serviceException);

            var expectedGuestServiceException =
                new GuestServiceException(failedGuestServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Guest> removeGuestByIdTask =
                this.guestService.RemoveGuestByIdAsync(inputGuestId);

            GuestServiceException actualGuestServiceException =
                await Assert.ThrowsAsync<GuestServiceException>(() =>
                    removeGuestByIdTask.AsTask());

            // then
            actualGuestServiceException.Should()
                .BeEquivalentTo(expectedGuestServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
