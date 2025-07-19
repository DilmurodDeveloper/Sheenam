// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Host randomHost = CreateRandomHost();
            Host someHost = randomHost;
            Guid hostId = someHost.Id;
            SqlException sqlException = GetSqlError();

            var failedHostStorageException =
                new FailedHostStorageException(sqlException);

            var expectedHostDependencyException =
                new HostDependencyException(failedHostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(hostId))
                    .Throws(sqlException);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(someHost);

            HostDependencyException actualHostDependencyException =
                await Assert.ThrowsAsync<HostDependencyException>(() =>
                    modifyHostTask.AsTask());

            // then
            actualHostDependencyException.Should()
                .BeEquivalentTo(expectedHostDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(hostId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(someHost),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Host randomHost = CreateRandomHost();
            Host someHost = randomHost;
            Guid hostId = someHost.Id;
            var databaseUpdateException = new DbUpdateException();

            var failedHostStorageException =
                new FailedHostStorageException(databaseUpdateException);

            var expectedHostDependencyException =
                new HostDependencyException(failedHostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(hostId))
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(someHost);

            HostDependencyException actualHostDependencyException =
                await Assert.ThrowsAsync<HostDependencyException>(() =>
                    modifyHostTask.AsTask());

            // then
            actualHostDependencyException.Should()
                .BeEquivalentTo(expectedHostDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(hostId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(someHost),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Host randomHost = CreateRandomHost();
            Host someHost = randomHost;
            Guid hostId = someHost.Id;
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedHostException =
                new LockedHostException(dbUpdateConcurrencyException);

            var expectedHostDependencyValidationException =
                new HostDependencyValidationException(lockedHostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(hostId))
                    .Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(someHost);

            HostDependencyValidationException actualHostDependencyValidationException =
                await Assert.ThrowsAsync<HostDependencyValidationException>(() =>
                    modifyHostTask.AsTask());

            // then
            actualHostDependencyValidationException.Should()
                .BeEquivalentTo(expectedHostDependencyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(hostId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(someHost),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Host randomHost = CreateRandomHost();
            Host someHost = randomHost;
            Guid hostId = someHost.Id;
            Exception serviceException = new Exception();

            var failedHostServiceException =
                new FailedHostServiceException(serviceException);

            var expectedHostServiceException =
                new HostServiceException(failedHostServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(hostId))
                    .Throws(serviceException);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(someHost);

            HostServiceException actualHostServiceException =
                await Assert.ThrowsAsync<HostServiceException>(() =>
                    modifyHostTask.AsTask());

            // then
            actualHostServiceException.Should()
                .BeEquivalentTo(expectedHostServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(hostId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(someHost),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
