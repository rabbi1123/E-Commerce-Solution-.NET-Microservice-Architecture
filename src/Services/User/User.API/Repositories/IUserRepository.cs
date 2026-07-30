using UserEntity = User.API.Models.User;

namespace User.API.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<UserEntity>> GetAllAsync();
    Task<UserEntity?> GetByIdAsync(Guid userId);
    Task<UserEntity> AddAsync(UserEntity user);
    Task<UserEntity?> UpdateAsync(UserEntity user);
    Task<bool> DeleteAsync(Guid userId);
}
