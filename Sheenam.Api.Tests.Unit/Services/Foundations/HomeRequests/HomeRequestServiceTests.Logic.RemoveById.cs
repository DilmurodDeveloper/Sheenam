// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.HomeRequests;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.HomeRequests
{
    public partial class HomeRequestServiceTests
    {
        [Fact]
        public async Task ShouldRemoveHomeRequestByIdAsync()
        {
            // given
            Guid randomHomeRequestId = Guid.NewGuid();
            Guid inputHomeRequestId = randomHomeRequestId;
            HomeRequest randomHomeRequest = CreateRandomHomeRequest();
            HomeRequest storageHomeRequest = randomHomeRequest;
            HomeRequest expectedInputHomeRequest = storageHomeRequest;
            HomeRequest deletedHomeRequest = expectedInputHomeRequest;
            HomeRequest expectedHomeRequest = deletedHomeRequest.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeRequestByIdAsync(inputHomeRequestId))
                    .ReturnsAsync(storageHomeRequest);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteHomeRequestAsync(expectedInputHomeRequest))
                    .ReturnsAsync(deletedHomeRequest);

            // when
            HomeRequest actualHomeRequest =
                await this.homeRequestService.RemoveHomeRequestByIdAsync(randomHomeRequestId);

            // then
            actualHomeRequest.Should().BeEquivalentTo(expectedHomeRequest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeRequestByIdAsync(inputHomeRequestId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteHomeRequestAsync(expectedInputHomeRequest),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
