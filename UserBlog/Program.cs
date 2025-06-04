using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using UserBlog;
using UserBlog.Consumer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserBlogDBContext>(options => options.UseSqlServer(connectionString));

var rabbitMQConfig = builder.Configuration.GetSection("RabbitMQ");
builder.Services.AddSingleton<IConnectionFactory>(_ =>
    new ConnectionFactory
    {
        HostName = rabbitMQConfig["HostName"]!,
        UserName = rabbitMQConfig["UserName"]!,
        Password = rabbitMQConfig["Password"]!,
    });

builder.Services.AddSingleton(sp =>
{
    var factory = sp.GetRequiredService<IConnectionFactory>();
    return factory.CreateConnectionAsync().Result;
});

builder.Services.AddSingleton(sp =>
{
    var connection = sp.GetRequiredService<IConnection>();
    return connection.CreateChannelAsync().Result;
});

builder.Services.AddHostedService<UserMessageConsumer>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", async (UserBlogDBContext db, CancellationToken cancellationToken) =>
{
    return await db.Users.ToListAsync(cancellationToken);
});




app.Run();

