namespace ChatService
{
    public class UserSession
    {
        public UserSession(string username)
        {
            UserName = username;
        }

        public string UserName { get; private set; }

        public event Action<Message> OnMessageSent;

        public void Update(Message message)
        {
            OnMessageSent(message);
        }
    }
}
