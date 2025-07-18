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
        public void ShouldRetrieveAllHomes()
        {
            // given
            IQueryable<Home> randomHomes = CreateRandomHomes();
            IQueryable<Home> storageHomes = randomHomes;
            IQueryable<Home> expectedHomes = storageHomes;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllHomes())
                    .Returns(storageHomes);

            // when
            IQueryable<Home> actualHomes =
                this.homeService.RetrieveAllHomes();

            // then
            actualHomes.Should().BeEquivalentTo(expectedHomes);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllHomes(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
