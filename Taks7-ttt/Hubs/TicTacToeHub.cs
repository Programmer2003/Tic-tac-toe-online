using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using Taks7_ttt.Models;
using Task7;

namespace Taks7_ttt.Hubs
{
    public class TicTacToeHub : Hub
    {
        static readonly ConcurrentBag<Game> games = new ConcurrentBag<Game>() { };

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public void GetGamesList()
        {
            var list = games
                        .Where(g => g.Player1 != null && !g.IsOver)
                        .Select(g => g.Player1 != null ? g.Player1.Name : "No name");
            Clients.Client(Context.ConnectionId).SendAsync(Constants.GetGames, list);
        }
    }
}
