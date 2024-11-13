using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL;
using OnlineStore.DTO;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly OnlineStoreContext _context;

        public UserService(OnlineStoreContext context)
        {
            _context = context;
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user =
                await _context.Users.FindAsync(userId)
                ?? throw new KeyNotFoundException($"User with ID {userId} not found.");

            return new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
            };
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user =
                await _context.Users.FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new KeyNotFoundException($"User with email {email} not found.");

            return new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
            };
        }

        public async Task RegisterUserAsync(UserCreateDto userDto)
        {
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password, // Consider hashing the password
                Role = UserRole.Customer,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserRoleAsync(int userId, UserRole role)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.Role = role;
                await _context.SaveChangesAsync();
            }
        }
    }
}
