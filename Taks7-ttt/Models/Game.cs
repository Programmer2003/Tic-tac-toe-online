namespace Taks7_ttt.Models
{
    abstract public class Game
    {
        private static int counter = 0;
        public int Id { get; set; }
        public bool IsOver { get; protected set; } = false;
        public bool IsDraw { get; protected set; } = false;
        public Player Player1 { get; set; }

        public Player Player2 { get; set; }
        public Game()
        {
            Id = Interlocked.Increment(ref counter);
            Player1 = new Player("", "");
            Player2 = new Player("", "");
        }

        public string FirstPlayerName()
        {
            return string.IsNullOrWhiteSpace(Player1.Name) ? "No name" : Player1.Name;
        }

        public string SecondPlayerName()
        {
            return string.IsNullOrWhiteSpace(Player2.Name) ? "No name" : Player2.Name;
        }

        abstract public string GetName();
        abstract protected bool CheckWinner();
        abstract public bool Play(int move, int position);

    }
}
