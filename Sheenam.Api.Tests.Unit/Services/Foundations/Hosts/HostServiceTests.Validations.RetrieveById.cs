// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidHostId = Guid.Empty;
            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
                key: nameof(Host.Id),
                values: "Id is required");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            // when
            ValueTask<Host> retrieveHostById =
                this.hostService.RetrieveHostByIdAsync(invalidHostId);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(() =>
                    retrieveHostById.AsTask());

            // then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfHostNotFoundAndLogItAsync()
        {
            // given
            Guid someHostId = Guid.NewGuid();
            Host noHost = null;

            var notFoundHostException =
                new NotFoundHostException(someHostId);

            var expectedHostValidationException =
                new HostValidationException(notFoundHostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noHost);

            // when
            ValueTask<Host> retriveByIdHostTask =
                this.hostService.RetrieveHostByIdAsync(someHostId);

            var actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    retriveByIdHostTask.AsTask);

            // then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(someHostId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
