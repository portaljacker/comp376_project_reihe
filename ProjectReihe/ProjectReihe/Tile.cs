using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ProjectReihe
{
    class Tile
    {
        private Texture2D tiles;
        private Rectangle source;
        private Vector2 position;
        private Vector2 origin;
        private char val;
        private int row;
        private int col;
        private Rectangle colBox;

        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public Rectangle Source
        {
            get
            {
                return source;
            }
        }

        public Char Val
        {
            get
            {
                return val;
            }

            set
            {
                val = value;
            }
        }

        public Rectangle ColBox
        {
            get
            {
                return colBox;
            }
        }

        public void updateColBox()
        {
            colBox = new Rectangle((int)position.X, (int)position.Y, 25, 25);
        }

        public Tile(Texture2D newTexture, char c)
        {
            tiles = newTexture;
            origin = new Vector2(0, 0);

            val = c;

            position = new Vector2(0, 0);

            colBox = new Rectangle((int)position.X, (int)position.Y, 25, 25);

        }

        public virtual void Draw(SpriteBatch spriteBatch, float scale, Vector2 newPos)
        {
            if (val == 'a')
            {
                row = 0;
                col = 0;
            }

            else if (val == 'b')
            {
                row = 1;
                col = 0;
            }

            else if (val == 'c')
            {
                row = 2;
                col = 0;
            }

            else if (val == 'd')
            {
                row = 3;
                col = 0;
            }

            else if (val == 'e')
            {
                row = 4;
                col = 0;
            }

            else if (val == 'f')
            {
                row = 0;
                col = 1;
            }

            else if (val == 'g')
            {
                row = 1;
                col = 1;
            }

            else if (val == 'h')
            {
                row = 2;
                col = 1;
            }

            else if (val == 'i')
            {
                row = 3;
                col = 1;
            }

            else if (val == 'j')
            {
                row = 4;
                col = 1;
            }

            else if (val == 'k')
            {
                row = 0;
                col = 2;
            }

            else if (val == 'l')
            {
                row = 1;
                col = 2;
            }

            else if (val == 'm')
            {
                row = 2;
                col = 2;
            }

            else if (val == 'n')
            {
                row = 3;
                col = 2;
            }

            else if (val == 'o')
            {
                row = 4;
                col = 2;
            }

            else if (val == 'p')
            {
                row = 0;
                col = 3;
            }

            else if (val == 'q')
            {
                row = 1;
                col = 3;
            }

            else if (val == 'r')
            {
                row = 2;
                col = 3;
            }

            else if (val == 's')
            {
                row = 3;
                col = 3;
            }

            else if (val == 't')
            {
                row = 4;
                col = 3;
            }

            else if (val == 'u')
            {
                row = 0;
                col = 4;
            }

            else if (val == 'v')
            {
                row = 1;
                col = 4;
            }

            else if (val == 'w')
            {
                row = 2;
                col = 4;
            }

            else if (val == 'x')
            {
                row = 3;
                col = 4;
            }

            else if (val == 'y')
            {
                row = 4;
                col = 4;
            }

            else if (val == 'z')
            {
                row = 0;
                col = 5;
            }

            else if (val == '0')
            {
                row = 1;
                col = 5;
            }

            else if (val == '1')
            {
                row = 2;
                col = 5;
            }

            else if (val == '2')
            {
                row = 3;
                col = 5;
            }

            else if (val == '3')
            {
                row = 4;
                col = 5;
            }

            source = new Rectangle(25 * col, 25 * row, 25, 25);


            spriteBatch.Draw(tiles, newPos, source, Color.White, 0f, origin, scale, SpriteEffects.None, 0);
        }
    }
}

