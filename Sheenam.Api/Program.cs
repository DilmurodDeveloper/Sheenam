// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Microsoft.OpenApi.Models;
using Sheenam.Api.Brokers.DateTimes;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Services.Foundations.Guests;
using Sheenam.Api.Services.Foundations.HomeRequests;
using Sheenam.Api.Services.Foundations.Hosts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<StorageBroker>();
builder.Services.AddTransient<IStorageBroker, StorageBroker>();
builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();
builder.Services.AddTransient<IDateTimeBroker, DateTimeBroker>();
builder.Services.AddTransient<IGuestService, GuestService>();
builder.Services.AddTransient<IHomeRequestService, HomeRequestService>();
builder.Services.AddTransient<IHostService, HostService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sheenam.Api",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Dilmurod Madirimov",
            Email = "dilmuroddev02@gmail.com",
            Url = new Uri("https://github.com/DilmurodDeveloper")
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            url: "/swagger/v1/swagger.json",
            name: "Sheenam.Api v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();