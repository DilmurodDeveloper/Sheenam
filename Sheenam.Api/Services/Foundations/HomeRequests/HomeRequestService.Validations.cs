﻿// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Sheenam.Api.Models.Foundations.HomeRequests;
using Sheenam.Api.Models.Foundations.HomeRequests.Exceptions;

namespace Sheenam.Api.Services.Foundations.HomeRequests
{
    public partial class HomeRequestService
    {
        private void ValidateHomeRequestOnAdd(HomeRequest homeRequest)
        {
            ValidateHomeRequestIsNotNull(homeRequest);

            Validate(
                (Rule: IsInvalid(homeRequest.Id, "Id"), Parameter: nameof(HomeRequest.Id)),
                (Rule: IsInvalid(homeRequest.GuestId, "Guest Id"), Parameter: nameof(HomeRequest.GuestId)),
                (Rule: IsInvalid(homeRequest.HomeId, "Home Id"), Parameter: nameof(HomeRequest.HomeId)),
                (Rule: IsInvalid(homeRequest.Message), Parameter: nameof(HomeRequest.Message)),
                (Rule: IsInvalid(homeRequest.StartDate), Parameter: nameof(HomeRequest.StartDate)),
                (Rule: IsInvalid(homeRequest.EndDate), Parameter: nameof(HomeRequest.EndDate)),
                (Rule: IsInvalid(homeRequest.CreatedDate), Parameter: nameof(HomeRequest.CreatedDate)),
                (Rule: IsInvalid(homeRequest.UpdatedDate), Parameter: nameof(HomeRequest.UpdatedDate)));
        }

        private static void ValidateHomeRequestId(Guid homeRequestId) =>
            Validate((Rule: IsInvalid(homeRequestId, "Id"), Parameter: nameof(HomeRequest.Id)));

        private void ValidateHomeRequestOnModify(HomeRequest homeRequest)
        {
            ValidateHomeRequestIsNotNull(homeRequest);

            Validate(
                (Rule: IsInvalid(homeRequest.Id, "Id"), Parameter: nameof(HomeRequest.Id)),
                (Rule: IsInvalid(homeRequest.GuestId, "Guest Id"), Parameter: nameof(HomeRequest.GuestId)),
                (Rule: IsInvalid(homeRequest.HomeId, "Home Id"), Parameter: nameof(HomeRequest.HomeId)),
                (Rule: IsInvalid(homeRequest.Message), Parameter: nameof(HomeRequest.Message)),
                (Rule: IsInvalid(homeRequest.StartDate), Parameter: nameof(HomeRequest.StartDate)),
                (Rule: IsInvalid(homeRequest.EndDate), Parameter: nameof(HomeRequest.EndDate)),
                (Rule: IsInvalid(homeRequest.CreatedDate), Parameter: nameof(HomeRequest.CreatedDate)),
                (Rule: IsInvalid(homeRequest.UpdatedDate), Parameter: nameof(HomeRequest.UpdatedDate)));
        }

        private static void ValidateAgainstStorageHomeRequestOnModify(HomeRequest inputHomeRequest, HomeRequest storageHomeRequest)
        {
            ValidateStorageHomeRequestIsNotNull(storageHomeRequest, inputHomeRequest.Id);

            Validate(
                (Rule: IsNotSame(
                    firstGuid: inputHomeRequest.Id,
                    secondGuid: storageHomeRequest.Id,
                    secondFieldName: nameof(HomeRequest.Id)),
                    Parameter: nameof(HomeRequest.Id)));
        }

        private static void ValidateHomeRequestIsNotNull(HomeRequest homeRequest)
        {
            if (homeRequest is null)
            {
                throw new NullHomeRequestException();
            }
        }

        private static void ValidateStorageHomeRequestIsNotNull(
            HomeRequest maybeHomeRequest,
            Guid homeRequestId)
        {
            if (maybeHomeRequest is null)
            {
                throw new NotFoundHomeRequestException(homeRequestId);
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

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
           Guid firstGuid,
           Guid secondGuid,
           string secondFieldName) => new
           {
               Condition = firstGuid != secondGuid,
               Message = $"Value is not the same as {secondFieldName}"
           };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidHomeRequestException = new InvalidHomeRequestException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidHomeRequestException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidHomeRequestException.ThrowIfContainsErrors();
        }
    }
}
