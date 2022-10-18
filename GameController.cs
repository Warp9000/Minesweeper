namespace Minesweeper
{
    /// <summary>
    /// A controller for running several rounds of minesweeper.
    /// </summary>
    public class GameController
    {
        private BaseGame Game;
        /// <summary>
        /// A controller for running several rounds of minesweeper.
        /// </summary>
        public GameController(BaseGame game, Tuple<int, int> size, int mines, BasePlayer player)
        {
            Game = game;
            Game.Field = new Field(size.Item1, size.Item2, mines);
            Game.Player = player;
            Game.Renderer = player.Renderer;
        }
        /// <summary>
        /// Runs the game.
        /// </summary>
        public void Run()
        {
            while (true)
            {
                Game.PlayRound();
                if (Game.Finished)
                    break;
            }
        }
    }
}