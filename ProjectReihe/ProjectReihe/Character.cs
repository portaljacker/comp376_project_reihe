using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ProjectReihe
{
    class Character : Sprite
    {
        Random roll = new Random(); //roll determines chance to crit (2x damage)

        public enum CharacterType
        {
            Cadwyn,
            BossSlime
        }

        int _hp;    //health
        public int HP
        {
            get
            {
                return _hp;
            }

            set
            {
                _hp = value;
                if (_hp < 0)
                    _hp = 0;
                else if (_hp > _maxHP)
                    _hp = _maxHP;
            }
        }
        int _atk;   //attack
        public int ATK
        {
            get
            {
                return _atk;
            }
        }
        int _matk;   //magic attack
        public int MATK
        {
            get
            {
                return _matk;
            }
        }
        int _def;   //defense
        public int DEF
        {
            get
            {
                return _def;
            }
        }
        int _mdef;  //magic defense
        public int MDEF
        {
            get
            {
                return _mdef;
            }
        }

        int _maxHP; //maximum health
        public int MaxHP
        {
            get
            {
                return _maxHP;
            }
        }

        public int PreviousHealth { get; set; }
        public bool Burned { get; set; }
        public bool Chilled { get; set; }

        public Character(CharacterType type, Texture2D newTexture, Vector2 newPosition, int newWidth, int newHeight)
            : base(newTexture, newPosition, newWidth, newHeight)
        {
            Burned = false;
            Chilled = false;
            switch (type)
            {
                case CharacterType.Cadwyn:
                    _hp = 500;
                    _maxHP = 500;
                    _atk = 50;
                    _matk = 50;
                    _def = 25;
                    _mdef = 15;
                    break;
                case CharacterType.BossSlime:
                    _hp = 500;
                    _maxHP = 500;
                    _atk = 75;
                    _matk = 35;
                    _def = 30;
                    _mdef = 25;
                    break;
            }
        }

        public void attack(Character enemy, List<Skills.Skill> chain)
        {
            PreviousHealth = _hp;
            int bonus = 10;  //bonus magic damage as percent of magic attack

            #region Attack
            if (chain[0] == Skills.Skill.Attack)
            {
                if (roll.Next(10) == 0)
                    enemy.HP -= this.ATK * 2 - enemy.DEF;
                else
                    enemy.HP -= this.ATK - enemy.DEF;
                #region Attack, Attack
                if (chain[1] == Skills.Skill.Attack)
                {
                    if (roll.Next(6) == 0)
                        enemy.HP -= this.ATK * 2 - enemy.DEF;
                    else
                        enemy.HP -= this.ATK - enemy.DEF;
                    #region Attack, Attack, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(4) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                    }
                    #endregion
                    #region Attack, Attack, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Attack, Attack, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Attack, Attack, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.25);
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                }
                #endregion
                #region Attack, Fire
                else if (chain[1] == Skills.Skill.Fire)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= this.MATK - enemy.MDEF / 2;
                    else
                        enemy.HP -= this.MATK - enemy.MDEF;
                    #region Attack, Fire, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= 2 * this.MATK / bonus;
                        else
                            enemy.HP -= this.MATK / bonus;
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                    }
                    #endregion
                    #region Attack, Fire, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                        {
                            enemy.HP -= this.MATK - enemy.MDEF;
                            enemy.Burned = true;
                        }
                        if (enemy.Chilled)
                            enemy.Chilled = false;
                    }
                    #endregion
                    #region Attack, Fire, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Attack, Fire, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.25);
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                }
                #endregion
                #region Attack, Ice
                else if (chain[1] == Skills.Skill.Ice)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= this.MATK - enemy.MDEF / 2;
                    else
                        enemy.HP -= this.MATK - enemy.MDEF;
                    #region Attack, Ice, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                    }
                    #endregion
                    #region Attack, Ice, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Attack, Ice, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                        {
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                            enemy.Burned = false;
                        }
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                        if (!enemy.Chilled)
                            enemy.Chilled = true;
                    }
                    #endregion
                    #region Attack, Ice, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.25);
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                }
                #endregion
                #region Attack, Bolt
                else if (chain[1] == Skills.Skill.Bolt)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.25);
                    else
                        enemy.HP -= this.MATK - enemy.MDEF;
                    #region Attack, Bolt, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                    }
                    #endregion
                    #region Attack, Bolt, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Attack, Bolt, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Attack, Bolt, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.50);
                        else
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF) * 1.25);
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
            #region Fire
            else if (chain[0] == Skills.Skill.Fire)
            {
                enemy.HP -= this.MATK - enemy.MDEF;
                #region Fire, Attack
                if (chain[1] == Skills.Skill.Attack)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= 2 * this.MATK / bonus;
                    else
                        enemy.HP -= this.MATK / bonus;
                    if (roll.Next(10) == 0)
                        enemy.HP -= this.ATK * 2 - enemy.DEF;
                    else
                        enemy.HP -= this.ATK - enemy.DEF;
                    #region Fire, Attack, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(6) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                    }
                    #endregion
                    #region Fire, Attack, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Fire, Attack, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Fire, Attack, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.25);
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                }
                #endregion
                #region Fire, Fire
                else if (chain[1] == Skills.Skill.Fire)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= this.MATK - enemy.MDEF / 2;
                    else
                    {
                        enemy.HP -= this.MATK - enemy.MDEF;
                        enemy.Burned = true;
                    }
                    if (enemy.Chilled)
                        enemy.Chilled = false;
                    #region Fire, Fire, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                        if (enemy.Burned == true)
                            enemy.HP -= 2 * this.MATK / bonus;
                        else
                            enemy.HP -= this.MATK / bonus;
                    }
                    #endregion
                    #region Fire, Fire, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        //FIREBALL!!!
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK * 2 - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK * 2 - enemy.MDEF;
                    }
                    #endregion
                    #region Fire, Fire, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Fire, Fire, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.25);
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                }
                #endregion
                #region Fire, Ice
                else if (chain[1] == Skills.Skill.Ice)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= this.MATK - enemy.MDEF / 2;
                    else
                        enemy.HP -= this.MATK - enemy.MDEF;
                    #region Fire, Ice, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                        if (enemy.Burned == true)
                            enemy.HP -= 2 * this.MATK / bonus;
                        else
                            enemy.HP -= this.MATK / bonus;
                    }
                    #endregion
                    #region Fire, Ice, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Fire, Ice, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                        {
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                            enemy.Burned = false;
                        }
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                        if (!enemy.Chilled)
                            enemy.Chilled = true;
                    }
                    #endregion
                    #region Fire, Ice, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.25);
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                }
                #endregion
                #region Fire, Bolt
                else if (chain[1] == Skills.Skill.Bolt)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.25);
                    else
                        enemy.HP -= this.MATK - enemy.MDEF;
                    #region Fire, Bolt, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                        if (enemy.Burned == true)
                            enemy.HP -= 2 * this.MATK / bonus;
                        else
                            enemy.HP -= this.MATK / bonus;
                    }
                    #endregion
                    #region Fire, Bolt, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Fire, Bolt, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Fire, Bolt, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.50);
                        else
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF) * 1.25);
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
            #region Ice
            else if (chain[0] == Skills.Skill.Ice)
            {
                enemy.HP -= this.MATK - enemy.MDEF;
                #region Ice, Attack
                if (chain[1] == Skills.Skill.Attack)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= 2 * this.MATK / bonus;
                    else
                        enemy.HP -= this.MATK / bonus;
                    if (roll.Next(10) == 0)
                        enemy.HP -= this.ATK * 2 - enemy.DEF;
                    else
                        enemy.HP -= this.ATK - enemy.DEF;
                    #region Ice, Attack, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(6) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                    }
                    #endregion
                    #region Ice, Attack, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Ice, Attack, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Ice, Attack, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.25);
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                }
                #endregion
                #region Ice, Fire
                else if (chain[1] == Skills.Skill.Fire)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= this.MATK - enemy.MDEF / 2;
                    else
                        enemy.HP -= this.MATK - enemy.MDEF;
                    #region Ice, Fire, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                        if (enemy.Burned == true)
                            enemy.HP -= 2 * this.MATK / bonus;
                        else
                            enemy.HP -= this.MATK / bonus;
                    }
                    #endregion
                    #region Ice, Fire, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                        {
                            enemy.HP -= this.MATK - enemy.MDEF;
                            enemy.Burned = true;
                        }
                        if (enemy.Chilled)
                            enemy.Chilled = false;
                    }
                    #endregion
                    #region Ice, Fire, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Ice, Fire, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        //Frostfire bolt!!!
                        if (enemy.Burned == true)
                            enemy.HP -= 2 * (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.25);
                        else
                        {
                            enemy.HP -= 2 * this.MATK - enemy.MDEF;
                            enemy.Burned = true;
                        }
                    }
                    #endregion
                }
                #endregion
                #region Ice, Ice
                else if (chain[1] == Skills.Skill.Ice)
                {
                    if (enemy.Burned == true)
                    {
                        enemy.HP -= this.MATK - enemy.MDEF / 2;
                        enemy.Burned = false;
                    }
                    else
                        enemy.HP -= this.MATK - enemy.MDEF;
                    if (!enemy.Chilled)
                        enemy.Chilled = true;
                    #region Ice, Ice, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                        enemy.HP -= this.MATK / bonus;
                    }
                    #endregion
                    #region Ice, Ice, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                        enemy.HP -= this.MATK - enemy.MDEF;
                    #endregion
                    #region Ice, Ice, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                        //Blizzard!!!
                        enemy.HP -= 2 * this.MATK - enemy.MDEF;
                    #endregion
                    #region Ice, Ice, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                        enemy.HP -= this.MATK - enemy.MDEF;
                    #endregion
                }
                #endregion
                #region Ice, Bolt
                else if (chain[1] == Skills.Skill.Bolt)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.50);
                    else
                        enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF) * 1.25);
                    #region Ice, Bolt, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                        if (enemy.Burned == true)
                            enemy.HP -= 2 * this.MATK / bonus;
                        else
                            enemy.HP -= this.MATK / bonus;
                    }
                    #endregion
                    #region Ice, Bolt, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Ice, Bolt, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Ice, Bolt, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.50);
                        else
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF) * 1.25);
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
            #region Bolt
            else if (chain[0] == Skills.Skill.Bolt)
            {
                if (enemy.Burned == true)
                    enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.50);
                else
                    enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF) * 1.25);
                #region Bolt, Attack
                if (chain[1] == Skills.Skill.Attack)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= 2 * this.MATK / bonus;
                    else
                        enemy.HP -= this.MATK / bonus;
                    if (roll.Next(10) == 0)
                        enemy.HP -= this.ATK * 2 - enemy.DEF;
                    else
                        enemy.HP -= this.ATK - enemy.DEF;
                    #region Bolt, Attack, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(6) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                    }
                    #endregion
                    #region Bolt, Attack, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Bolt, Attack, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Bolt, Attack, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.25);
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                }
                #endregion
                #region Bolt, Fire
                else if (chain[1] == Skills.Skill.Fire)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= this.MATK - enemy.MDEF / 2;
                    else
                        enemy.HP -= this.MATK - enemy.MDEF;
                    #region Bolt, Fire, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                        if (enemy.Burned == true)
                            enemy.HP -= 2 * this.MATK / bonus;
                        else
                            enemy.HP -= this.MATK / bonus;
                    }
                    #endregion
                    #region Bolt, Fire, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                        {
                            enemy.HP -= this.MATK - enemy.MDEF;
                            enemy.Burned = true;
                        }
                        if (enemy.Chilled)
                            enemy.Chilled = false;
                    }
                    #endregion
                    #region Bolt, Fire, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Bolt, Fire, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.25);
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                }
                #endregion
                #region Bolt, Ice
                else if (chain[1] == Skills.Skill.Ice)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= this.MATK - enemy.MDEF / 2;
                    else
                        enemy.HP -= this.MATK - enemy.MDEF;
                    #region Bolt, Ice, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                        if (enemy.Burned == true)
                            enemy.HP -= 2 * this.MATK / bonus;
                        else
                            enemy.HP -= this.MATK / bonus;
                    }
                    #endregion
                    #region Bolt, Ice, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Bolt, Ice, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                        {
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                            enemy.Burned = false;
                        }
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                        if (!enemy.Chilled)
                            enemy.Chilled = true;
                    }
                    #endregion
                    #region Bolt, Ice, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.25);
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                }
                #endregion
                #region Bolt, Bolt
                else if (chain[1] == Skills.Skill.Bolt)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.50);
                    else
                        enemy.HP -= (int)Math.Round((this.MATK - enemy.MDEF) * 1.25);
                    #region Bolt, Bolt, Attack
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                        if (enemy.Burned == true)
                            enemy.HP -= 2 * this.MATK / bonus;
                        else
                            enemy.HP -= this.MATK / bonus;
                    }
                    #endregion
                    #region Bolt, Bolt, Fire
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Bolt, Bolt, Ice
                    else if (chain[2] == Skills.Skill.Ice)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                    #endregion
                    #region Bolt, Bolt, Bolt
                    else if (chain[2] == Skills.Skill.Bolt)
                    {
                        //Thunderstorm!!!
                        if (enemy.Burned == true)
                            enemy.HP -= 2 * (int)Math.Round((this.MATK - enemy.MDEF / 2) * 1.50);
                        else
                            enemy.HP -= 2 * (int)Math.Round((this.MATK - enemy.MDEF) * 1.25);
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
        }

        public void Draw(SpriteBatch spriteBatch, float scale)
        {
            if (Burned)
            {
                spriteBatch.Draw(Texture, Position, SourceRect, Color.Red, 0f, Origin, scale, SpriteEffects.None, 0);
            }
            else
            {
                base.Draw(spriteBatch, scale, SourceRect);

            }
        }
    }
}
