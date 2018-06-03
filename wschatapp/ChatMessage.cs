using System;

namespace wschatapp
{
    public class ChatMessage
    {
        public string User;
        public string Message;
        public DateTime Sent;

        public ChatMessage(string user, string message, DateTime sent)
        {
            User = user;
            Message = message;
            Sent = sent;
        }

        public ChatMessage(string user, string message)
            : this(user, message, DateTime.UtcNow)
        { }

        public ChatMessage()
        { }

        public override string ToString()
        {
            return String.Format(
                "{0} @{1}: {2}",
                User,
                Sent.ToShortTimeString(),
                Message);
        }
    }
}
