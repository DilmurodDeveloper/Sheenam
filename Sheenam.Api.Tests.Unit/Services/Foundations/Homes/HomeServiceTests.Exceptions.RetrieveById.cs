﻿// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Homes;
using Sheenam.Api.Models.Foundations.Homes.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Homes
{
    public partial class HomeServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someHomeId = Guid.NewGuid();
            SqlException sqlException = GetSqlError();

            var failedHomeStorageException =
                new FailedHomeStorageException(sqlException);

            var expectedHomeDependencyException =
                new HomeDependencyException(failedHomeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Home> retrieveHomeById =
                this.homeService.RetrieveHomeByIdAsync(someHomeId);

            HomeDependencyException actualHomeDependencyException =
                await Assert.ThrowsAsync<HomeDependencyException>(() =>
                    retrieveHomeById.AsTask());

            // then
            actualHomeDependencyException.Should()
                .BeEquivalentTo(expectedHomeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeByIdAsync(someHomeId),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHomeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdAsyncIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            Exception serverException = new Exception();

            var failedHomeServiceException =
                new FailedHomeServiceException(serverException);

            var expectedHomeServiceException =
                new HomeServiceException(failedHomeServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ValueTask<Home> retrieveHomeById =
                this.homeService.RetrieveHomeByIdAsync(someId);

            HomeServiceException actualHomeServiceException =
                await Assert.ThrowsAsync<HomeServiceException>(() =>
                    retrieveHomeById.AsTask());

            // then
            actualHomeServiceException.Should().BeEquivalentTo(expectedHomeServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeByIdAsync(someId),
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
