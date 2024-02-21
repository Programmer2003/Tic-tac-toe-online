using Microsoft.AspNetCore.SignalR;
using Taks7_ttt.Models;
using System.Collections.Concurrent;
using Task7;

namespace Taks7_ttt.Hubs
{
    public class PlayerHub : Hub
    {
        private static ConcurrentBag<Player> players = new ConcurrentBag<Player>();

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public void RegisterPlayer(string name)
        {
            var exists = players.First(p => p.Name == name);
            if (exists != null)
            {
                if (DateTime.Now.Subtract(exists.RegisterTime).Minutes < 20)
                {
                    exists.RegisterTime = DateTime.Now;
                    Clients.Client(Context.ConnectionId).SendAsync(Constants.RegistrationComplete, name);
                }
                else
                {
                    Clients.Client(Context.ConnectionId).SendAsync(Constants.PlayerExists);
                }

                return;
            }

            var player = new Player(name, Context.ConnectionId);
            players.Add(player);
            Clients.Client(Context.ConnectionId).SendAsync(Constants.RegistrationComplete, name);
        }

        private void Remove(Player player)
        {
            players = new ConcurrentBag<Player>(players.Except(new[] { player }));
        }
    }
}
