// = = = = = = = = = = = = = = = = = = = = = = = = = 
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
    }
}
