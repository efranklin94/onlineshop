using Microsoft.AspNetCore.Mvc;
using TrackingCode.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/TrackingCode/{prefix}/{count}", ([FromRoute] string prefix, [FromRoute] int count = 1) =>
{
    var trackingCodes = Enumerable.Range(0, count)
        .Select(_ => $"{prefix}-{Random.Shared.Next(10000, 99999)}")
        .ToList();

    var result = new GetViewModel { TrackingCodes = trackingCodes };

    return result;
})
.WithName("TrackingCode");

app.Run();
