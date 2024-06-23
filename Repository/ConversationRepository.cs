using Microsoft.EntityFrameworkCore;
using MyChatApi.Dto;
using MyChatApi.Models;
using MyChatApi.Repository.Interfaces;

namespace MyChatApi.Repository
{
    public class ConversationRepository : IConverationRepository, IDisposable
    {
        private ZLinkMyChatContext _context;
        public ConversationRepository(ZLinkMyChatContext context)
        {
            _context = context;
        }
        public async Task<OneToOneConversation> AddAsync(AddNewMessageDto data)
        {
            OneToOneConversation c = new OneToOneConversation();
            c.Id = Guid.NewGuid().ToString();
            c.SenderId = data.SenderId;
            c.RecieverId = data.RecieverId;
            c.LastMessagedOn = DateTime.Now;
            await _context.OneToOneConversations.AddAsync(c);
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task<GETUserConversationDto?> GetConversationByIdAsync(string conversationId)
        {
            return await _context.OneToOneConversations.Where(k => k.Id == conversationId).Select(
                    k=>
                    new GETUserConversationDto()
                    {
                        ConversationId = k.Id,
                        LastMessagedOn=k.LastMessagedOn.ToString("g"),
                        ChatMessages=k.OneToOneMessages.Select(k =>
                            new GETChatMessageDto()
                            {
                                ConversationId = k.ConversationId,
                                Id = k.Id,
                                Message = k.Message,
                                Sender = new UserDto() { Email = k.Sender.Email, Id = k.Sender.Id, Name = k.Sender.Name },
                                SendDateTime = k.SendDateTime.ToString("yyyy-MM-dd HH:mm"),
                            }).ToList(),
                        Reciever=new UserDto() { Email=k.Reciever.Email, Id=k.Reciever.Id, Name=k.Reciever.Name},
                        Sender=new UserDto { Email=k.Sender.Email,Id=k.Sender.Id,Name=k.Sender.Name},
                    }
                ).FirstOrDefaultAsync();
        }

        public async Task<List<GETUserConversationDto>> GetConversationsAsync(string userId)
        {
            List<OneToOneConversation> data = await _context.OneToOneConversations.Where(k => k.SenderId == userId || k.RecieverId == userId).Include(k=>k.Sender).Include(k=>k.OneToOneMessages.OrderBy(k=>k.SendDateTime)).OrderByDescending(k=>k.LastMessagedOn).ToListAsync();
            List<GETUserConversationDto> result = new List<GETUserConversationDto>();
            foreach (var conversation in data) {
                GETUserConversationDto item=new GETUserConversationDto();
                item.ConversationId = conversation.Id;
                item.LastMessagedOn = conversation.LastMessagedOn.ToString("yyyy-MM-dd HH:mm");
                item.Sender=new UserDto() { Email=conversation.Sender.Email,Id=conversation.Sender.Id,Name=conversation.Sender.Name };
                item.Reciever = new UserDto() { Email = conversation.Reciever.Email, Id = conversation.Reciever.Id, Name = conversation.Reciever.Name };
                

                List<GETChatMessageDto> messages = conversation.OneToOneMessages.Select(k =>
                    new GETChatMessageDto()
                    {
                        ConversationId = k.ConversationId,
                        Id = k.Id,
                        Message = k.Message,
                        Sender= new UserDto() { Email = k.Sender.Email, Id = k.Sender.Id, Name = k.Sender.Name },
                        SendDateTime=k.SendDateTime.ToString("yyyy-MM-dd HH:mm"),
                    }).ToList();
                item.ChatMessages = messages;
                result.Add(item);
            }
            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<bool> UpdateLastMessagedOn(string conversationId)
        {
            OneToOneConversation c=await _context.OneToOneConversations.Where(k=>k.Id==conversationId).FirstOrDefaultAsync();
            c.LastMessagedOn=DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
