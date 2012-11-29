using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ProjectReihe
{
    class Sprite
    {
        private Texture2D texture; //Stores the texture.
        private Vector2 position; //Stores the position.
        private int width; //Stores the width of the sprite.
        private int height; //Stores the height of the sprite.
        private Vector2 origin; //Stores the origin of the sprite (center point).
        private Rectangle sourceRect;
        private int currentFrame = 0;
        private int currentRow = 0;

        //Accessors and mutators.
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

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

        public Vector2 Origin
        {
            get
            {
                return origin;
            }

            set
            {
                origin = value;
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
        }

        public int CurrentFrame
        {
            get
            {
                return currentFrame;
            }

            set
            {
                currentFrame = value;
                sourceRect = new Rectangle(Width * currentFrame, Height * currentRow, Width, Height);
            }
        }

        public int CurrentRow
        {
            get
            {
                return currentRow;
            }

            set
            {
                currentRow = value;
                sourceRect = new Rectangle(Width * currentFrame, Height * currentRow, Width, Height);
            }
        }

        public Rectangle SourceRect
        {
            get
            {
                return sourceRect;
            }

            set
            {
                sourceRect = value;
            }
        }

        public Sprite(Texture2D newTexture, Vector2 newPosition, int newWidth, int newHeight)
        {
            texture = newTexture;
            position = newPosition;
            width = newWidth;
            height = newHeight;
            origin = new Vector2(width / 2, height / 2);
            sourceRect = new Rectangle(Width * currentFrame, Height * currentRow, Width, Height);

        }

        public virtual void Draw(SpriteBatch spriteBatch, float scale)
        {
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, origin, scale, SpriteEffects.None, 0);
        }

        public virtual void Draw(SpriteBatch spriteBatch, float scale, Rectangle sourceR)
        {
            spriteBatch.Draw(texture, position, sourceR, Color.White, 0f, origin, scale, SpriteEffects.None, 0);
        }

    }
}
