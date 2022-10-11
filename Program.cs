namespace Minesweeper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                IEnumerable<BaseGame> games = typeof(BaseGame)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BaseGame)) && !t.IsAbstract)
                .Select(t => (BaseGame)Activator.CreateInstance(t)!)!;

                Console.WriteLine("Welcome to Minesweeper!");
                Console.WriteLine("Fullscreen is recommended.");
                Console.WriteLine();
                Console.WriteLine("Please select a gamemode:");
                foreach (BaseGame g in games)
                {
                    Console.WriteLine($"  {g.Name}");
                }
                int cursorPosition = 0;
                BaseGame? game = null;
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 4);
                Console.WriteLine(">");
                string[] desc = games.ElementAt(cursorPosition).Description.Split("\n");
                Console.SetCursorPosition(40, 4);
                foreach (var item in desc)
                {
                    if (item.Length > Console.BufferWidth - 41)
                    {
                        int i = 0;
                        while ((Console.BufferWidth - 41) * (i + 1) < item.Length)
                        {
                            Console.Write(item.Substring((Console.BufferWidth - 41) * i, Math.Min((Console.BufferWidth - 41) * (i + 1), item.Length)));
                            Console.CursorLeft = 40;
                            Console.CursorTop++;
                        }
                    }
                    Console.Write(item + new string(' ', Console.BufferWidth - 41 - item.Length));
                    Console.CursorLeft = 40;
                    Console.CursorTop++;
                    Console.Write(new string(' ', Console.BufferWidth - 41));
                    Console.CursorLeft = 40;
                }
                while (game == null)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.UpArrow)
                    {
                        if (cursorPosition > 0)
                        {
                            Console.SetCursorPosition(0, cursorPosition + 4);
                            Console.Write(" ");
                            cursorPosition--;
                            Console.SetCursorPosition(0, cursorPosition + 4);
                            Console.Write(">");
                            desc = games.ElementAt(cursorPosition).Description.Split("\n");
                            Console.SetCursorPosition(40, 4);
                            foreach (var item in desc)
                            {
                                if (item.Length > Console.BufferWidth - 41)
                                {
                                    int i = 0;
                                    while ((Console.BufferWidth - 41) * (i + 1) < item.Length)
                                    {
                                        Console.Write(item.Substring((Console.BufferWidth - 41) * i, Math.Min((Console.BufferWidth - 41) * (i + 1), item.Length)));
                                        Console.CursorLeft = 40;
                                        Console.CursorTop++;
                                    }
                                }
                                Console.Write(item + new string(' ', Console.BufferWidth - 41 - item.Length));
                                Console.CursorLeft = 40;
                                Console.CursorTop++;
                                Console.Write(new string(' ', Console.BufferWidth - 41));
                                Console.CursorLeft = 40;
                            }
                        }
                    }
                    else if (key.Key == ConsoleKey.DownArrow)
                    {
                        if (cursorPosition < games.Count() - 1)
                        {
                            Console.SetCursorPosition(0, cursorPosition + 4);
                            Console.Write(" ");
                            cursorPosition++;
                            Console.SetCursorPosition(0, cursorPosition + 4);
                            Console.Write(">");
                            desc = games.ElementAt(cursorPosition).Description.Split("\n");
                            Console.SetCursorPosition(40, 4);
                            foreach (var item in desc)
                            {
                                if (item.Length > Console.BufferWidth - 41)
                                {
                                    int i = 0;
                                    while ((Console.BufferWidth - 41) * (i + 1) < item.Length)
                                    {
                                        Console.Write(item.Substring((Console.BufferWidth - 41) * i, Math.Min((Console.BufferWidth - 41) * (i + 1), item.Length)));
                                        Console.CursorLeft = 40;
                                        Console.CursorTop++;
                                    }
                                }
                                Console.Write(item + new string(' ', Console.BufferWidth - 41 - item.Length));
                                Console.CursorLeft = 40;
                                Console.CursorTop++;
                                Console.Write(new string(' ', Console.BufferWidth - 41));
                                Console.CursorLeft = 40;
                            }
                        }
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        game = games.ElementAt(cursorPosition);
                    }
                }
                Console.Clear();


                IEnumerable<BasePlayer> players = typeof(BasePlayer)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BasePlayer)) && !t.IsAbstract)
                .Select(t => (BasePlayer)Activator.CreateInstance(t)!)!;

                Console.WriteLine("Welcome to Minesweeper!");
                Console.WriteLine("Fullscreen is recommended.");
                Console.WriteLine();
                Console.WriteLine("Please select a player:");
                foreach (BasePlayer p in players)
                {
                    Console.WriteLine($"  {p.Name}");
                }
                cursorPosition = 0;
                BasePlayer? player = null;
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 4);
                Console.WriteLine(">");
                while (player == null)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.UpArrow)
                    {
                        if (cursorPosition > 0)
                        {
                            Console.SetCursorPosition(0, cursorPosition + 4);
                            Console.Write(" ");
                            cursorPosition--;
                            Console.SetCursorPosition(0, cursorPosition + 4);
                            Console.Write(">");
                        }
                    }
                    else if (key.Key == ConsoleKey.DownArrow)
                    {
                        if (cursorPosition < players.Count() - 1)
                        {
                            Console.SetCursorPosition(0, cursorPosition + 4);
                            Console.Write(" ");
                            cursorPosition++;
                            Console.SetCursorPosition(0, cursorPosition + 4);
                            Console.Write(">");
                        }
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        player = players.ElementAt(cursorPosition);
                    }
                }
                Console.Clear();
                Console.CursorVisible = true;
                Console.WriteLine("Enter the size of the playfield, comma seperated x,y, max at current console size is " + (Console.WindowWidth - 10) / 5 + "," + (Console.WindowHeight - 6) / 3);
                int sx = 0;
                int sy = 0;
                while (sx == 0 || sy == 0)
                {
                    string[] size = Console.ReadLine()!.Split(',');
                    if (size.Length != 2)
                    {
                        Console.WriteLine("Invalid input, try again");
                        continue;
                    }
                    if (!int.TryParse(size[0], out sx))
                    {
                        Console.WriteLine("Invalid input, try again");
                        continue;
                    }
                    if (!int.TryParse(size[1], out sy))
                    {
                        Console.WriteLine("Invalid input, try again");
                        continue;
                    }
                }
                Console.WriteLine("Enter the number of mines");
                int mines = 0;
                while (mines == 0)
                {
                    string input = Console.ReadLine()!;
                    if (!int.TryParse(input, out mines))
                    {
                        Console.WriteLine("Invalid input, try again");
                        continue;
                    }
                }
                Console.Clear();
                Console.CursorVisible = false;
                var gameC = new GameController(game, new Tuple<int, int>(sx, sy), mines, player);
                gameC.Run();
            }
        }
    }
}