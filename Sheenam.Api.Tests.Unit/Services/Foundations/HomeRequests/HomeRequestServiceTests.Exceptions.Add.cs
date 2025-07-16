// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.HomeRequests;
using Sheenam.Api.Models.Foundations.HomeRequests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.HomeRequests
{
    public partial class HomeRequestServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            HomeRequest someHomeRequest = CreateRandomHomeRequest();
            SqlException sqlException = GetSqlError();

            var failedHomeRequestStorageException =
                new FailedHomeRequestStorageException(sqlException);

            var expectedHomeRequestDependencyException =
                new HomeRequestDependencyException(failedHomeRequestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertHomeRequestAsync(someHomeRequest))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<HomeRequest> addHomeRequestTask =
                this.homeRequestService.AddHomeRequestAsync(someHomeRequest);

            // then
            await Assert.ThrowsAsync<HomeRequestDependencyException>(() =>
                addHomeRequestTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHomeRequestAsync(someHomeRequest),
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
        public async Task ShouldThrowDependencyValidationOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            HomeRequest someHomeRequest = CreateRandomHomeRequest();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistHomeRequestException =
                new AlreadyExistHomeRequestException(duplicateKeyException);

            var expectedHomeRequestDependencyValidationException =
                new HomeRequestDependencyValidationException(alreadyExistHomeRequestException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertHomeRequestAsync(someHomeRequest))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<HomeRequest> addHomeRequestTask =
                this.homeRequestService.AddHomeRequestAsync(someHomeRequest);

            // then
            await Assert.ThrowsAsync<HomeRequestDependencyValidationException>(() =>
                addHomeRequestTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHomeRequestAsync(someHomeRequest),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHomeRequestDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            HomeRequest someHomeRequest = CreateRandomHomeRequest();
            var serviceException = new Exception();

            var failedHomeRequestServiceException =
                new FailedHomeRequestServiceException(serviceException);

            var expectedHomeRequestServiceException =
                new HomeRequestServiceException(failedHomeRequestServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertHomeRequestAsync(someHomeRequest))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HomeRequest> addHomeRequestTask =
                this.homeRequestService.AddHomeRequestAsync(someHomeRequest);

            // then
            await Assert.ThrowsAsync<HomeRequestServiceException>(() =>
                addHomeRequestTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHomeRequestAsync(someHomeRequest),
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
