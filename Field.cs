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
                    Tiles[x, y] = new Tile(CalculateNeighborMineCount(x, y), x, y);
                }
            }
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Mines { get; set; }
        public int Flags { get; set; }
        public bool[,] MineField { get; set; }
        public Tile[,] Tiles { get; set; }
        public int CalculateNeighborMineCount(int x, int y)
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
        public Tile[,] GenerateHiddenField()
        {
            var hiddenField = new Tile[Width, Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int mines = Tiles[x, y].IsRevealed ? Tiles[x, y].NeighborMines : -1;
                    hiddenField[x, y] = new Tile(mines, x, y);
                    hiddenField[x, y].IsRevealed = Tiles[x, y].IsRevealed;
                    hiddenField[x, y].IsFlagged = Tiles[x, y].IsFlagged;
                }
            }
            return hiddenField;
        }
        public class Tile
        {
            public Tile(int neighborMines, int x, int y)
            {
                NeighborMines = neighborMines;
                X = x;
                Y = y;
            }

            /// <returns>
            /// The number of mines around this tile. -1 if <see cref="IsRevealed"/> == false.
            /// </returns>
            public int NeighborMines { get; set; }

            // public int GetNeighborMineCount()
            // {
            //     if (IsRevealed)
            //     {
            //         return NeighborMines;
            //     }
            //     else
            //     {
            //         return -1;
            //     }
            // }

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

            public override bool Equals(object? obj)
            {
                if (obj is Tile tile)
                {
                    return tile.NeighborMines == NeighborMines && tile.IsRevealed == IsRevealed && tile.IsFlagged == IsFlagged;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(NeighborMines, IsRevealed, IsFlagged);
            }
        }
        public override bool Equals(object? obj)
        {
            if (obj is Field field)
            {
                return field.Width == Width && field.Height == Height && field.Mines == Mines;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height, Mines);
        }
        public static string Stringify(Tile[,] tiles)
        {
            string s = "";
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    var n = tiles[x, y].NeighborMines;
                    s += n == -1 ? " " : n.ToString();
                }
                s += "\n";
            }
            return s;
        }
    }
}