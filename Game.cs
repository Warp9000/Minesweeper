namespace Minesweeper
{
    public abstract class BaseGame
    {
        public abstract string Name { get; }
        public Field Field;
        public BaseRenderer Renderer;
        public BasePlayer Player;
        public bool Finished = false;
        public bool? Won = null;
        public abstract void PlayRound();
    }

    public class DefaultGame : BaseGame
    {
        public override string Name => "Default";
        private bool firstReveal = true;
        public override void PlayRound()
        {
            if (!Player.selfRender)
                Renderer.Render(Field.Tiles, Field.Mines - Field.Flags);
            var input = Player.Play(Field.Tiles, Field.Mines - Field.Flags);
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
                                    Field.Tiles[xx, yy] = new Field.Tile(Field.GetNeighborMineCount(xx, yy), xx, yy);
                                }
                            }
                            tile.IsRevealed = true;
                            firstReveal = false;
                            continue;
                        }
                        // if (!Player.selfRender)
                        Renderer.Render(Field.Tiles, Field.Mines - Field.Flags, Field.MineField);
                        Renderer.Dialog("You lost!");
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
                Renderer.Dialog("You won!");
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