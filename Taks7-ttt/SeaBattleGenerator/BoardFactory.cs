namespace SeaBattle
{
    class BoardFactory
    {
        public static GameBoard makeBoard()
        {
            GameBoard board = new GameBoard(getCells(), getShips());
            return board;
        }

        private static int getCells() => Settings.BOARD_SIZE;

        private static int getShips() => Settings.ONE_DECKERS_COUNT + Settings.TWO_DECKERS_COUNT + Settings.THREE_DECKERS_COUNT + Settings.FOUR_DECKERS_COUNT;
    }
}
