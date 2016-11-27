﻿using System.Windows.Media.Imaging;

namespace Dota2CharacterCalculator.Models
{
    public abstract class BaseModel
    {
        public BitmapImage Icon { get; }

        protected BaseModel(BitmapImage icon)
        {
            Icon = icon;
        }
    }

    public class Hero : BaseModel
    {
        public string Name { get; }
        public AttackDamage Damage { get; }
        public Armor Armor { get; }
        public MovementSpeed BaseMs { get; }

        public Hero(string name, BitmapImage icon, AttackDamage damage, Armor armor, MovementSpeed baseMs) : base(icon)
        {
            Name = name;
            Damage = damage;
            Armor = armor;
            BaseMs = baseMs;
        }
    }

    public class AttackDamage : BaseModel
    {
        public int Min { get; }
        public int Max { get; }
        public int Average => (Min + Max) / 2;


        public AttackDamage(int min, int max, BitmapImage icon) : base(icon)
        {
            Min = min;
            Max = max;
        }
    }

    public class Armor : BaseModel
    {
        public double BaseArmor { get; }

        public Armor(double baseArmor, BitmapImage icon) : base(icon)
        {
            BaseArmor = baseArmor;
        }
    }

    public class MovementSpeed : BaseModel
    {
        public double Value { get; }

        public MovementSpeed(double value, BitmapImage icon) : base(icon)
        {
            Value = value;
        }
    }
}