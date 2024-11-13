using OnlineStore.DTO;
using OnlineStore.Models;

namespace OnlineStore.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task RegisterUserAsync(UserCreateDto userDto);
        Task UpdateUserRoleAsync(int userId, UserRole role);
    }
}
