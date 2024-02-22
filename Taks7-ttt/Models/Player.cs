namespace Taks7_ttt.Models
{
    public class Player
    {
        public string Name { get; set; } = "";
        public Player? Opponent { get; set; }
        public bool IsPlaying { get; set; } = false;
        public bool WaitingForMove { get; set; } = false;
        public string ConnectionId { get; set; } = "";

        public Player(string connectionId, string name)
        {
            Name = name;
            ConnectionId = connectionId;
            WaitingForMove = false;
            IsPlaying = false;
        }
    }
}
