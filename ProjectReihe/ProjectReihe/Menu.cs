using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectReihe
{
    class Menu
    {
        private List<string> MenuItems;
        private int iterator;
        public MenuType CurrentMenuType { get; set; }
        public string InfoText { get; set; }
        public string Title { get; set; }
        public int Iterator
        {
            get
            {
                return iterator;
            }
            set
            {
                iterator = value;
                if (iterator > MenuItems.Count - 1)
                    iterator = 0;
                if (iterator < 0)
                    iterator = MenuItems.Count - 1;
            }
        }
        public enum MenuType
        {
            Battle,
            Fight
        }

        public Menu(MenuType menuType)
        {
            CurrentMenuType = menuType;
            switch (menuType)
            {
                case MenuType.Battle:
                    Title = string.Empty;
                    MenuItems = new List<string>();
                    MenuItems.Add("Fight");
                    Iterator = 0;
                    InfoText = string.Empty;
                    break;
                case MenuType.Fight:
                    Title = "Fight";
                    MenuItems = new List<string>();
                    MenuItems.Add("Attack");
                    MenuItems.Add("Fire");
                    MenuItems.Add("Ice");
                    MenuItems.Add("Bolt");
                    Iterator = 0;
                    InfoText = string.Empty;
                    break;
            }
        }

        public int GetNumberOfOptions()
        {
            return MenuItems.Count;
        }

        public string GetItem(int index)
        {
            return MenuItems[index];
        }

        public void DrawMenu(SpriteBatch batch, int screenWidth, SpriteFont font)
        {
            batch.DrawString(font, Title, new Vector2(screenWidth / 2 - font.MeasureString(Title).X / 2, 20), Color.White);
            int yPos = 100;
            for (int i = 0; i < GetNumberOfOptions(); i++)
            {
                Color colour = Color.White;
                if (i == Iterator)
                {
                    colour = Color.Gray;
                }
                batch.DrawString(font, GetItem(i), new Vector2(screenWidth / 2 - font.MeasureString(GetItem(i)).X / 2, yPos), colour);
                yPos += 50;
            }
        }

        public void DrawEndScreen(SpriteBatch batch, int screenWidth, SpriteFont arial)
        {
            batch.DrawString(arial, InfoText, new Vector2(screenWidth / 2 - arial.MeasureString(InfoText).X / 2, 300), Color.White);
            string prompt = "Press Enter to Continue";
            batch.DrawString(arial, prompt, new Vector2(screenWidth / 2 - arial.MeasureString(prompt).X / 2, 400), Color.White);
        }
    }
}
