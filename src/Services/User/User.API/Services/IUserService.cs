using UserEntity = User.API.Models.User;

namespace User.API.Services;

public interface IUserService
{
    Task<IEnumerable<UserEntity>> GetAllAsync();
    Task<UserEntity?> GetByIdAsync(Guid userId);
    Task<UserEntity> CreateAsync(UserEntity user);
    Task<UserEntity?> UpdateAsync(Guid userId, UserEntity user);
    Task<bool> DeleteAsync(Guid userId);
}
