// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.HomeRequests;
using Sheenam.Api.Models.Foundations.HomeRequests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.HomeRequests
{
    public partial class HomeRequestServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someHomeRequestId = Guid.NewGuid();
            SqlException sqlException = GetSqlError();

            var failedHomeRequestStorageException =
                new FailedHomeRequestStorageException(sqlException);

            var expectedHomeRequestDependencyException =
                new HomeRequestDependencyException(failedHomeRequestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeRequestByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<HomeRequest> retrieveHomeRequestByIdTask =
                this.homeRequestService.RetrieveHomeRequestByIdAsync(someHomeRequestId);

            HomeRequestDependencyException actualHomeRequestDependencyException =
                await Assert.ThrowsAsync<HomeRequestDependencyException>(() =>
                    retrieveHomeRequestByIdTask.AsTask());

            // then
            actualHomeRequestDependencyException.Should()
                .BeEquivalentTo(expectedHomeRequestDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeRequestByIdAsync(someHomeRequestId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHomeRequestDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someHomeRequestId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedHomeRequestServiceException =
                new FailedHomeRequestServiceException(serviceException);

            var expectedHomeRequestServiceException =
                new HomeRequestServiceException(failedHomeRequestServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeRequestByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HomeRequest> retrieveHomeRequestByIdTask =
                this.homeRequestService.RetrieveHomeRequestByIdAsync(someHomeRequestId);

            HomeRequestServiceException actualHomeRequestServiceException =
                await Assert.ThrowsAsync<HomeRequestServiceException>(() =>
                    retrieveHomeRequestByIdTask.AsTask());

            // then
            actualHomeRequestServiceException.Should()
                .BeEquivalentTo(expectedHomeRequestServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeRequestByIdAsync(someHomeRequestId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHomeRequestServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
