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
    /// <summary>
    /// Represents a game of Minesweeper.
    /// </summary>
    public class Game
    {
        private Field Field;
        private BaseRenderer Renderer;
        private BasePlayer Player;
        public Game(Tuple<int, int> size, int mines, BasePlayer player)
        {
            Field = new Field(size.Item1, size.Item2, mines);
            Player = player;
            Renderer = player.Renderer;
        }
        public void Run()
        {
            bool firstGame = true;
            while (true)
            {
                if (!Player.selfRender)
                    Renderer.Render(Field.Tiles, Field.Mines - Field.Flags);
                var input = Player.Play(Field.Tiles, Field.Mines);
                if (input.FlagPositions.Count > 0)
                {
                    foreach (var flagPosition in input.FlagPositions)
                    {
                        if (Field.Tiles[flagPosition.Item1, flagPosition.Item2].IsRevealed)
                        {
                            continue;
                        }
                        Field.Tiles[flagPosition.Item1, flagPosition.Item2].IsFlagged = !Field.Tiles[flagPosition.Item1, flagPosition.Item2].IsFlagged;
                        Field.Flags += Field.Tiles[flagPosition.Item1, flagPosition.Item2].IsFlagged ? 1 : -1;
                    }
                }
                if (input.RevealPositions.Count > 0)
                {
                    foreach (var revealPosition in input.RevealPositions)
                    {
                        var tile = Field.Tiles[revealPosition.Item1, revealPosition.Item2];
                        tile.IsRevealed = true;
                        if (Field.MineField[revealPosition.Item1, revealPosition.Item2])
                        {
                            if (firstGame)
                            {
                                Field.MineField[revealPosition.Item1, revealPosition.Item2] = false;
                                var rng = new Random();
                                var x = rng.Next(0, Field.Width);
                                var y = rng.Next(0, Field.Height);
                                Field.MineField[x, y] = true;
                                for (int xx = 0; xx < Field.Width; xx++)
                                {
                                    for (int yy = 0; yy < Field.Height; yy++)
                                    {
                                        Field.Tiles[xx, yy] = new Field.Tile(Field.GetNeighborMineCount(xx, yy), xx, yy);
                                    }
                                }
                                firstGame = false;
                                continue;
                            }
                            if (!Player.selfRender)
                                Renderer.Render(Field.Tiles, Field.Mines);
                            Console.WriteLine("You lost!");
                            return;
                        }
                    }
                }
                if (checkWin())
                {
                    Renderer.Render(Field.Tiles, Field.Mines);
                    Console.WriteLine("You won!");
                    return;
                }
                firstGame = false;
            }
        }
        private bool checkWin()
        {
            for (int y = 0; y < Field.Tiles.GetLength(1); y++)
            {
                for (int x = 0; x < Field.Tiles.GetLength(0); x++)
                {
                    if (Field.Tiles[x, y].IsRevealed == false && Field.MineField[x, y] == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}