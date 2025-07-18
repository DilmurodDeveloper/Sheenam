// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Sheenam.Api.Models.Foundations.Homes;
using Sheenam.Api.Models.Foundations.Homes.Exceptions;

namespace Sheenam.Api.Services.Foundations.Homes
{
    public partial class HomeService
    {
        private void ValidateHomeOnAdd(Home home)
        {
            ValidateHomeIsNull(home);

            Validate(
                (Rule: IsInvalid(home.Id, "Id"), Parameter: nameof(Home.Id)),
                (Rule: IsInvalid(home.HostId, "Host Id"), Parameter: nameof(Home.HostId)),
                (Rule: IsInvalid(home.Address), Parameter: nameof(Home.Address)),
                (Rule: IsInvalid(home.AdditionalInfo), Parameter: nameof(Home.AdditionalInfo)),

                (Rule: IsInvalid(home.NumberOfBedrooms, "Number of bedrooms"),
                    Parameter: nameof(Home.NumberOfBedrooms)),

                (Rule: IsInvalid(home.NumberOfBathrooms, "Number of bathrooms"),
                    Parameter: nameof(Home.NumberOfBathrooms)),

                (Rule: IsInvalid(home.Area, "Area (square meters)"), Parameter: nameof(Home.Area)),
                (Rule: IsInvalid(home.Price, "Price"), Parameter: nameof(Home.Price)),
                (Rule: IsInvalid(home.Type), Parameter: nameof(Home.Type)));
        }

        private static void ValidateHomeIsNull(Home home)
        {
            if (home is null)
            {
                throw new NullHomeException();
            }
        }

        private static dynamic IsInvalid(Guid Id, string fieldName) => new
        {
            Condition = Id == Guid.Empty,
            Message = $"{fieldName} is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(int number, string fieldName) => new
        {
            Condition = number <= 0,
            Message = $"{fieldName} must be greater than 0"
        };

        private static dynamic IsInvalid(double number, string fieldName) => new
        {
            Condition = number <= 0,
            Message = $"{fieldName} must be greater than 0"
        };

        private static dynamic IsInvalid(decimal number, string fieldName) => new
        {
            Condition = number <= 0,
            Message = $"{fieldName} must be greater than 0"
        };

        private static dynamic IsInvalid(HouseType type) => new
        {
            Condition = Enum.IsDefined(type) is false,
            Message = "Type is required"
        };

        private static void ValidateHomeId(Guid homeId, string fieldName) =>
            Validate((Rule: IsInvalid(homeId, "Id"), Parameter: nameof(Home.Id)));

        private static void ValidateStorageHome(Home maybeHome, Guid homeId)
        {
            if (maybeHome is null)
            {
                throw new NotFoundHomeException(homeId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidHomeException = new InvalidHomeException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidHomeException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidHomeException.ThrowIfContainsErrors();
        }
    }
}
