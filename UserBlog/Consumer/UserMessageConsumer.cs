using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using UserBlog.Models;

namespace UserBlog.Consumer;

public class UserMessageConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IChannel _channel;

    private const string ExchangeName = "user_exchange";
    private const string QueueName = "user_queue";

    public UserMessageConsumer(IServiceScopeFactory serviceScopeFactory, IChannel channel)
    {
        _serviceScopeFactory = serviceScopeFactory;

        _channel = channel;

        _channel.ExchangeDeclareAsync(
            exchange: ExchangeName,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false
            );

        _channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
            );

        _channel.QueueBindAsync(
            queue: QueueName,
            exchange: ExchangeName,
            routingKey: string.Empty
            );
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (obj, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var stringMessage = Encoding.UTF8.GetString(body);

                var message = JsonSerializer.Deserialize<UserMessage>(stringMessage);
                if (message != null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<UserBlogDBContext>();

                    var entity = await db.Users.FindAsync([message.Id], stoppingToken);
                    if (entity is null)
                    {
                        entity = new User
                        {
                            Id = message.Id,
                            Name = message.Name,
                            IsActive = message.IsActive,
                        };

                        await db.Users.AddAsync(entity, stoppingToken);
                    }
                    else
                    {
                        if (message.IsDeleted)
                        {
                            db.Remove(entity);
                        }
                        else
                        {
                            entity.Name = message.Name;
                            entity.IsActive = message.IsActive;

                            db.Update(entity);
                        }
                    }
                    await db.SaveChangesAsync(stoppingToken);
                }

                await _channel.BasicAckAsync(
                    deliveryTag: eventArgs.DeliveryTag,
                    multiple: false,
                    cancellationToken: stoppingToken
                    );
            }
            catch (Exception)
            {
                await _channel.BasicNackAsync(
                    deliveryTag: eventArgs.DeliveryTag,
                    multiple: false,
                    requeue: true,
                    cancellationToken: stoppingToken
                    );
            }
        };

        await _channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken
            );
    }
}