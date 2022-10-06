namespace Minesweeper
{
    /// <summary>
    /// Represents a game of Minesweeper.
    /// </summary>
    public class GameController
    {
        private BaseGame Game;
        public GameController(BaseGame game, Tuple<int, int> size, int mines, BasePlayer player)
        {
            Game = game;
            Game.Field = new Field(size.Item1, size.Item2, mines);
            Game.Player = player;
            Game.Renderer = player.Renderer;
        }
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