namespace SeaBattle
{
    class Cell
    {
        private Content content;
        private Ship? ship;

        public bool isDeck => content.Type == ContentType.DECK;
        public bool isOccupied { get; set; }
        public bool isBroken { get; set; }
        public Content Content => content;


        public Cell()
        {
            content = new Content();
            isOccupied = false;
            isBroken = false;
        }

        public bool Hit()
        {
            if (!isDeck) return false;

            return (isBroken = true);
        }

        public Content addShip(Ship s)
        {
            ship = s;
            content.SetDeck();
            s.addDeck(content);
            return content;
        }
    }
}
