// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;
using Host = Sheenam.Api.Models.Foundations.Hosts.Host;

namespace Sheenam.Api.Services.Foundations.Hosts
{
    public partial class HostService
    {
        private void ValidateHostOnAdd(Host host)
        {
            ValidateHostNotNull(host);

            Validate(
                (Rule: IsInvalid(host.Id), Parameter: nameof(Host.Id)),
                (Rule: IsInvalid(host.FirstName), Parameter: nameof(Host.FirstName)),
                (Rule: IsInvalid(host.LastName), Parameter: nameof(Host.LastName)),
                (Rule: IsInvalid(host.DateOfBirth), Parameter: nameof(Host.DateOfBirth)),
                (Rule: IsInvalid(host.Email), Parameter: nameof(Host.Email)),
                (Rule: IsInvalid(host.PhoneNumber), Parameter: nameof(Host.PhoneNumber)),
                (Rule: IsInvalid(host.Gender), Parameter: nameof(Host.Gender)));
        }

        private static void ValidateHostOnModify(Host host)
        {
            ValidateHostNotNull(host);

            Validate(
                (Rule: IsInvalid(host.Id), Parameter: nameof(Host.Id)),
                (Rule: IsInvalid(host.FirstName), Parameter: nameof(Host.FirstName)),
                (Rule: IsInvalid(host.LastName), Parameter: nameof(Host.LastName)),
                (Rule: IsInvalid(host.DateOfBirth), Parameter: nameof(Host.DateOfBirth)),
                (Rule: IsInvalid(host.Email), Parameter: nameof(Host.Email)),
                (Rule: IsInvalid(host.PhoneNumber), Parameter: nameof(Host.PhoneNumber)));
        }

        private static void ValidateAgainstStorageHostOnModify(Host inputHost, Host storageHost)
        {
            ValidateStorageHost(storageHost, inputHost.Id);

            Validate(
                (Rule: IsNotSame(
                   firstGuid: inputHost.Id,
                   secondGuid: storageHost.Id,
                   secondDateName: nameof(Host.DateOfBirth)),
                   Parameter: nameof(Host.DateOfBirth)));
        }

        private static void ValidateHostNotNull(Host host)
        {
            if (host is null)
            {
                throw new NullHostException();
            }
        }

        private static void ValidateHostId(Guid hostId) =>
            Validate((Rule: IsInvalid(hostId), Parameter: nameof(Host.Id)));

        private static void ValidateStorageHost(Host maybeHost, Guid hostId)
        {
            if (maybeHost is null)
            {
                throw new NotFoundHostException(hostId);
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
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

        private static dynamic IsInvalid(HostGenderType gender) => new
        {
            Condition = Enum.IsDefined(gender) is false,
            Message = "Value is invalid"
        };

        private static dynamic IsNotSame(
            Guid firstGuid,
            Guid secondGuid,
            string secondDateName) => new
            {
                Condition = firstGuid != secondGuid,
                Message = $"Guid is not same as {secondDateName}"
            };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidHostException = new InvalidHostException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidHostException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidHostException.ThrowIfContainsErrors();
        }
    }
}
