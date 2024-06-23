using MyChatApi.Dto;
using MyChatApi.Models;

namespace MyChatApi.Repository.Interfaces
{
    public interface IConverationRepository
    {
        Task<GETUserConversationDto?> GetConversationByIdAsync(string conversationId);
        Task<List<GETUserConversationDto>> GetConversationsAsync(string userId);
        Task<OneToOneConversation> AddAsync(AddNewMessageDto data);

        Task<bool> UpdateLastMessagedOn(string conversationId);
    }
}
