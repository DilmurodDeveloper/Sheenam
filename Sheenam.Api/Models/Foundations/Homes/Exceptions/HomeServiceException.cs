// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Homes.Exceptions
{
    public class HomeServiceException : Xeption
    {
        public HomeServiceException(Xeption innerException)
            : base(message: "Home service error occurred, contact support.",
                  innerException)
        { }
    }
}
