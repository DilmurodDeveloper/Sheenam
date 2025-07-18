// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Homes;
using Sheenam.Api.Models.Foundations.Homes.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Homes
{
    public partial class HomeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidHomeId = Guid.Empty;

            var invalidHomeException = new InvalidHomeException();

            invalidHomeException.AddData(
                key: nameof(Home.Id),
                values: "Id is required");

            var expectedHomeValidationException =
                new HomeValidationException(invalidHomeException);

            // when
            ValueTask<Home> removeHomeById =
                this.homeService.RemoveHomeByIdAsync(invalidHomeId);

            HomeValidationException actualHomeValidationException =
                await Assert.ThrowsAsync<HomeValidationException>(() =>
                    removeHomeById.AsTask());

            // then
            actualHomeValidationException.Should()
                .BeEquivalentTo(expectedHomeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHomeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteHomeAsync(It.IsAny<Home>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
