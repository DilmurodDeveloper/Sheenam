﻿// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidGuestId = Guid.Empty;
            var invalidGuestException = new InvalidGuestException();

            invalidGuestException.AddData(
                key: nameof(Guest.Id),
                values: "Id is required");

            var expectedGuestValidationException =
                new GuestValidationException(invalidGuestException);

            // when
            ValueTask<Guest> removeGuestByIdTask =
                this.guestService.RemoveGuestByIdAsync(invalidGuestId);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(() =>
                    removeGuestByIdTask.AsTask());

            // then
            actualGuestValidationException.Should()
                .BeEquivalentTo(expectedGuestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuestAsync(It.IsAny<Guest>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveByIdIfGuestNotFoundAndLogItAsync()
        {
            // given
            Guid randomGuestId = Guid.NewGuid();
            Guid inputGuestId = randomGuestId;
            Guest nullGuest = null;
            var notFoundGuestException = new NotFoundGuestException(inputGuestId);

            var expectedGuestValidationException =
                new GuestValidationException(notFoundGuestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(inputGuestId))
                    .ReturnsAsync(nullGuest);

            // when
            ValueTask<Guest> removeGuestByIdTask =
                this.guestService.RemoveGuestByIdAsync(inputGuestId);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(() =>
                    removeGuestByIdTask.AsTask());

            // then
            actualGuestValidationException.Should()
                .BeEquivalentTo(expectedGuestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(inputGuestId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuestAsync(It.IsAny<Guest>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
