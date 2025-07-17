// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sheenam.Api.Models.Foundations.HomeRequests;
using Sheenam.Api.Models.Foundations.HomeRequests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.HomeRequests
{
    public partial class HomeRequestServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someHomeRequestId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedHomeRequestException =
                new LockedHomeRequestException(databaseUpdateConcurrencyException);

            var expectedHomeRequestDependencyValidationException =
                new HomeRequestDependencyValidationException(lockedHomeRequestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeRequestByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<HomeRequest> removeHomeRequestByIdTask =
                this.homeRequestService.RemoveHomeRequestByIdAsync(someHomeRequestId);

            HomeRequestDependencyValidationException actualHomeRequestDependencyValidationException =
                await Assert.ThrowsAsync<HomeRequestDependencyValidationException>(() =>
                    removeHomeRequestByIdTask.AsTask());

            // then
            actualHomeRequestDependencyValidationException.Should()
                .BeEquivalentTo(expectedHomeRequestDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeRequestByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHomeRequestDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteHomeRequestAsync(It.IsAny<HomeRequest>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
