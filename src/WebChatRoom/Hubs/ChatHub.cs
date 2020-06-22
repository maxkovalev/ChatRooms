using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        /// <summary>
        /// Send message to all participants in the room
        /// </summary>
        /// <param name="roomId">The room id</param>
        /// <param name="user">The user name who sending a message</param>
        /// <param name="message">The message</param>
        /// <returns></returns>
        public async Task SendMessage(string roomId, string user, string message)
        {
            await Clients.Group(roomId).SendAsync("ReceiveMessage", user, message);
        }

        /// <summary>
        /// Join a room, called when roon page is loaded in browser
        /// </summary>
        /// <param name="roomId">The roon id</param>
        /// <returns></returns>
        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        /// <summary>
        /// For room typing indication 
        /// </summary>
        /// <param name="roomId">The room id</param>
        /// <param name="user">The user name, who is typing message</param>
        /// <returns></returns>
        public async Task ParticipantTyping(string roomId, string user)
        {
            await Clients.GroupExcept(roomId, Context.ConnectionId).SendAsync("Typing", user);
        }
        
    }
}