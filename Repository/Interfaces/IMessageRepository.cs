using MyChatApi.Dto;
using MyChatApi.Models;

namespace MyChatApi.Repository.Interfaces
{
    public interface IMessageRepository
    {
        Task<OneToOneMessage> GetByIdAsync(string messageId);
        Task<OneToOneMessage> AddAsync(AddNewMessageDto message);
        Task<List<OneToOneMessage>> GetAllByConversationAsync(string conversationId);

    }
}
