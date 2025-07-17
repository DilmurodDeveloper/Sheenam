// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.HomeRequests;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.HomeRequests
{
    public partial class HomeRequestServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveHomeRequestByIdAsync()
        {
            // given
            Guid randomHomeRequestId = Guid.NewGuid();
            Guid inputHomeRequestId = randomHomeRequestId;
            HomeRequest randomHomeRequest = CreateRandomHomeRequest();
            HomeRequest storageHomeRequest = randomHomeRequest;
            HomeRequest expectedHomeRequest = storageHomeRequest;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeRequestByIdAsync(inputHomeRequestId))
                    .ReturnsAsync(storageHomeRequest);

            // when
            HomeRequest actualHomeRequest =
                await this.homeRequestService
                    .RetrieveHomeRequestByIdAsync(inputHomeRequestId);

            // then
            actualHomeRequest.Should().BeEquivalentTo(expectedHomeRequest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeRequestByIdAsync(inputHomeRequestId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
