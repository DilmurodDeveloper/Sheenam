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
        public async Task ShouldModifyHomeRequestAsync()
        {
            // given
            HomeRequest randomHomeRequest = CreateRandomHomeRequest();
            HomeRequest inputHomeRequest = randomHomeRequest;
            HomeRequest modifiedHomeRequest = inputHomeRequest;
            HomeRequest updatedHomeRequest = inputHomeRequest;
            HomeRequest expectedHomeRequest = updatedHomeRequest;
            Guid inputHomeRequestId = inputHomeRequest.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeRequestByIdAsync(inputHomeRequestId))
                    .ReturnsAsync(modifiedHomeRequest);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateHomeRequestAsync(inputHomeRequest))
                    .ReturnsAsync(updatedHomeRequest);

            // when
            HomeRequest actualHomeRequest =
                await this.homeRequestService.ModifyHomeRequestAsync(inputHomeRequest);

            // then
            actualHomeRequest.Should().BeEquivalentTo(expectedHomeRequest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeRequestByIdAsync(inputHomeRequestId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHomeRequestAsync(inputHomeRequest),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
