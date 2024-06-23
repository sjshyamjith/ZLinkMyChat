using Microsoft.EntityFrameworkCore;
using MyChatApi.Dto;
using MyChatApi.Models;
using MyChatApi.Repository.Interfaces;

namespace MyChatApi.Repository
{
    public class UserRepository: IUserRepository,IDisposable
    {
        private ZLinkMyChatContext _context;
        public UserRepository(ZLinkMyChatContext context) { 
            _context = context;
        }

        public async Task<User> AddAsync(UserRegisterDto user)
        {
            User data=new User();
            data.Id = Guid.NewGuid().ToString();
            data.Name = user.Name;
            data.Email = user.Email;
            data.Password = user.Password;
            await _context.Users.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            User user = await _context.Users.Where(k => k.Email.Equals(email)).FirstOrDefaultAsync();
            return user;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<List<UserDto>> GetAllUsers(string userId)
        {
            List<UserDto> users = await _context.Users.Where(k=>k.Id!=userId).Select(k=> new UserDto() { Email = k.Email, Id = k.Id, Name = k.Name }).ToListAsync();
            return users;
        }
    }
}
