// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions
{
    public class FailedHostServiceException : Xeption
    {
        public FailedHostServiceException(Exception innerException)
            : base(message: "Host service error occurred, contact support.",
                  innerException)
        { }
    }
}
