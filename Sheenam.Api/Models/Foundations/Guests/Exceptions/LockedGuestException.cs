// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class LockedGuestException : Xeption
    {
        public LockedGuestException(Exception innerException)
            : base(message: "Guest is locked, try again later.",
                  innerException)
        { }
    }
}
