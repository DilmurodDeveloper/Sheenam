﻿// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Homes;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Homes
{
    public partial class HomeServiceTests
    {
        [Fact]
        public async Task ShouldModifyHomeAsync()
        {
            // given
            Home randomHome = CreateRandomHome();
            Home inputHome = randomHome;
            Home storageHome = inputHome.DeepClone();
            Home updatedHome = inputHome;
            Home expectedHome = updatedHome.DeepClone();
            Guid InputHomeId = inputHome.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHomeByIdAsync(InputHomeId))
                    .ReturnsAsync(storageHome);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateHomeAsync(inputHome))
                    .ReturnsAsync(updatedHome);

            // when
            Home actualHome = await this.homeService.ModifyHomeAsync(inputHome);

            // then
            actualHome.Should().BeEquivalentTo(expectedHome);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeByIdAsync(InputHomeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHomeAsync(inputHome),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
