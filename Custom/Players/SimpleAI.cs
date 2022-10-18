namespace Minesweeper.Custom.Players
{
    public sealed class SimpleAIPlayer : BasePlayer
    {
        public SimpleAIPlayer()
        {
            Name = "Simple AI";
            Renderer = new FancyRenderer();
        }
        private bool firstMove = true;
        public override PlayerReturn Play(Field.Tile[,] tiles, int remainingMines)
        {
            var pr = new PlayerReturn(new List<(int, int)>(), new List<(int, int)>());
            if (firstMove)
            {
                firstMove = false;
                pr.RevealPositions.Add((tiles.GetLength(0) / 2, tiles.GetLength(1) / 2));
                return pr;
            }
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
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    if (tiles[x, y].NeighborMines == GetNeighbors(x, y, tiles).Where(t => t.IsFlagged).Count())
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
                                && !tiles[x + j, y + i].IsRevealed
                                && !tiles[x + j, y + i].IsFlagged)
                                {
                                    pr.RevealPositions.Add((x + j, y + i));
                                    return pr;
                                }
                            }
                        }
                    }
                    if (tiles[x, y].NeighborMines == GetNeighbors(x, y, tiles).Where(t => !t.IsRevealed).Count())// - GetNeighbors(x, y, tiles).Where(t => t.IsFlagged).Count())
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
                                && !tiles[x + j, y + i].IsRevealed
                                && !tiles[x + j, y + i].IsFlagged)
                                {
                                    pr.FlagPositions.Add((x + j, y + i));
                                    return pr;
                                }
                            }
                        }

                    }
                }
            }
            if (pr.RevealPositions.Count == 0 && pr.FlagPositions.Count == 0)
            {
                Renderer.Dialog("I don't know what to do!");
                var rng = new Random();
                var x = rng.Next(tiles.GetLength(0));
                var y = rng.Next(tiles.GetLength(1));
                while (tiles[x, y].IsRevealed)
                {
                    x = rng.Next(tiles.GetLength(0));
                    y = rng.Next(tiles.GetLength(1));
                }
                pr.RevealPositions.Add((x, y));
            }
            return pr;
        }
        public Field.Tile[] GetNeighbors(int x, int y, Field.Tile[,] tiles)
        {
            var neighbors = new List<Field.Tile>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (j == 0 && i == 0)
                    {
                        continue;
                    }
                    if (x + j >= 0 && x + j < tiles.GetLength(0) && y + i >= 0 && y + i < tiles.GetLength(1))
                    {
                        neighbors.Add(tiles[x + j, y + i]);
                    }
                }
            }
            return neighbors.ToArray();
        }
    }
}