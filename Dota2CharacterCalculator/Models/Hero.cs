using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Windows.Media.Imaging;

namespace Dota2CharacterCalculator.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        public BitmapImage Icon { get; }

        protected BaseModel(BitmapImage icon)
        {
            Icon = icon;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyProperyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Hero : BaseModel
    {
        public string Name { get; }
        public AttackDamage Damage { get; }
        public Armor Armor { get; }
        public MovementSpeed BaseMs { get; }
        public Tuple<Attribute, Attribute, Attribute> Attributes { get; }
        public AttributeType PrimaryAttribute { get; }

        private int _level;
        public int Level
        {
            get { return _level; }
            private set
            {
                if (_level == value) return;

                _level = value;
                NotifyProperyChanged(nameof(Level));
            }
        }

        public Hero(string name, BitmapImage icon, AttackDamage damage, Armor armor,
                    MovementSpeed baseMs, Tuple<Attribute, Attribute, Attribute> attributes,
                    int level, AttributeType primaryAttribute) : base(icon)
        {
            Name = name;

            Damage = damage;

            Armor = armor;
            BaseMs = baseMs;
            Attributes = attributes;
            Level = level;
            PrimaryAttribute = primaryAttribute;
        }

        public bool CanIncreaseLevel()
        {
            return Level < 25;
        }

        public void IncreaseLevel()
        {
            Level++;
            ChangeAttributes();
        }

        public bool CanDecreaseLevel()
        {
            return Level > 1;
        }

        public void DecreaseLevel()
        {
            Level--;
            ChangeAttributes();
        }

        private void ChangeAttributes()
        {
            Attributes.Item1.Change(Level);
            Attributes.Item2.Change(Level);
            Attributes.Item3.Change(Level);
        }
    }

    public class AttackDamage : BaseModel
    {
        public int Min { get; }
        public int Max { get; }

        public int MainMax { get; private set; }
        public int MainMin { get; private set; }

        public int Average => (MainMin + MainMax) / 2;


        public AttackDamage(int min, int max, BitmapImage icon) : base(icon)
        {
            Min = min;
            Max = max;
        }

        public void ChangeDamage(double attributeValue)
        {
            MainMin = Min + (int)attributeValue;
            MainMax = Max + (int)attributeValue;
            NotifyProperyChanged(nameof(Average));
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

    public class Attribute : BaseModel
    {
        public AttributeType Type { get; }
        public double Value { get; private set; }
        public double Growth { get; }

        private readonly double _startingValue;

        public Attribute(AttributeType type, double value, double growth, BitmapImage icon) : base(icon)
        {
            Type = type;
            Value = value;
            Growth = growth;
            _startingValue = value;
        }

        public void Change(int level)
        {
            Value = _startingValue;

            for (var i = 0; i < level-1; i++)
            {
                Value += Growth;
            }
            NotifyProperyChanged(nameof(Value));
        }
    }

    public enum AttributeType
    {
        Strength,
        Agility,
        Intelligence
    }
}