using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ProjectReihe
{
    class Map
    {
        private Tile[,] grid;

        private Texture2D tiles;
        private Vector2 origin;
        private const int W = 70;
        private const int H = 30;
        private bool closed = true;

        public Tile getTile(int i, int j)
        {
            return grid[i, j];
        }

        public void setTile(int i, int j, char c)
        {
            getTile(i, j).Val = c;
        }

        public bool Closed
        {
            get
            {
                return closed;
            }

            set
            {
                closed = value;
            }
        }

        public Map(Texture2D newTexture, float scale)
        {
            grid = new Tile[W, H];
            tiles = newTexture;
            origin = new Vector2(0, 0);

            //Fill map array with blank tiles.
            for (int i = 0; i < W; i++)
            {
                for (int j = 0; j < H; j++)
                {
                    grid[i, j] = new Tile(tiles, '3');
                    grid[i, j].Position = new Vector2(0 + 25 * j * scale, 0 + 25 * i * scale);
                }
            }

            //Begin drawing map.
            //Bottom gym wall.
            grid[0, 0].Val = 'e';
            for (int j = 1; j < 11; j++)
            {
                grid[0, j].Val = 'g';
            }
            grid[0, 11].Val = 'o';

            grid[1, 0].Val = 'd';

            for (int j = 1; j < 11; j++)
            {
                grid[1, j].Val = 'f';
            }
            grid[1, 11].Val = 'n';

            //Left gym wall.
            for (int i = 2; i < 20; i++)
            {
                grid[i, 0].Val = 'b';
            }

            //Right gym wall.
            for (int i = 2; i < 20; i++)
            {
                grid[i, 11].Val = 'l';
            }

            //Fill gym area.
            for (int i = 2; i < 19; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    grid[i, j].Val = 'y';
                }
            }

            //Top gym wall.
            grid[20, 0].Val = 'a';
            for (int j = 1; j < 6; j++)
            {
                grid[19, j].Val = 'g';
            }
            grid[19, 6].Val = 'p';
            grid[19, 7].Val = 'h';
            grid[19, 8].Val = 'q';
            for (int j = 9; j < 11; j++)
            {
                grid[19, j].Val = 'g';
            }

            for (int j = 1; j < 6; j++)
            {
                grid[20, j].Val = 'f';
            }
            grid[20, 6].Val = '2';//Exit from gym.
            grid[20, 7].Val = 'h';
            grid[20, 8].Val = '1';
            for (int j = 9; j < 11; j++)
            {
                grid[20, j].Val = 'f';
            }
            grid[20, 11].Val = 'k';

            //Narrow passage 1.
            //Left wall.
            for (int i = 21; i < 29; i++)
            {
                grid[i, 6].Val = 'b';
            }
            for (int i = 33; i < 43; i++)
            {
                grid[i, 6].Val = 'b';
            }
            //Path.
            for (int i = 21; i < 42; i++)
            {
                grid[i, 7].Val = 'h';
            }
            //Right wall.
            for (int i = 21; i < 39; i++)
            {
                grid[i, 8].Val = 'l';
            }

            //Branch.
            grid[29, 6].Val = 'u';
            for (int j = 6; j >= 2; j--)
            {
                grid[31, j].Val = 'h';
            }
            grid[31, 1].Val = '0'; //Switch tile.
            grid[30, 0].Val = 'd';

            //Branch Bottom wall.
            for (int j = 1; j < 6; j++)
            {
                grid[29, j].Val = 'g';
            }
            for (int j = 1; j < 6; j++)
            {
                grid[30, j].Val = 'f';
            }

            //Branch side wall.
            grid[29, 0].Val = 'e';
            grid[31, 0].Val = 'b';
            grid[32, 0].Val = 'b';
            grid[33, 0].Val = 'a';

            //Branch top wall.
            for (int j = 1; j < 6; j++)
            {
                grid[32, j].Val = 'g';
            }
            for (int j = 1; j < 6; j++)
            {
                grid[33, j].Val = 'f';
            }
            grid[30, 6].Val = 's';
            grid[32, 6].Val = 'p';
            grid[33, 6].Val = '2';
            //End branch.

            //Turn right.
            grid[39, 8].Val = 'v';
            for (int j = 9; j < 17; j++)
            {
                grid[39, j].Val = 'g';
            }
            grid[40, 8].Val = 'r';
            for (int j = 9; j < 17; j++)
            {
                grid[40, j].Val = 'f';
            }
            grid[43, 6].Val = 'a';


            for (int j = 7; j < 15; j++)
            {
                grid[42, j].Val = 'g';
            }
            for (int j = 7; j < 15; j++)
            {
                grid[43, j].Val = 'f';
            }

            for (int j = 8; j < 17; j++)
            {
                grid[41, j].Val = 'h';
            }

            //Turn up.
            grid[42, 15].Val = 'p';
            grid[43, 15].Val = '2';
            grid[39, 17].Val = 'o';
            grid[40, 17].Val = 'n';
            //Left wall
            for (int i = 44; i < 59; i++)
            {
                grid[i, 15].Val = 'b';
            }
            grid[59, 15].Val = 'u';

            //Right wall.
            for (int i = 41; i < 59; i++)
            {
                grid[i, 17].Val = 'l';
            }
            grid[59, 17].Val = 'v';

            //Path up.
            if (closed)
            {
                grid[42, 16].Val = 'j';//Closed door.
                grid[43, 16].Val = 'i';
            }
            else
            {
                grid[42, 16].Val = 'h';//Open door.
                grid[43, 16].Val = 'h';
            }
            for (int i = 44; i < 61; i++)
            {
                grid[i, 16].Val = 'h';
            }

            //Boss room.
            //Bottom wall.
            grid[59, 14].Val = 'j';
            grid[59, 13].Val = 'g';
            grid[59, 12].Val = 'e';
            grid[60, 15].Val = 's';
            grid[60, 14].Val = 'i';
            grid[60, 13].Val = 'f';
            grid[60, 12].Val = 'd';
            grid[60, 17].Val = 'r';
            grid[60, 18].Val = 'f';
            grid[60, 19].Val = 'i';
            grid[60, 20].Val = 'n';
            grid[59, 18].Val = 'g';
            grid[59, 19].Val = 'j';
            grid[59, 20].Val = 'o';

            //Left wall.
            for (int i = 61; i < 66; i++)
            {
                grid[i, 12].Val = 'b';
            }
            grid[66, 12].Val = 'a';

            //Right wall.
            for (int i = 61; i < 66; i++)
            {
                grid[i, 20].Val = 'l';
            }
            grid[66, 20].Val = 'k';

            //Top wall.
            for (int j = 13; j < 20; j++)
            {
                grid[66, j].Val = 'f';
            }
            for (int j = 13; j < 20; j++)
            {
                grid[65, j].Val = 'g';
            }

            //Fill room with floor tiles.
            for (int i = 61; i < 65; i++)
            {
                for (int j = 13; j < 20; j++)
                {
                    grid[i, j].Val = 'h';
                }
            }


            //Add cracks detail at various places.
            grid[5, 0].Val = 'c';
            grid[11, 0].Val = 'c';
            grid[15, 11].Val = 'm';
            grid[19, 3].Val = 'j';
            grid[20, 3].Val = 'i';
            grid[62, 12].Val = 'c';
            grid[64, 20].Val = 'm';
            grid[29, 3].Val = 'j';
            grid[30, 3].Val = 'i';

        }

        /*public void move(Vector2 translate, float scale, Rectangle cadBox)
        {

            for (int i = 0; i < W; i++)
            {
                for (int j = 0; j < H; j++)
                {
                    getTile(i, j).Position = new Vector2(0 + 25 * j * scale, 0 + -25 * i * scale);
                }
            }

        }*/

        public virtual void Draw(SpriteBatch spriteBatch, float scale, Vector2 translate, Vector2 cadPos)
        {
            for (int i = 0; i < W; i++)
            {
                for (int j = 0; j < H; j++)
                {
                    Vector2 pos = new Vector2(j * 25 * scale + cadPos.X - 35, -i * 25 * scale + cadPos.Y + 50);
                    pos += translate;
                    getTile(i, j).Draw(spriteBatch, scale, pos);

                }
            }
        }





    }
}
