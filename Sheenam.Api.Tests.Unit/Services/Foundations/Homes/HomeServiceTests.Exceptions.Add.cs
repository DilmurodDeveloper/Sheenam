// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Homes;
using Sheenam.Api.Models.Foundations.Homes.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Homes
{
    public partial class HomeServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Home someHome = CreateRandomHome();
            SqlException sqlException = GetSqlError();
            var failedHomeStorageException = new FailedHomeStorageException(sqlException);

            var expectedHomeDependencyException =
                new HomeDependencyException(failedHomeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertHomeAsync(someHome))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Home> addHomeTask =
                this.homeService.AddHomeAsync(someHome);

            // then
            await Assert.ThrowsAsync<HomeDependencyException>(() =>
                addHomeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHomeAsync(someHome),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHomeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            Home someHome = CreateRandomHome();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistHomeException =
                new AlreadyExistHomeException(duplicateKeyException);

            var expectedHomeDependencyValidationException =
                new HomeDependencyValidationException(alreadyExistHomeException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertHomeAsync(someHome))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Home> addHomeTask =
                this.homeService.AddHomeAsync(someHome);

            // then
            await Assert.ThrowsAsync<HomeDependencyValidationException>(() =>
                addHomeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHomeAsync(someHome),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHomeDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Home someHome = CreateRandomHome();
            var serviceException = new Exception();

            var failedHomeServiceException =
                new FailedHomeServiceException(serviceException);

            var expectedHomeServiceException =
                new HomeServiceException(failedHomeServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertHomeAsync(someHome))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Home> addHomeTask =
                this.homeService.AddHomeAsync(someHome);

            // then
            await Assert.ThrowsAsync<HomeServiceException>(() =>
                addHomeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHomeAsync(someHome),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHomeServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
