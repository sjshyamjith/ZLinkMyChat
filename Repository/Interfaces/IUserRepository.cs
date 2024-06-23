using MyChatApi.Dto;
using MyChatApi.Models;

namespace MyChatApi.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserDto>> GetAllUsers(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> AddAsync(UserRegisterDto user);
    }
}
