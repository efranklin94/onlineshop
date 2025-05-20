using Application.Jobs;
using DomainModel.Models.TPC;
using DomainModel.Models.TPH;
using DomainModel.Models.TPT;
using Hangfire;
using Hangfire.MemoryStorage;
using Humanizer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using onlineshop;
using onlineshop.Attributes;
using onlineshop.Behaviours;
using onlineshop.Data;
using onlineshop.Features;
using onlineshop.Middlewares;
using onlineshop.Proxies;
using onlineshop.Repositories;
using onlineshop.Service;
using onlineshop.ViewModels;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMemoryCache();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITrackingCodeProxy, TrackingCodeProxy>();

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    options.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
});

builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));

// Hangfire
builder.Services.AddTransient<UsersTrackingCodeJob>();
builder.Services.AddHangfire(config =>
    config.UseMemoryStorage()
);
builder.Services.AddHangfireServer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<IdempotencyMiddleware>();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<RateLimitMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.MapGet("/Cities", async (MyDbContext db, CancellationToken cancellationToken) =>
{
    var entities = await db.Cities
        // populate country
        .Include(city => city.Country)
        .ToListAsync(cancellationToken);
    
    return BaseResult.Success(entities);
}).WithTags("City");

var enumTypes = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(assembly => assembly.GetTypes())
    .Where(t => t.IsEnum
                && t.Namespace != null
                && t.Namespace.Contains("onlineshop.Enums")
                && t.GetCustomAttributes(typeof(EnumEndpointAttribute), false).Length != 0)
    .ToList();

foreach (var enumType in enumTypes)
{
    var attribute = (enumType.GetCustomAttribute(typeof(EnumEndpointAttribute)) as EnumEndpointAttribute)!;
    var route = attribute.Route;

    app.MapGet(route, () =>
    {
        var enumValues = Enum.GetValues(enumType)
                             .Cast<Enum>()
                             .Select(e => new EnumViewModel
                             {
                                 Id = (int)(object)e,
                                 Title = e.ToString(),
                                 Description = e.Humanize(),
                                 //Populate the Infos
                                 Infos = enumType
                                    .GetMember(e.ToString())[0]
                                    .GetCustomAttributes(typeof(InfoAttribute), false)
                                    .Cast<InfoAttribute>()
                                    .ToDictionary(
                                        attr => attr.Key,
                                        attr => attr.Value
                                    )
                             });
        return BaseResult.Success(enumValues);
    }).WithTags("Enums");
}

app.MapPost("/tpc", async (MyDbContext db, CancellationToken cancellationToken) =>
{
    var shoe = new Shoe { Size = 12, Name = "shima", Price = 23000 };
    var shoe2 = new Shoe { Size = 13, Name = "melli", Price = 22000 };
    var shoe3 = new Shoe { Size = 11, Name = "Nike", Price = 50000 };

    await db.AddAsync(shoe, cancellationToken);
    await db.AddAsync(shoe2, cancellationToken);
    await db.AddAsync(shoe3, cancellationToken);
    await db.SaveChangesAsync(cancellationToken);
});

app.MapPost("/tph", async (MyDbContext db, CancellationToken cancellationToken) =>
{
    var gold = new Gold { Karat = 24 , Name = "parsian24", Price=30000000 };
    var gold2 = new Gold { Karat = 24, Name = "parsian18", Price = 20000000 };
    var gold3 = new Gold { Karat = 24, Name = "parsian19", Price = 10000000 };


    await db.AddAsync(gold, cancellationToken);
    await db.AddAsync(gold2, cancellationToken);
    await db.AddAsync(gold3, cancellationToken);
    await db.SaveChangesAsync(cancellationToken);
});

app.MapPost("/tpt", async (MyDbContext db, CancellationToken cancellationToken) =>
{
    var cellphpne = new CellPhone { Model = "Nokia", Name = "6600", Price=2000 };
    var cellphpne2 = new CellPhone { Model = "sony", Name = "a55", Price = 2000 };
    var cellphpne3 = new CellPhone { Model = "samsung", Name = "a66", Price = 2000 };

    await db.AddAsync(cellphpne, cancellationToken);
    await db.AddAsync(cellphpne2, cancellationToken);
    await db.AddAsync(cellphpne3, cancellationToken);
    await db.SaveChangesAsync(cancellationToken);
});

app.UseHangfireDashboard("/hangfire");

RecurringJob.AddOrUpdate<UsersTrackingCodeJob>(
    "get-users-tracking-code-job",
    job => job.Get(),
    //"*/30 * * * * *"
    Cron.Daily(1)
    );

app.Run();
