namespace Minesweeper
{
    /// <summary>
    /// A field of tiles.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// A field of tiles.
        /// </summary>
        /// <param name="width">The width of the board</param>
        /// <param name="height">The height of the board</param>
        /// <param name="mines">The amount of mines on the board</param>
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
        /// <summary>
        /// The width of the board
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height of the board
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The amount of mines on the board
        /// </summary>
        public int Mines { get; set; }

        /// <summary>
        /// The amount of flags on the board
        /// </summary>
        public int Flags { get; set; }

        /// <summary>
        /// The array of mine locations
        /// </summary>
        public bool[,] MineField { get; set; }

        /// <summary>
        /// The array of tiles
        /// </summary>
        public Tile[,] Tiles { get; set; }

        /// <summary>
        /// Calculates the amount of mines around a tile
        /// </summary>
        /// <param name="x">The x position of the tile</param>
        /// <param name="y">The y position of the tile</param>
        /// <returns>The amount of mines around the tile</returns>
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

        /// <summary>
        /// Makes a field with -1 as the value of every unrevealed tile
        /// </summary>
        /// <returns>An array of tiles</returns>
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

        /// <summary>
        /// A tile on the field.
        /// </summary>
        public class Tile
        {
            /// <summary>
            /// A tile on the field.
            /// </summary>
            /// <param name="neighborMines">The amount of mines around the tile</param>
            /// <param name="x">The x position of the tile</param>
            /// <param name="y">The y position of the tile</param>
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

        /// <summary>
        /// Turns an array of tiles into a string
        /// </summary>
        /// <param name="tiles">The array of tiles</param>
        /// <returns>A string representation of the array of tiles</returns>
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