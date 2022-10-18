using System.Diagnostics;
namespace Minesweeper
{
    /// <summary>
    /// Represents a game of minesweeper.
    /// </summary>
    public abstract class BaseGame
    {
        /// <summary>
        /// Name of the game.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Description of the game. Telling the players the rules etc.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// The field to play on.
        /// </summary>
        public Field Field = new Field(0, 0, 0);

        /// <summary>
        /// The renderer to use. Defaulted to <see cref="FancyRenderer"/>.
        /// </summary>
        public BaseRenderer Renderer = new FancyRenderer();

        /// <summary>
        /// The player to play the game. Defaulted to <see cref="HumanPlayer"/>.
        /// </summary>
        public BasePlayer Player = new HumanPlayer();

        /// <summary>
        /// A bool representing if the game is finished.
        /// </summary>
        public bool Finished = false;

        /// <summary>
        /// A bool representing if the game is won. Only valid if <see cref="Finished"/> is true.
        /// </summary>
        public bool? Won = null;

        /// <summary>
        /// Runs a round of the game.
        /// </summary>
        public abstract void PlayRound();
    }

    public class DefaultGame : BaseGame
    {
        public override string Name => "Default";
        public override string Description => "The default game of Minesweeper";
        private bool firstReveal = true;
        Stopwatch PlayerTimer = new Stopwatch();
        Stopwatch GameTimer = new Stopwatch();
        public override void PlayRound()
        {
            GameTimer.Start();
            var hiddenField = Field.GenerateHiddenField();
            if (!Player.selfRender)
                Renderer.Render(hiddenField, Field.Mines - Field.Flags);
            GameTimer.Stop();
            PlayerTimer.Start();
            var input = Player.Play(hiddenField, Field.Mines - Field.Flags);
            PlayerTimer.Stop();
            GameTimer.Start();
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
                        if (firstReveal)
                        {
                            Field.MineField[revealPosition.Item1, revealPosition.Item2] = false;
                            for (int y = 0; y < Field.Height; y++)
                            {
                                for (int x = 0; x < Field.Width; x++)
                                {
                                    if (!Field.MineField[x, y] && !Field.Tiles[x, y].IsRevealed)
                                    {
                                        Field.MineField[x, y] = true;
                                        y = Field.Height + 1;
                                        break;
                                    }
                                }
                            }
                            for (int xx = 0; xx < Field.Width; xx++)
                            {
                                for (int yy = 0; yy < Field.Height; yy++)
                                {
                                    Field.Tiles[xx, yy] = new Field.Tile(Field.CalculateNeighborMineCount(xx, yy), xx, yy);
                                }
                            }
                            tile.IsRevealed = true;
                            firstReveal = false;
                            continue;
                        }
                        // if (!Player.selfRender)
                        Renderer.Render(Field.Tiles, Field.Mines - Field.Flags, Field.MineField);
                        Renderer.Highlight(revealPosition.Item1, revealPosition.Item2);
                        List<string> msg = new List<string>();
                        msg.Add("You lost!");
                        msg.Add($"You took {PlayerTimer.ElapsedMilliseconds}ms to play.");
                        msg.Add($"The game took {GameTimer.ElapsedMilliseconds}ms compute.");
                        Renderer.Dialog(msg.ToArray());
                        Finished = true;
                        Won = false;
                        return;
                    }
                }
                firstReveal = false;
            }
            if (checkWin())
            {
                Renderer.Render(Field.Tiles, Field.Mines - Field.Flags, Field.MineField);
                GameTimer.Stop();
                List<string> msg = new List<string>();
                msg.Add("You won!");
                msg.Add($"You took {PlayerTimer.ElapsedMilliseconds}ms to play the game.");
                msg.Add($"The game took {GameTimer.ElapsedMilliseconds}ms compute.");
                Renderer.Dialog(msg.ToArray());
                Finished = true;
                Won = true;
                return;
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