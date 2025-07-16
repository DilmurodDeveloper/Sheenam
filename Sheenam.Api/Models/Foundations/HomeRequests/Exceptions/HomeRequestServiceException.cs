// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Xeptions;

namespace Sheenam.Api.Models.Foundations.HomeRequests.Exceptions
{
    public class HomeRequestServiceException : Xeption
    {
        public HomeRequestServiceException(Xeption innerException)
            : base("Home request service error occurred, contact support.",
                  innerException)
        { }
    }
}
