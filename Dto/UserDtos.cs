using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace MyChatApi.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class UserRegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}