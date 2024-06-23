using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyChatApi.Dto;
using MyChatApi.Repository.Interfaces;

namespace MyChatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private IConverationRepository _conversationRepo;
        private IMessageRepository _messageRepo;
        public ConversationController(IConverationRepository repo, IMessageRepository messageRepo)
        {
            _conversationRepo = repo;
            _messageRepo = messageRepo;
        }

        [HttpPost]
        [Route("message")]
        public async Task<IActionResult> AddNewMessage(AddNewMessageDto message)
        {
            //check conversation exist
            var checkConversation = await _conversationRepo.GetConversationByIdAsync(message.ConversationId);
            if (checkConversation == null)
            {
                //creating conversation
                var conversation = await _conversationRepo.AddAsync(message);
                if (conversation!=null)
                {
                    message.ConversationId = conversation.Id;
                    //add message
                    
                }
            }
            else
            {
                //update conversation
                await _conversationRepo.UpdateLastMessagedOn(message.ConversationId);
            }
            var result = await _messageRepo.AddAsync(message);
            if (result != null)
            {
                GETChatMessageDto m = new GETChatMessageDto()
                {
                    SendDateTime = result.SendDateTime.ToString("G"),
                    ConversationId = result.ConversationId,
                    Id = result.Id,
                    Message = result.Message,
                };
                // success
                return Ok(m);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("all/{userId}")]
        public async Task<IActionResult> GetAllConversations(string userId)
        {
            //check conversation exist
            var result=await _conversationRepo.GetConversationsAsync(userId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }


        [HttpGet]
        [Route("/{conversationId}")]
        public async Task<IActionResult> GetConversationById(string conversationId)
        {
            //check conversation exist
            var result = await _conversationRepo.GetConversationByIdAsync(conversationId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
