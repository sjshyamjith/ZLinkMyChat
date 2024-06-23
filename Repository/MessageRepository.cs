using MyChatApi.Dto;
using MyChatApi.Models;
using MyChatApi.Repository.Interfaces;

namespace MyChatApi.Repository
{
    public class MessageRepository : IMessageRepository, IDisposable
    {
        private ZLinkMyChatContext _context;
        public MessageRepository(ZLinkMyChatContext context)
        {
            _context = context;
        }

        public async Task<OneToOneMessage> AddAsync(AddNewMessageDto message)
        {
            OneToOneMessage m= new OneToOneMessage();
            m.SenderId = message.SenderId;
            m.ConversationId = message.ConversationId;
            m.Message = message.Message;
            m.Id = Guid.NewGuid().ToString();
            m.SendDateTime = DateTime.Now;
            await _context.OneToOneMessages.AddAsync(m);
            await _context.SaveChangesAsync();
            return m;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<List<OneToOneMessage>> GetAllByConversationAsync(string conversationId)
        {
            throw new NotImplementedException();
        }

        public Task<OneToOneMessage> GetByIdAsync(string messageId)
        {
            throw new NotImplementedException();
        }
    }
}
