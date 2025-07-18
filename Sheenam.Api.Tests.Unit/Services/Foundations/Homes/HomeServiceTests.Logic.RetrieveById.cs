// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Homes;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Homes
{
    public partial class HomeServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveHomeByIdAsync()
        {
            // given
            Guid randomHomeId = Guid.NewGuid();
            Guid inputHomeId = randomHomeId;
            Home randomHome = CreateRandomHome();
            Home storageHome = randomHome;
            Home expectedHome = storageHome;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeByIdAsync(inputHomeId))
                    .ReturnsAsync(storageHome);

            // when
            Home actualHome =
                await this.homeService.RetrieveHomeByIdAsync(inputHomeId);

            // then
            actualHome.Should().BeEquivalentTo(expectedHome);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeByIdAsync(inputHomeId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
