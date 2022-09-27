namespace Minesweeper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                IEnumerable<BasePlayer> players = typeof(BasePlayer)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BasePlayer)) && !t.IsAbstract)
                .Select(t => (BasePlayer)Activator.CreateInstance(t)!)!;

                System.Console.WriteLine("Select a player:");
                foreach (BasePlayer p in players)
                {
                    System.Console.WriteLine($"  {p.Name}");
                }
                int cursorPosition = 0;
                BasePlayer? player = null;
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 1);
                System.Console.WriteLine(">");
                while (player == null)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.UpArrow)
                    {
                        if (cursorPosition > 0)
                        {
                            Console.SetCursorPosition(0, cursorPosition + 1);
                            System.Console.Write(" ");
                            cursorPosition--;
                            Console.SetCursorPosition(0, cursorPosition + 1);
                            System.Console.Write(">");
                        }
                    }
                    else if (key.Key == ConsoleKey.DownArrow)
                    {
                        if (cursorPosition < players.Count() - 1)
                        {
                            Console.SetCursorPosition(0, cursorPosition + 1);
                            System.Console.Write(" ");
                            cursorPosition++;
                            Console.SetCursorPosition(0, cursorPosition + 1);
                            System.Console.Write(">");
                        }
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        player = players.ElementAt(cursorPosition);
                    }
                }
                Console.Clear();
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
                var game = new Game(new Tuple<int, int>(sx, sy), mines, player);
                Console.Clear();
                game.Run();
                Console.ReadKey(true);
            }
        }
    }
}