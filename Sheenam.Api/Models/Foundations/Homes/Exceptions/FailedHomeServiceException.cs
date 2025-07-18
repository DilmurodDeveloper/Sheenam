// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Homes.Exceptions
{
    public class FailedHomeServiceException : Xeption
    {
        public FailedHomeServiceException(Exception innerException)
            : base(message: "Home service error occurred, contact support.",
                  innerException)
        { }
    }
}
