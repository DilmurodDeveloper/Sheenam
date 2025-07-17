// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.HomeRequests;
using Sheenam.Api.Models.Foundations.HomeRequests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.HomeRequests
{
    public partial class HomeRequestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidHomeRequestId = Guid.Empty;
            var invalidHomeRequestException = new InvalidHomeRequestException();

            invalidHomeRequestException.AddData(
                key: nameof(HomeRequest.Id),
                values: "Id is required");

            var expectedHomeRequestValidationException =
                new HomeRequestValidationException(invalidHomeRequestException);

            // when
            ValueTask<HomeRequest> retrieveHomeRequestByIdTask =
                this.homeRequestService.RetrieveHomeRequestByIdAsync(invalidHomeRequestId);

            HomeRequestValidationException actualHomeRequestValidationException =
                await Assert.ThrowsAsync<HomeRequestValidationException>(() =>
                    retrieveHomeRequestByIdTask.AsTask());

            // then
            actualHomeRequestValidationException.Should()
                .BeEquivalentTo(expectedHomeRequestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHomeRequestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeRequestByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfHomeRequestIsNotFoundAndLogItAsync()
        {
            // given
            Guid someHomeRequestId = Guid.NewGuid();
            Guid inputHomeRequestId = someHomeRequestId;
            HomeRequest nullHomeRequest = null;

            var notFoundHomeRequestException =
                new NotFoundHomeRequestException(inputHomeRequestId);

            var expectedHomeRequestValidationException =
                new HomeRequestValidationException(notFoundHomeRequestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeRequestByIdAsync(inputHomeRequestId))
                    .ReturnsAsync(nullHomeRequest);

            // when
            ValueTask<HomeRequest> retrieveHomeRequestByIdTask =
                this.homeRequestService.RetrieveHomeRequestByIdAsync(inputHomeRequestId);

            HomeRequestValidationException actualHomeRequestValidationException =
                await Assert.ThrowsAsync<HomeRequestValidationException>(() =>
                    retrieveHomeRequestByIdTask.AsTask());

            // then
            actualHomeRequestValidationException.Should()
                .BeEquivalentTo(expectedHomeRequestValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeRequestByIdAsync(inputHomeRequestId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHomeRequestValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
