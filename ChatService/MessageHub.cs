namespace ChatService
{
    public class MessageHub
    {
        private Dictionary<string, UserSession> _sessions = new Dictionary<string,UserSession>();

        public void AddSession(UserSession session)
        {
            if (!_sessions.ContainsKey(session.UserName))
            {
                _sessions[session.UserName] = session;
            }
        }

        public void RemoveSession(string username)
        {
            if (_sessions.ContainsKey(username))
            {
                _sessions.Remove(username);
            }
        }

        public async Task Send(Message message)
        {
            foreach (var session in _sessions)
            {
                session.Value.Update(message);
            }
        }
    }
}
