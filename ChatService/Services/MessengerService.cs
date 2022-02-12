using Grpc.Core;

namespace ChatService.Services
{
    public class MessengerService : Messenger.MessengerBase
    {
        private IServerStreamWriter<Message>  _responseStream;
        private MessageHub _messageHub;

        public MessengerService(MessageHub messageHub)
        {
            _messageHub = messageHub;
        }

        public override async Task<Response> Send(Message request, ServerCallContext context)
        {
            await _messageHub.Send(request);
            return new Response { Text = "Success"};
        }

        public override async Task Join(JoinRequest request, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            string username = request.Username;
            _responseStream = responseStream;
            UserSession session = new UserSession(request.Username);

            try
            {
                session.OnMessageSent += MessengerService_OnMessageSent;
                _messageHub.AddSession(session);
                await Task.Delay(TimeSpan.FromMinutes(30), context.CancellationToken);
            }
            finally
            {
                session.OnMessageSent -= MessengerService_OnMessageSent;
                _messageHub.RemoveSession(username);
            }
        }

        private async void MessengerService_OnMessageSent(Message message)
        {
            await _responseStream.WriteAsync(message);
        }
    }
}
