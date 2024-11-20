// ---------------------------------------------------
// Copyright (c) Coalition of Good-hearted Engineers
// Free to use to find comfort and pease
// ---------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class NotFoundGuestException : Xeption
    {
        public NotFoundGuestException(Guid guestId)
            : base(message: $"Couldn't find job with id: {guestId}")
        { }


    }
}