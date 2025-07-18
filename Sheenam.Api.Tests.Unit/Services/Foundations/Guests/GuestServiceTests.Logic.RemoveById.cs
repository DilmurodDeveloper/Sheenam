﻿// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldRemoveGuestByIdAsync()
        {
            // given
            Guid randomGuestId = Guid.NewGuid();
            Guid inputGuestId = randomGuestId;
            Guest randomGuest = CreateRandomGuest();
            Guest storageGuest = randomGuest;
            Guest expectedInputGuest = storageGuest;
            Guest deleteGuest = expectedInputGuest;
            Guest expectedGuest = deleteGuest.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(inputGuestId))
                    .ReturnsAsync(storageGuest);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteGuestAsync(expectedInputGuest))
                    .ReturnsAsync(deleteGuest);

            // when
            Guest actualGuest =
                await this.guestService.RemoveGuestByIdAsync(inputGuestId);

            // then
            actualGuest.Should().BeEquivalentTo(expectedGuest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(inputGuestId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuestAsync(expectedInputGuest),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
