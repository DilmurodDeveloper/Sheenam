// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sheenam.Api.Models.Foundations.Homes;
using Sheenam.Api.Models.Foundations.Homes.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Homes
{
    public partial class HomeServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Home randomHome = CreateRandomHome();
            Home someHome = randomHome;
            Guid homeId = someHome.Id;
            SqlException sqlException = GetSqlError();

            var failedHomeStorageException =
                new FailedHomeStorageException(sqlException);

            var expectedHomeDependencyException =
                new HomeDependencyException(failedHomeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeByIdAsync(homeId)).Throws(sqlException);

            // when
            ValueTask<Home> modifyHomeTask =
                this.homeService.ModifyHomeAsync(someHome);

            HomeDependencyException actualHomeDependencyException =
                await Assert.ThrowsAsync<HomeDependencyException>(() =>
                    modifyHomeTask.AsTask());

            // then
            actualHomeDependencyException.Should()
                .BeEquivalentTo(expectedHomeDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHomeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeByIdAsync(homeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHomeAsync(someHome),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Home randomHome = CreateRandomHome();
            Home someHome = randomHome;
            Guid homeId = someHome.Id;
            var dbUpdateException = new DbUpdateException();

            var failedHomeStorageException =
                new FailedHomeStorageException(dbUpdateException);

            var expectedHomeDependencyException =
                new HomeDependencyException(failedHomeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeByIdAsync(homeId)).Throws(dbUpdateException);

            // when
            ValueTask<Home> modifyHomeTask =
                this.homeService.ModifyHomeAsync(someHome);

            HomeDependencyException actualHomeDependencyException =
                await Assert.ThrowsAsync<HomeDependencyException>(() =>
                    modifyHomeTask.AsTask());

            // then
            actualHomeDependencyException.Should()
                .BeEquivalentTo(expectedHomeDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHomeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeByIdAsync(homeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHomeAsync(someHome),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Home randomHome = CreateRandomHome();
            Home someHome = randomHome;
            Guid homeId = someHome.Id;
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedHomeException =
                new LockedHomeException(dbUpdateConcurrencyException);

            var expectedHomeDependencyValidationException =
                new HomeDependencyValidationException(lockedHomeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeByIdAsync(homeId))
                    .Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<Home> modifyHomeTask =
                this.homeService.ModifyHomeAsync(someHome);

            HomeDependencyValidationException actualHomeDependencyValidationException =
                await Assert.ThrowsAsync<HomeDependencyValidationException>(() =>
                    modifyHomeTask.AsTask());

            // then
            actualHomeDependencyValidationException.Should()
                .BeEquivalentTo(expectedHomeDependencyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHomeDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeByIdAsync(homeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHomeAsync(someHome),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Home randomHome = CreateRandomHome();
            Home someHome = randomHome;
            Guid homeId = someHome.Id;
            var serviceException = new Exception();

            var failedHomeServiceException =
                new FailedHomeServiceException(serviceException);

            var expectedHomeServiceException =
                new HomeServiceException(failedHomeServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeByIdAsync(homeId))
                    .Throws(serviceException);

            // when
            ValueTask<Home> modifyHomeTask =
                this.homeService.ModifyHomeAsync(someHome);

            HomeServiceException actualHomeServiceException =
                await Assert.ThrowsAsync<HomeServiceException>(() =>
                    modifyHomeTask.AsTask());

            // then
            actualHomeServiceException.Should()
                .BeEquivalentTo(expectedHomeServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHomeServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeByIdAsync(homeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHomeAsync(someHome),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
