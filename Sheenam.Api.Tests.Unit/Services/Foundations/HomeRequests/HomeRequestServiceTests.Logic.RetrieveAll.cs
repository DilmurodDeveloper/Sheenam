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
        public void ShouldRetrieveAllHomeRequestsAsync()
        {
            // given
            IQueryable<HomeRequest> randomHomeRequests = CreateRandomHomeRequests();
            IQueryable<HomeRequest> storageHomeRequests = randomHomeRequests;
            IQueryable<HomeRequest> expectedHomeRequests = storageHomeRequests;
            
            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllHomeRequests())
                    .Returns(storageHomeRequests);

            // when
            IQueryable<HomeRequest> actualHomeRequests =
                this.homeRequestService.RetrieveAllHomeRequests();

            // then
            actualHomeRequests.Should().BeEquivalentTo(expectedHomeRequests);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllHomeRequests(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
