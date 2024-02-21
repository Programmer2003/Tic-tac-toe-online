using System.Numerics;

namespace Taks7_ttt.Models
{
    public class TicTacToeGame : Game
    {
        private readonly int[] field = new int[9];
        private int movesLeft = 9;
        public TicTacToeGame() : base()
        {
            for (var i = 0; i < field.Length; i++)
            {
                field[i] = -1;
            }
        }

        private void PlacePlayerNumber(int player, int position)
        {
            this.movesLeft -= 1;

            if (this.movesLeft <= 0)
            {
                this.IsOver = true;
                this.IsDraw = true;
            }

            if (position < field.Length && field[position] == -1)
            {
                field[position] = player;
            }
        }

        public override bool Play(int player, int position)
        {
            if (this.IsOver)
            {
                return false;
            }

            this.PlacePlayerNumber(player, position);
            return this.CheckWinner();
        }

        protected override bool CheckWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                if (((field[i * 3] != -1 && field[(i * 3)] == field[(i * 3) + 1] && field[(i * 3)] == field[(i * 3) + 2]) ||
                     (field[i] != -1 && field[i] == field[i + 3] && field[i] == field[i + 6])))
                {
                    this.IsOver = true;
                    return true;
                }
            }

            if ((field[0] != -1 && field[0] == field[4] && field[0] == field[8]) || (field[2] != -1 && field[2] == field[4] && field[2] == field[6]))
            {
                this.IsOver = true;
                return true;
            }

            return false;
        }

        public override int Number()
        {
            return 0;
        }
        public override string GetName()
        {
            return "Tic Tac Toe";
        }

        public override object GetMovementData(int position, int move, bool forFirstPlayer = true)
        {
            var type = move == 0 ? "mark--x" : "mark--o";
            return new { position = $"ttt-tile-{position}", type };
        }
    }
}
