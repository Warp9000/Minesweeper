
class goodies
{
    // check the direct 8 neighbors around given x and y coordinates
    public void CheckNeighbors(int x, int y, int sizex, int sizey)
    {
        for (int yy = y - 1; yy <= y + 1; yy++) // loops vertically
        {
            for (int xx = x - 1; xx <= x + 1; xx++) // loops horizontally
            {
                // sizex and sizey can be hardcoded if its a fixed size of your board
                if ((xx == x && yy == y) || x < 0 || x >= sizex || y < 0 || y >= sizey) // dont check current tile, and dont go out of bounds
                    continue; // continues the for loop skiping the code below once
                              // your code here, access your array with xx and yy here
            }
        }
    }


    // check all tiles in all 8 directions
    public void Check8Directions(int x, int y, int sizex, int sizey)
    {
        for (int yy = -1; yy <= 1; yy++) // y multiplier for i
        {
            for (int xx = -1; xx <= 1; xx++) // x multiplier for i
            {
                if (xx == 0 && yy == 0) // dont check current tile
                    continue;

                for (int i = 1; i < Math.Max(sizex, sizey); i++) // multiplied with xx and yy gives coordinates to check
                {
                    int cX = xx * i + x;
                    int cY = yy * i + y;
                    if (cX < 0 || cX > sizex || cY < 0 || cY > sizey) // make sure tiles out of bound gets checked
                        break;

                    // your code here, check with cX and cY
                }
                // other code here after each directions is checked
            }
        }
    }
}