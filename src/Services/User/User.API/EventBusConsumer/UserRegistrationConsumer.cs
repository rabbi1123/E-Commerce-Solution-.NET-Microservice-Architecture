using Eventbus.Messages.Events;
using MassTransit;
using User.API.Repositories;
using UserEntity = User.API.Models.User;

namespace User.API.EventBusConsumer;

public class UserRegistrationConsumer : IConsumer<UserRegistrationEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserRegistrationConsumer> _logger;

    public UserRegistrationConsumer(
        IUserRepository userRepository,
        ILogger<UserRegistrationConsumer> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserRegistrationEvent> context)
    {
        var message = context.Message;

        var existingUser = await _userRepository.GetByIdAsync(message.UserId);
        if (existingUser is not null)
        {
            _logger.LogInformation(
                "User {UserId} already exists. Skipping registration event.",
                message.UserId);
            return;
        }

        await _userRepository.AddAsync(new UserEntity
        {
            UserId = message.UserId,
            Email = message.Email
        });

        _logger.LogInformation(
            "User {UserId} created from registration event.",
            message.UserId);
    }
}
