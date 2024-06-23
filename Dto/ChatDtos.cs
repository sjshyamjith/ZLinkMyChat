namespace MyChatApi.Dto
{
    public class AddNewMessageDto
    {
        public string Message { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
        public string? ConversationId { get; set; }
    }

    public class GETUserConversationDto
    {
        public string ConversationId { get; set; }
        public UserDto Sender { get; set; }
        public UserDto Reciever { get; set; }
        public string LastMessagedOn { get; set; }
        public List<GETChatMessageDto>? ChatMessages { get; set; }

    }

    public class GETChatMessageDto
    {
        public string Id { get; set; }

        public string ConversationId { get; set; }

        public string Message { get; set; }

        public UserDto Sender { get; set; }

        public string SendDateTime { get; set; }
    }
}
