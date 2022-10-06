namespace Minesweeper
{
    public class Field
    {
        public Field(int width, int height, int mines)
        {
            Width = width;
            Height = height;
            Mines = mines;
            Tiles = new Tile[width, height];
            MineField = new bool[width, height];
            while (mines > 0)
            {
                var rng = new Random();
                var x = rng.Next(0, width);
                var y = rng.Next(0, height);
                if (MineField[x, y] == false)
                {
                    MineField[x, y] = true;
                    mines--;
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tiles[x, y] = new Tile(GetNeighborMineCount(x, y), x, y);
                }
            }
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Mines { get; set; }
        public int Flags { get; set; }
        public bool[,] MineField { get; set; }
        public Tile[,] Tiles { get; set; }
        public int GetNeighborMineCount(int x, int y)
        {
            var count = 0;
            for (var i = x - 1; i <= x + 1; i++)
            {
                for (var j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < Width && j >= 0 && j < Height)
                    {
                        if (MineField[i, j])
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }
        public class Tile
        {
            public Tile(int neighborMines, int x, int y)
            {
                NeighborMines = neighborMines;
                X = x;
                Y = y;
            }

            private int NeighborMines { get; set; }

            /// <returns>
            /// The number of mines around this tile. -1 if <see cref="IsRevealed"/> == false.
            /// </returns>
            public int GetNeighborMineCount()
            {
                if (IsRevealed)
                {
                    return NeighborMines;
                }
                else
                {
                    return -1;
                }
            }

            /// <summary>
            /// Whether this tile is revealed.
            /// </summary>
            public bool IsRevealed { get; set; } = false;

            /// <summary>
            /// Whether this tile is flagged.
            /// </summary>
            public bool IsFlagged { get; set; } = false;

            /// <summary>
            /// X coordinate of this tile.
            /// </summary>
            public int X { get; protected set; }

            /// <summary>
            /// Y coordinate of this tile.
            /// </summary>
            public int Y { get; protected set; }


        }

    }
}