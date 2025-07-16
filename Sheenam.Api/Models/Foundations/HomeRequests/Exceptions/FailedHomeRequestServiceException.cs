// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Xeptions;

namespace Sheenam.Api.Models.Foundations.HomeRequests.Exceptions
{
    public class FailedHomeRequestServiceException : Xeption
    {
        public FailedHomeRequestServiceException(Exception innerException)
            : base("Failed home request service error occurred, contact support.",
                  innerException)
        { }
    }
}
