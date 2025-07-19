// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions
{
    public class NotFoundHostException : Xeption
    {
        public NotFoundHostException(Guid hostId)
            : base(message: $"Couldn't find host with id: {hostId}.")
        { }
    }
}
