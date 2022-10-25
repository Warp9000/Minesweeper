using System.Diagnostics;
using System.Threading;

namespace Minesweeper.Custom.Games
{
    public class AIBenchmarker : BaseGame
    {
        public override string Name => "AI Benchmarker";

        public override string Description => $"A game to benchmark AI's\n Runs {gamesToDo} default games and outputs the average time it took to win";

        List<GameResult> times = new List<GameResult>();

        int gamesToDo = 10000;

        int gamesDone = 0;

        public override void PlayRound()
        {
            for (gamesDone = 0; gamesDone < gamesToDo; gamesDone++)
            {
                var game = new DefaultGame();
                game.Field = new Field(this.Field.Width, this.Field.Height, this.Field.Mines);
                game.Player = this.Player;
                game.Renderer = new DudRenderer();
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                while (!game.Finished)
                {
                    game.PlayRound();
                }
                stopwatch.Stop();
                times.Add(new GameResult(game.Won!.Value, stopwatch.ElapsedMilliseconds));
                Console.SetCursorPosition(0, 0);
                Console.Write($"Games done: {gamesDone}/{gamesToDo} ({((gamesDone / (double)gamesToDo) * 100).ToString("N2")}%)");
                Console.SetCursorPosition(0, 1);
                Console.Write($"this: {stopwatch.ElapsedMilliseconds}ms\t{(game.Won.Value ? "win " : "loss")}");
                Console.SetCursorPosition(0, 2);
                Console.Write("Winrate: " + (times.Where(x => x.Won).Count() / ((double)gamesDone + 1) * 100).ToString("N2") + "%");
                Console.SetCursorPosition(0, 3);
                Console.Write("average: " + (times.Where(x => x.Won).Count() != 0 ? $"Win: {(times.Where(x => x.Won).Average(x => x.Time)).ToString("N2")}ms " : ""));
                if (times.Where(x => !x.Won).Count() != 0)
                    Console.Write($"Loss: {(times.Where(x => !x.Won).Average(x => x.Time)).ToString("N2")}ms");
            }
            var winrate = times.Where(x => x.Won).Count() / ((double)gamesToDo);
            double avgWinTime = -1;
            double avgLossTime = -1;
            if (times.Where(x => x.Won).Count() != 0)
            {
                avgWinTime = times.Where(x => x.Won).Average(x => x.Time);
            }
            if (times.Where(x => !x.Won).Count() != 0)
            {
                avgLossTime = times.Where(x => !x.Won).Average(x => x.Time);
            }
            Console.Clear();
            Console.WriteLine($"Winrate: {winrate * 100}%");
            Console.WriteLine($"Average win time: {avgWinTime}ms");
            Console.WriteLine($"Average lose time: {avgLossTime}ms");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Finished = true;
        }
        public class GameResult
        {
            public bool Won;
            public double Time;
            public GameResult(bool won, double time)
            {
                Won = won;
                Time = time;
            }
        }
    }
    public class DudRenderer : BaseRenderer
    {
        public override void Dialog(string message) { }
        public override void Dialog(string[] message) { }
        public override (int, int) GetHighlightPos()
        {
            return (0, 0);
        }
        public override void Highlight(int x, int y) { }
        public override void Render(Field.Tile[,] tiles, int remainingMines, bool[,]? mines = null) { }
    }
}