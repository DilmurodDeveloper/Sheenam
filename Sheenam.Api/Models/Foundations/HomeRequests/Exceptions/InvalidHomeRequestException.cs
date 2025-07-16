// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Xeptions;

namespace Sheenam.Api.Models.Foundations.HomeRequests.Exceptions
{
    public class InvalidHomeRequestException : Xeption
    {
        public InvalidHomeRequestException()
            : base("HomeRequest is invalid.")
        { }
    }
}
