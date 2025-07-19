// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllHosts()
        {
            // given
            IQueryable<Host> randomHost = CreateRandomHosts();
            IQueryable<Host> storageHost = randomHost;
            IQueryable<Host> expectedHost = storageHost.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllHosts()).Returns(storageHost);

            // when
            IQueryable<Host> actualHost =
                this.hostService.RetrieveAllHosts();

            // then
            actualHost.Should().BeEquivalentTo(expectedHost);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllHosts(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
