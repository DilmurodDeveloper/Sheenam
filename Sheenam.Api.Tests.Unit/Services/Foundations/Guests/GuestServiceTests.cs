//=================================================
//Copyright (c) Coalition of Good-Hearted Engineers
//Free To Use To Find Comfort and Peace   
//=================================================

using Moq;
using Xunit;
using FluentAssertions;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Services.Foundations.Guests;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public class GuestServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IGuestService guestService;

        public GuestServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();

            this.guestService = 
                new GuestService(storageBroker: this.storageBrokerMock.Object);
        }

        [Fact]
        public async Task ShouldAddGuestAsync()
        {
            //Arrange
            Guest randomGuest = new Guest
            {
                Id = Guid.NewGuid(),
                FirstName = "Alex",
                LastName = "Doe",
                Address = "Brooks Str. #12",
                DateOfBirth = new DateTimeOffset(),
                Email = "random@mail.ru",
                Gender = GenderType.Male,
                PhoneNumber = "1234567890"
            };

            this.storageBrokerMock.Setup(broker => 
            broker.InsertGuestAsync(randomGuest))
                .ReturnsAsync(randomGuest);
            //Act
            Guest actual = await this.guestService.AddGuestAsync(randomGuest);
            //Assert
            actual.Should().BeEquivalentTo(randomGuest);
        }
    }
}
