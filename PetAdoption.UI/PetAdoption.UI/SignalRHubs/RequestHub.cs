using Microsoft.AspNetCore.SignalR;

namespace PetAdoption.UI.SignalRHubs
{
    public class RequestHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendRequest(string userId, string userName, int requestId, string petName)
        {
            await Clients.All.SendAsync("PetRequest", userId, userName, petName);
        }
    }
}
