﻿// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Brokers.DateTimes;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Services.Foundations.Hosts;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IHostService hostService;

        public HostServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.hostService = new HostService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static Host CreateRandomHost() =>
            CreateHostFiller(date: GetRandomDateTimeOffset()).Create();

        private IQueryable<Host> CreateRandomHosts()
        {
            return CreateHostFiller(GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber()).AsQueryable();
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(2, 9).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static T GetInvalidEnum<T>()
        {
            int randomNumber = GetRandomNumber();

            while (Enum.IsDefined(typeof(T), randomNumber) is true)
            {
                randomNumber = GetRandomNumber();
            }

            return (T)(object)randomNumber;
        }

        private static SqlException GetSqlError() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(typeof(SqlException));

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static Filler<Host> CreateHostFiller(DateTimeOffset date)
        {
            var filler = new Filler<Host>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(date);

            return filler;
        }
    }
}
