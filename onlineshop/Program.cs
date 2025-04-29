using Microsoft.EntityFrameworkCore;
using onlineshop.Data;
using onlineshop.Features;
using onlineshop.Filters;
using onlineshop.Middlewares;
using onlineshop.Repositories;
using onlineshop.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CacheResponseActionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMemoryCache();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<RateLimitMiddleware>();
app.UseMiddleware<IdempotencyMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.MapGet("/Cities", async (MyDbContext db, CancellationToken cancellationToken) =>
{
    var entities = await db.Cities.ToListAsync(cancellationToken);
    return BaseResult.Success(entities);
}).WithTags("Cities");

app.Run();
