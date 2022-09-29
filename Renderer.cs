namespace Minesweeper
{
    public abstract class BaseRenderer
    {
        public abstract void Render(Field.Tile[,] tiles, int remainingMines, bool[,]? mines = null);

        /// <summary>
        /// Used by the player to highlight a tile.
        /// </summary>
        public abstract void Highlight(int x, int y);

        public abstract void Dialog(string message);
    }
    public class SimpleRenderer : BaseRenderer
    {
        public override void Render(Field.Tile[,] tiles, int remainingMines, bool[,]? mines = null)
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
        public override void Dialog(string message)
        {
            Console.WriteLine(message);
        }
    }
    public class FancyRenderer : BaseRenderer
    {
        private Field.Tile[,] Tiles = null!;
        private Dictionary<int, ConsoleColor> Colors = new Dictionary<int, ConsoleColor>()
        {
            { 0, ConsoleColor.White },
            { 1, ConsoleColor.Blue },
            { 2, ConsoleColor.Green },
            { 3, ConsoleColor.Red },
            { 4, ConsoleColor.DarkBlue },
            { 5, ConsoleColor.DarkRed },
            { 6, ConsoleColor.DarkCyan },
            { 7, ConsoleColor.Black },
            { 8, ConsoleColor.DarkGray }
        };
        public override void Render(Field.Tile[,] t, int remainingMines, bool[,]? mines = null)
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
                    if (mines != null && mines[x, y])
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("┌───┐");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("│ * │");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("└───┘");
                        continue;
                    }
                    if (Tiles[x, y].IsRevealed)
                    {
                        if (n == 0)
                        {
                            Console.Write("     ");
                            Console.CursorTop++;
                            Console.CursorLeft -= 5;
                            Console.Write("     ");
                            Console.CursorTop++;
                            Console.CursorLeft -= 5;
                            Console.Write("     ");
                            continue;
                        }
                        Console.ForegroundColor = Colors[n];
                        Console.Write("     ");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("  " + n + "  ");
                        Console.CursorTop++;
                        Console.CursorLeft -= 5;
                        Console.Write("     ");
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
            if (mines == null)
                Highlight(lastHighlight.Item1, lastHighlight.Item2);
        }
        private (int, int) lastHighlight = (-1, -1);
        public override void Highlight(int x, int y)
        {
            if (x == -1 && y == -1)
                return;
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
                        if (Tiles[lastHighlight.Item1, lastHighlight.Item2].GetNeighborMineCount() == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("     ");
                            Console.CursorTop++;
                            Console.CursorLeft -= 5;
                            Console.Write("     ");
                            Console.CursorTop++;
                            Console.CursorLeft -= 5;
                            Console.Write("     ");
                        }
                        else
                        {
                            Console.ForegroundColor = Colors[Tiles[lastHighlight.Item1, lastHighlight.Item2].GetNeighborMineCount()];
                            Console.Write("     ");
                            Console.CursorTop++;
                            Console.CursorLeft -= 5;
                            Console.Write("  " + Tiles[lastHighlight.Item1, lastHighlight.Item2].GetNeighborMineCount() + "  ");
                            Console.CursorTop++;
                            Console.CursorLeft -= 5;
                            Console.Write("     ");
                        }
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
                Console.Write("║ " + (tile.GetNeighborMineCount() == 0 ? " " : tile.GetNeighborMineCount()) + " ║");
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
        public override void Dialog(string message)
        {
            var cSize = (Console.WindowWidth, Console.WindowHeight);
            var center = (cSize.Item1 / 2 - 1, cSize.Item2 / 2);
            int width = Math.Max(message.Length + 4, "Press Enter to continue".Length + 4);
            Console.SetCursorPosition(center.Item1 - width / 2, center.Item2 - 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("╔" + new string('═', width) + "╗");
            Console.CursorTop++;
            Console.CursorLeft -= width + 2;
            Console.Write("║" + new string(' ', width) + "║");
            Console.CursorTop++;
            Console.CursorLeft -= width + 2;
            Console.Write("║" + new string(' ', (width - message.Length) / 2) + message + new string(' ', (width - message.Length) / 2 + (width - message.Length) % 2) + "║");
            Console.CursorTop++;
            Console.CursorLeft -= width + 2;
            Console.Write("║" + new string(' ', width) + "║");
            Console.CursorTop++;
            Console.CursorLeft -= width + 2;
            Console.Write("║  " + "Press Enter to continue" + "  ║");
            Console.CursorTop++;
            Console.CursorLeft -= width + 2;
            Console.Write("╚" + new string('═', width) + "╝");
            while (true)
            {
                var cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                    break;
            }
        }
    }
}