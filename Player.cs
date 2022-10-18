namespace Minesweeper
{
    /// <summary>
    /// Represents a player in the game.
    /// </summary>
    public abstract class BasePlayer
    {
        /// <summary>
        /// The Player's name.
        /// </summary>
        public string Name = "Unset";

        /// <summary>
        /// A bool set by the player if it will render the game.
        /// otherwise the game will render itself.
        /// </summary>
        public bool selfRender = false;

        /// <summary>
        /// The renderer to use. Defaulted to <see cref="SimpleRenderer"/>.
        /// </summary>
        public BaseRenderer Renderer = new SimpleRenderer();

        /// <summary>
        /// Called every time the player needs to make a move.
        /// </summary>
        public abstract PlayerReturn Play(Field.Tile[,] tiles, int remainingMines);
    }
    /// <summary>
    /// Represents what a player does in the game.
    /// </summary>
    public class PlayerReturn
    {
        /// <summary>
        /// A list of coordinates to flag.
        /// </summary>
        public List<(int, int)> FlagPositions;

        /// <summary>
        /// A list of coordinates to reveal.
        /// </summary>
        public List<(int, int)> RevealPositions;

        /// <summary>
        /// Represents what a player does in the game.
        /// </summary>
        /// <param name="revealPositions"></param>
        /// <param name="flagPositions"></param>
        public PlayerReturn(List<(int, int)> revealPositions, List<(int, int)> flagPositions)
        {
            FlagPositions = flagPositions;
            RevealPositions = revealPositions;
        }
    }
    public sealed class HumanPlayer : BasePlayer
    {
        public HumanPlayer()
        {
            Name = "Human (you)";
            Renderer = new FancyRenderer();
            selfRender = true;
        }
        public (int, int) cursorPosition = (0, 0);
        public override PlayerReturn Play(Field.Tile[,] tiles, int remainingMines)
        {
            var pr = new PlayerReturn(new List<(int, int)>(), new List<(int, int)>());
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    if (tiles[x, y].NeighborMines == 0)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                if (j == 0 && i == 0)
                                {
                                    continue;
                                }
                                if (x + j >= 0
                                && x + j < tiles.GetLength(0)
                                && y + i >= 0
                                && y + i < tiles.GetLength(1)
                                && !tiles[x + j, y + i].IsRevealed)
                                    pr.RevealPositions.Add((x + j, y + i));
                            }
                        }

                    }
                }
            }
            if (pr.RevealPositions.Count != 0)
            {
                return pr;
            }
            Renderer.Render(tiles, remainingMines);
            while (pr.RevealPositions.Count == 0 && pr.FlagPositions.Count == 0)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        cursorPosition = (cursorPosition.Item1, cursorPosition.Item2 - 1);
                        ClampCursor((tiles.GetLength(0), tiles.GetLength(1)));
                        Renderer.Highlight(cursorPosition.Item1, cursorPosition.Item2);
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        cursorPosition = (cursorPosition.Item1, cursorPosition.Item2 + 1);
                        ClampCursor((tiles.GetLength(0), tiles.GetLength(1)));
                        Renderer.Highlight(cursorPosition.Item1, cursorPosition.Item2);
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        cursorPosition = (cursorPosition.Item1 - 1, cursorPosition.Item2);
                        ClampCursor((tiles.GetLength(0), tiles.GetLength(1)));
                        Renderer.Highlight(cursorPosition.Item1, cursorPosition.Item2);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        cursorPosition = (cursorPosition.Item1 + 1, cursorPosition.Item2);
                        ClampCursor((tiles.GetLength(0), tiles.GetLength(1)));
                        Renderer.Highlight(cursorPosition.Item1, cursorPosition.Item2);
                        break;
                    case ConsoleKey.Spacebar:
                        pr.RevealPositions.Add(cursorPosition);
                        break;
                    case ConsoleKey.F:
                        pr.FlagPositions.Add(cursorPosition);
                        break;
                    default:
                        break;
                }
                // Renderer.Render(tiles, remainingMines);
            }
            return pr;
        }
        private void ClampCursor((int, int) size)
        {
            cursorPosition = (Math.Clamp(cursorPosition.Item1, 0, size.Item1 - 1), Math.Clamp(cursorPosition.Item2, 0, size.Item2 - 1));
        }
    }

}