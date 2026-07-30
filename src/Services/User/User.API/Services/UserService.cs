using User.API.Repositories;
using UserEntity = User.API.Models.User;

namespace User.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<IEnumerable<UserEntity>> GetAllAsync()
        => _userRepository.GetAllAsync();

    public Task<UserEntity?> GetByIdAsync(Guid userId)
        => _userRepository.GetByIdAsync(userId);

    public Task<UserEntity> CreateAsync(UserEntity user)
        => _userRepository.AddAsync(user);

    public async Task<UserEntity?> UpdateAsync(Guid userId, UserEntity user)
    {
        user.UserId = userId;
        return await _userRepository.UpdateAsync(user);
    }

    public Task<bool> DeleteAsync(Guid userId)
        => _userRepository.DeleteAsync(userId);
}
