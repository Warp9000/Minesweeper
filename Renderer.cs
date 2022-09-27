namespace Minesweeper
{
    public abstract class BaseRenderer
    {
        public abstract void Render(Field.Tile[,] tiles, int remainingMines);

        /// <summary>
        /// Used by the player to highlight a tile.
        /// </summary>
        public abstract void Highlight(int x, int y);
    }
    public class SimpleRenderer : BaseRenderer
    {
        public override void Render(Field.Tile[,] tiles, int remainingMines)
        {
            Console.Clear();
            Console.WriteLine("Remaining mines: " + remainingMines);
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    Console.SetCursorPosition(x, y + 1);
                    var tile = tiles[x, y];
                    if (tile.IsRevealed)
                    {
                        if (tile.GetNeighborMineCount() == 0)
                        {
                            Console.Write(" ");
                        }
                        else
                        {
                            Console.Write(tile.GetNeighborMineCount());
                        }
                    }
                    else if (tile.IsFlagged)
                    {
                        Console.Write("F");
                    }
                    else
                    {
                        Console.Write("#");
                    }
                }
            }
            System.Console.WriteLine();
        }
        public override void Highlight(int x, int y)
        {
            Console.SetCursorPosition(x, y + 1);
        }
    }
    public class FancyRenderer : BaseRenderer
    {
        private Field.Tile[,] Tiles = null!;
        private Dictionary<int, ConsoleColor> Colors = new Dictionary<int, ConsoleColor>()
        {
            { 0, ConsoleColor.Black },
            { 1, ConsoleColor.Blue },
            { 2, ConsoleColor.Green },
            { 3, ConsoleColor.Red },
            { 4, ConsoleColor.DarkBlue },
            { 5, ConsoleColor.DarkRed },
            { 6, ConsoleColor.DarkCyan },
            { 7, ConsoleColor.Black },
            { 8, ConsoleColor.DarkGray }
        };
        public override void Render(Field.Tile[,] t, int remainingMines)
        {
            // Console.Clear();
            Console.CursorVisible = false;
            Tiles = t;
            (int, int) cSize = (Console.WindowWidth, Console.WindowHeight);
#if WINDOWS
            Console.SetBufferSize(cSize.Item1, cSize.Item2);
#endif
            if (cSize.Item1 < (Tiles.GetLength(0) + 1) * 5 || cSize.Item2 < (Tiles.GetLength(1) + 2) * 3)
            {
                while (true)
                {
                    Console.WriteLine("Please resize your console window to at least " + ((Tiles.GetLength(0) + 1) * 5) + "x" + ((Tiles.GetLength(1) + 2) * 3) + ", Current size: " + Console.WindowWidth + "x" + Console.WindowHeight);
                    System.Threading.Thread.Sleep(1000);
                    if (Console.WindowWidth >= (Tiles.GetLength(0) + 1) * 5 && Console.WindowHeight >= (Tiles.GetLength(1) + 2) * 3)
                    {
                        break;
                    }
                }
            }
            (int, int) center = (cSize.Item1 / 2, cSize.Item2 / 2);
            Console.ResetColor();
            Console.SetCursorPosition(center.Item1 - ("Remaining mines: " + remainingMines).Length / 2, 0);
            Console.WriteLine("Remaining mines: " + remainingMines);
            for (int y = 0; y < Tiles.GetLength(1); y++)
            {
                for (int x = 0; x < Tiles.GetLength(0); x++)
                {
                    int n = Tiles[x, y].GetNeighborMineCount();
                    Console.SetCursorPosition(center.Item1 + (x - Tiles.GetLength(0) / 2) * 5, center.Item2 + (y - Tiles.GetLength(1) / 2) * 3);
                    if (Tiles[x, y].IsRevealed)
                    {
                        Console.ForegroundColor = Colors[n];
                        Console.Write("┌───┐");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("│ " + n + " │");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("└───┘");
                    }
                    else if (Tiles[x, y].IsFlagged)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("┌───┐");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("│ F │");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("└───┘");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("┌───┐");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("│   │");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("└───┘");
                    }
                }
            }
        }
        private (int, int) lastHighlight = (-1, -1);
        public override void Highlight(int x, int y)
        {
            Console.CursorVisible = false;
            (int, int) cSize = (Console.WindowWidth, Console.WindowHeight);
            (int, int) center = (cSize.Item1 / 2, cSize.Item2 / 2);
            if (lastHighlight.Item1 != -1)
            {
                try
                {
                    Console.SetCursorPosition(center.Item1 + (lastHighlight.Item1 - Tiles.GetLength(0) / 2) * 5, center.Item2 + (lastHighlight.Item2 - Tiles.GetLength(1) / 2) * 3);
                    if (Tiles[lastHighlight.Item1, lastHighlight.Item2].IsRevealed)
                    {
                        Console.ForegroundColor = Colors[Tiles[lastHighlight.Item1, lastHighlight.Item2].GetNeighborMineCount()];
                        Console.Write("┌───┐");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("│ " + Tiles[lastHighlight.Item1, lastHighlight.Item2].GetNeighborMineCount() + " │");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("└───┘");
                    }
                    else if (Tiles[lastHighlight.Item1, lastHighlight.Item2].IsFlagged)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("┌───┐");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("│ F │");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("└───┘");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("┌───┐");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("│   │");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("└───┘");
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    //ignore
                }
            }
            Console.SetCursorPosition(center.Item1 + (x - Tiles.GetLength(0) / 2) * 5, center.Item2 + (y - Tiles.GetLength(1) / 2) * 3);
            var tile = Tiles[x, y];
            // Console.ForegroundColor = ConsoleColor.Red;
            if (tile.IsRevealed)
            {
                Console.ForegroundColor = Colors[tile.GetNeighborMineCount()];
                Console.Write("╔═══╗");
                Console.CursorTop++;
                Console.CursorLeft -= 5;
                Console.Write("║ " + tile.GetNeighborMineCount() + " ║");
                Console.CursorTop++;
                Console.CursorLeft -= 5;
                Console.Write("╚═══╝");
            }
            else if (tile.IsFlagged)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("╔═══╗");
                Console.CursorTop++;
                Console.CursorLeft -= 5;
                Console.Write("║ F ║");
                Console.CursorTop++;
                Console.CursorLeft -= 5;
                Console.Write("╚═══╝");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("╔═══╗");
                Console.CursorTop++;
                Console.CursorLeft -= 5;
                Console.Write("║   ║");
                Console.CursorTop++;
                Console.CursorLeft -= 5;
                Console.Write("╚═══╝");
            }
            lastHighlight = (x, y);
        }
    }
}