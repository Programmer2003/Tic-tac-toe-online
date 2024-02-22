using SeaBattle;

namespace Taks7_ttt.Models
{
    public class SeaBattleGame : Game
    {
        private GameBoard? FirstBoard { get; set; }
        private GameBoard? SecondBoard { get; set; }
        public SeaBattleGame() : base()
        {
            GetBoards();
            PrepareBoards();
        }

        private void GetBoards()
        {
            FirstBoard = BoardFactory.makeBoard();
            SecondBoard = BoardFactory.makeBoard();
        }

        private void PrepareBoards()
        {
            FirstBoard?.ArrangeShips();
            SecondBoard?.ArrangeShips();
        }
        public override bool MustChangeTurn(int position, bool firstPlayer)
        {
            if (firstPlayer)
            {
                if (SecondBoard == null) return true;
                if (SecondBoard.IsShip(position)) return false;
            }
            else
            {
                if (FirstBoard == null) return true;
                if (FirstBoard.IsShip(position)) return false;
            }

            return true;
        }
        public override bool Play(int player, int position)
        {
            if (this.IsOver)
            {
                return false;
            }

            this.Shoot(player == 0, position);
            return this.CheckWinner();
        }

        private void Shoot(bool first, int position)
        {
            if (first)
            {
                SecondBoard?.Hit(position);
            }
            else
            {
                FirstBoard?.Hit(position);
            }
        }

        protected override bool CheckWinner()
        {
            if (FirstBoard != null && !FirstBoard.HaveShips())
            {
                this.IsOver = true;
                return true;
            }

            if (SecondBoard != null && !SecondBoard.HaveShips())
            {
                this.IsOver = true;
                return true;
            }

            return false;
        }

        public override object? OnStartData(bool forFirstPlayer)
        {
            if (forFirstPlayer) return new { field1 = FirstBoard?.GetJsonField(), field2 = SecondBoard?.GetJsonField() };

            return new { field1 = SecondBoard?.GetJsonField(), field2 = FirstBoard?.GetJsonField() };
        }
        public override int Number() => 1;

        public override string GetName() => "Battle Ship";

        public override object GetMovementData(int position, int move, bool forFirstPlayer = true)
        {
            string type;
            if (move == 0)
            {
                if (SecondBoard != null && SecondBoard.IsShip(position)) type = "ship broken";
                else type = "missed";
            }
            else
            {
                if (FirstBoard != null && FirstBoard.IsShip(position)) type = "ship broken";
                else type = "missed";
            }

            var id = forFirstPlayer ? $"sea-cell-{position}" : $"sea-my-cell-{position}";
            return new { position = id, type };
        }
    }
}
