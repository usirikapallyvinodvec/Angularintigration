namespace Angularintigration.Models
{
    public class ChatMessageModel
    {
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
