﻿// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using System.Text.Json.Serialization;
using Sheenam.Api.Models.Foundations.HomeRequests;

namespace Sheenam.Api.Models.Foundations.Guests
{
    public class Guest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public GenderType Gender { get; set; }
        [JsonIgnore]
        public List<HomeRequest>? HomeRequests { get; set; }
    }
}
