﻿// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using System.Text.Json.Serialization;
using Sheenam.Api.Models.Foundations.HomeRequests;
using Host = Sheenam.Api.Models.Foundations.Hosts.Host;

namespace Sheenam.Api.Models.Foundations.Homes
{
    public class Home
    {
        public Guid Id { get; set; }
        public Guid HostId { get; set; }
        public Host Host { get; set; }
        public string Address { get; set; }
        public string AdditionalInfo { get; set; }
        public bool IsVacant { get; set; }
        public bool IsPetAllowed { get; set; }
        public bool IsShared { get; set; }
        public int NumberOfBedrooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public double Area { get; set; }
        public decimal Price { get; set; }
        public HouseType Type { get; set; }
        [JsonIgnore]
        public List<HomeRequest>? HomeRequests { get; set; }
    }
}
