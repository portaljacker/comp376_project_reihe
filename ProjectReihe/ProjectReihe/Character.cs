using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectReihe
{
    class Character
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
        bool _burned = false;    //is burned?
        public bool Burned { get; set; }

        public Character(CharacterType type)
        {
            switch (type)
            {
                case CharacterType.Cadwyn:
                    _hp = 350;
                    _atk = 50;
                    _matk = 40;
                    _def = 25;
                    _mdef = 15;
                    break;
                case CharacterType.BossSlime:
                    _hp = 500;
                    _atk = 75;
                    _matk = 35;
                    _def = 30;
                    _mdef = 25;
                    break;
            }
        }

        public void attack(Character enemy, List<Skills.Skill> chain)
        {
            int bonus = 0;  //bonus magic damage as percent of magic attack

            if (chain[0] == Skills.Skill.Attack)
            {
                if (roll.Next(10) == 0)
                    enemy.HP -= this.ATK * 2 - enemy.DEF;
                else
                    enemy.HP -= this.ATK - enemy.DEF;
                if (chain[1] == Skills.Skill.Attack)
                {
                    if (roll.Next(6) == 0)
                        enemy.HP -= this.ATK * 2 - enemy.DEF;
                    else
                        enemy.HP -= this.ATK - enemy.DEF;
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(4) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                    }
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                    }
                }
                else if (chain[1] == Skills.Skill.Fire)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= this.MATK - enemy.MDEF / 2;
                    else
                        enemy.HP -= this.MATK - enemy.MDEF;
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        bonus += 10;
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK / bonus - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK / bonus - enemy.MDEF;
                        if (roll.Next(6) == 0)
                        {
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        }
                        else
                        {
                            enemy.HP -= this.ATK - enemy.DEF;
                        }
                    }
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                        enemy.Burned = true;
                    }
                }
            }
            else if (chain[0] == Skills.Skill.Fire)
            {
                enemy.HP -= this.MATK - enemy.MDEF;
                if (chain[1] == Skills.Skill.Attack)
                {
                    bonus += 10;
                    if (enemy.Burned == true)
                        enemy.HP -= this.MATK / bonus - enemy.MDEF / 2;
                    else
                        enemy.HP -= this.MATK /bonus - enemy.MDEF;
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                    }
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK - enemy.MDEF;
                        enemy.Burned = true;
                    }
                }
                else if (chain[1] == Skills.Skill.Fire)
                {
                    if (enemy.Burned == true)
                        enemy.HP -= this.MATK - enemy.MDEF / 2;
                    else
                        enemy.HP -= this.MATK - enemy.MDEF;
                    enemy.Burned = true;
                    if (chain[2] == Skills.Skill.Attack)
                    {
                        if (roll.Next(10) == 0)
                            enemy.HP -= this.ATK * 2 - enemy.DEF;
                        else
                            enemy.HP -= this.ATK - enemy.DEF;
                        bonus += 10;
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK / bonus - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK / bonus - enemy.MDEF;
                    }
                    else if (chain[2] == Skills.Skill.Fire)
                    {
                        //FIREBALL!!!
                        if (enemy.Burned == true)
                            enemy.HP -= this.MATK * 2 - enemy.MDEF / 2;
                        else
                            enemy.HP -= this.MATK * 2 - enemy.MDEF;
                    }
                }
            }
        }
    }
}
