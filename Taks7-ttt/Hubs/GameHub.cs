using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Numerics;
using Taks7_ttt.Models;
using Task7;

namespace Taks7_ttt.Hubs
{
    public class GameHub : Hub
    {
        static ConcurrentBag<Game> games = new ConcurrentBag<Game>() { };
        static int players_count = 0;
        private static readonly Random toss = new Random();

        public override Task OnConnectedAsync()
        {
            players_count++;
            UpdateUserCount();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            players_count--;
            UpdateUserCount();

            var game = games.FirstOrDefault(g => g.Player1.ConnectionId == Context.ConnectionId || g.Player2.ConnectionId == Context.ConnectionId);
            if (game == null) return base.OnDisconnectedAsync(exception);

            var leaver = game.Player1.ConnectionId == Context.ConnectionId ? game.Player1 : game.Player2;
            Remove(game);
            if (leaver.Opponent != null)
            {
                return OnOpponentDisconnected(leaver.Opponent.ConnectionId, leaver.Name);
            }
            
            return base.OnDisconnectedAsync(exception);
        }

        public Task OnOpponentDisconnected(string connectionId, string playerName)
        {
            return Clients.Client(connectionId).SendAsync(Constants.Info, "Opponent disconnected");
        }

        public void MakeAMove(int position)
        {
            var game = games.FirstOrDefault(x => x.Player1.ConnectionId == Context.ConnectionId || x.Player2.ConnectionId == Context.ConnectionId);
            if (game == null || game.IsOver) return;

            int move = game.Player1.ConnectionId == Context.ConnectionId ? 0 : 1;
            var player = move == 0 ? game.Player1 : game.Player2;
            if (player.WaitingForMove) return;

            bool forFirstPlayer = move == 0;

            Clients.Client(game.Player1.ConnectionId).SendAsync(Constants.MoveMade, game.GetMovementData(position, move, forFirstPlayer));
            Clients.Client(game.Player2.ConnectionId).SendAsync(Constants.MoveMade, game.GetMovementData(position, move, !forFirstPlayer));

            if (game.Play(move, position))
            {
                if (move == 0)
                {
                    Clients.Client(game.Player1.ConnectionId).SendAsync(Constants.Info, "You win");
                    Clients.Client(game.Player2.ConnectionId).SendAsync(Constants.Info, "You lose(");
                }
                else
                {
                    Clients.Client(game.Player1.ConnectionId).SendAsync(Constants.Info, "You lose(");
                    Clients.Client(game.Player2.ConnectionId).SendAsync(Constants.Info, "You win");
                }

                Clients.Client(game.Player1.ConnectionId).SendAsync(Constants.GameStatus, "Game over");
                Clients.Client(game.Player2.ConnectionId).SendAsync(Constants.GameStatus, "Game over");
                Remove(game);
                return;
            }

            if (game.IsDraw)
            {
                Clients.Client(game.Player1.ConnectionId).SendAsync(Constants.Info, "It's a draw");
                Clients.Client(game.Player2.ConnectionId).SendAsync(Constants.Info, "It's a draw");

                Clients.Client(game.Player1.ConnectionId).SendAsync(Constants.GameStatus, "Game over");
                Clients.Client(game.Player2.ConnectionId).SendAsync(Constants.GameStatus, "Game over");
                Remove(game);
                return;
            }

            if (!game.IsOver)
            {
                if (!game.MustChangeTurn(position, move == 0)) return;

                player.WaitingForMove = !player.WaitingForMove;
                player.Opponent.WaitingForMove = !player.Opponent.WaitingForMove;

                if (player.WaitingForMove)
                {
                    Clients.Client(player.Opponent.ConnectionId).SendAsync(Constants.GameStatus, "It's your move");
                    Clients.Client(player.ConnectionId).SendAsync(Constants.GameStatus, "Waiting for opponent move");
                }
                else
                {
                    Clients.Client(player.Opponent.ConnectionId).SendAsync(Constants.GameStatus, "Waiting for opponent move");
                    Clients.Client(player.ConnectionId).SendAsync(Constants.GameStatus, "It's your move");
                }
            }

        }

        public void GetGames()
        {
            var list = games.Where(g => g.Player1 != null && !g.IsOver)
                            .Select(g => new { g.Id, author = g.FirstPlayerName(), name = g.GetName() });
            Clients.Client(Context.ConnectionId).SendAsync(Constants.GetGames, list);
        }


        public void AddGame(string name, int type)
        {
            if (games.FirstOrDefault(g => g.Player1.ConnectionId == Context.ConnectionId || g.Player1.ConnectionId == Context.ConnectionId) != null) return;

            Game game;
            switch (type)
            {
                case 1:
                    game = new TicTacToeGame();
                    break;
                case 2:
                    game = new SeaBattleGame();
                    break;
                default:
                    game = new TicTacToeGame();
                    break;
            }

            game.Player1 = new Player(Context.ConnectionId, name);
            games.Add(game);
            UpdateGames();
            Clients.Client(Context.ConnectionId).SendAsync(Constants.WaitingForOpponent);
        }

        private void UpdateGames()
        {
            var list = games.Where(g => g.Player1 != null && !g.IsOver)
                            .Select(g => new { g.Id, author = g.FirstPlayerName(), name = g.GetName() });
            Clients.All.SendAsync(Constants.GetGames, list);
        }

        public void JoinGame(int id, string name)
        {
            var game = games.FirstOrDefault(g => g.Id == id);
            if (game == null) //No game
            {
                Clients.Client(Context.ConnectionId).SendAsync(Constants.NoSuchGame);
                return;
            }

            if (game.Player1 == null) //No players in game
            {
                game.Player1 = new Player(Context.ConnectionId, name);
                Clients.Client(Context.ConnectionId).SendAsync(Constants.WaitingForOpponent);
            }
            else //One player is waiting
            {
                game.Player2 = new Player(Context.ConnectionId, name);
                game.Player1.Opponent = game.Player2;
                game.Player2.Opponent = game.Player1;

                Clients.Client(game.Player1.ConnectionId).SendAsync(Constants.OpponentFound, game.Number(), game.OnStartData(true));
                Clients.Client(Context.ConnectionId).SendAsync(Constants.OpponentFound, game.Number(), game.OnStartData(false));

                if (toss.Next(0, 1) == 0)
                {
                    game.Player1.WaitingForMove = false;
                    game.Player2.WaitingForMove = true;

                    Clients.Client(game.Player1.ConnectionId).SendAsync(Constants.GameStatus, "It's your move");
                    Clients.Client(game.Player2.ConnectionId).SendAsync(Constants.GameStatus, "Waiting for opponent move");
                }
                else
                {
                    game.Player1.WaitingForMove = true;
                    game.Player2.WaitingForMove = false;

                    Clients.Client(game.Player2.ConnectionId).SendAsync(Constants.GameStatus, "It's your move");
                    Clients.Client(game.Player1.ConnectionId).SendAsync(Constants.GameStatus, "Waiting for opponent move");
                }
            }
        }

        private void Remove(Game game)
        {
            games = new ConcurrentBag<Game>(games.Except(new[] { game }));
            UpdateGames();
        }

        public void UpdateUserCount()
        {
            Clients.All.SendAsync(Constants.UpdateUsersCount, players_count - 1);
        }
    }
}
