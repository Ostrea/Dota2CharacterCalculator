using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Windows;
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
        public MovementSpeed MovementSpeed { get; }
        public Tuple<Attribute, Attribute, Attribute> Attributes { get; }
        public AttributeType PrimaryAttribute { get; }

        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

        public ObservableCollection<string> ActiveEffects { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> PassiveEffects { get; } = new ObservableCollection<string>();

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
                    MovementSpeed movementSpeed, Tuple<Attribute, Attribute, Attribute> attributes,
                    int level, AttributeType primaryAttribute) : base(icon)
        {
            Name = name;

            // Need to do it before damage and armor calculation
            Attributes = attributes;
            Attributes.Item1.SetHero(this);
            Attributes.Item2.SetHero(this);
            Attributes.Item3.SetHero(this);

            PrimaryAttribute = primaryAttribute;

            Damage = damage;
            Damage.SetPrimaryAttribute(GetPrimaryAttribute());

            Armor = armor;
            Armor.SetAgilityAttribute(Attributes.Item2);

            MovementSpeed = movementSpeed;
            Level = level;

            for (var i = 0; i < 6; i++)
            {
                Items.Add(null);
            }
            Items.CollectionChanged += OnInventoryChange;
        }

        public bool CanIncreaseLevel()
        {
            return Level < 25;
        }

        public void IncreaseLevel()
        {
            Level++;
        }

        public bool CanDecreaseLevel()
        {
            return Level > 1;
        }

        public void DecreaseLevel()
        {
            Level--;
        }

        private Attribute GetPrimaryAttribute()
        {
            switch (PrimaryAttribute)
            {
                case AttributeType.Strength:
                    return Attributes.Item1;
                case AttributeType.Agility:
                    return Attributes.Item2;
                case AttributeType.Intelligence:
                    return Attributes.Item3;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnInventoryChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            var previousItem = e.OldItems?[0] as Item;
            if (previousItem?.MovementSpeedBonus != null)
            {
                MovementSpeed.BonusValue -= previousItem.MovementSpeedBonus.Value;
            }
            if (previousItem?.AttackDamageBonus != null)
            {
                Damage.BonusValue -= previousItem.AttackDamageBonus.Value;
            }
            if (previousItem != null && previousItem.AdditionalActiveProperties.Count != 0)
            {
                foreach (var activeProperty in previousItem.AdditionalActiveProperties)
                {
                    ActiveEffects.Remove(activeProperty);
                }
            }
            if (previousItem != null && previousItem.AdditionalPassiveProperties.Count != 0)
            {
                foreach (var passiveProperty in previousItem.AdditionalPassiveProperties)
                {
                    PassiveEffects.Remove(passiveProperty);
                }
            }
            if (previousItem?.StrengthBonus != null)
            {
                Attributes.Item1.BonusValue -= previousItem.StrengthBonus.Value;
            }

            var newItem = (Item) e.NewItems[0];
            if (newItem.MovementSpeedBonus != null)
            {
                MovementSpeed.BonusValue += newItem.MovementSpeedBonus.Value;
            }
            if (newItem.AttackDamageBonus != null)
            {
                Damage.BonusValue += newItem.AttackDamageBonus.Value;
            }
            if (newItem.AdditionalActiveProperties.Count != 0)
            {
                foreach (var activeProperty in newItem.AdditionalActiveProperties)
                {
                    ActiveEffects.Add(activeProperty);
                }
            }
            if (newItem.AdditionalPassiveProperties.Count != 0)
            {
                foreach (var passiveProperty in newItem.AdditionalPassiveProperties)
                {
                    PassiveEffects.Add(passiveProperty);
                }
            }
            if (newItem?.StrengthBonus != null)
            {
                Attributes.Item1.BonusValue += newItem.StrengthBonus.Value;
            }
        }
    }

    public class AttackDamage : BaseModel
    {
        public int BaseMin { get; }
        public int BaseMax { get; }

        public int MainMax { get; private set; }
        public int MainMin { get; private set; }

        public int Average => (MainMin + MainMax) / 2;

        private int _bonusValue;
        public int BonusValue
        {
            get { return _bonusValue; }
            set
            {
                if (_bonusValue == value) return;

                _bonusValue = value;
                NotifyProperyChanged(nameof(BonusValue));
            }
        }

        public AttackDamage(int baseMin, int baseMax, BitmapImage icon) : base(icon)
        {
            BaseMin = baseMin;
            BaseMax = baseMax;
        }

        public void SetPrimaryAttribute(Attribute attribute)
        {
            attribute.PropertyChanged += OnAttributePropertyChange;
        }

        private void OnAttributePropertyChange(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(Attribute.Value)) return;

            var attributeValue = ((Attribute) sender).Value;

            MainMin = BaseMin + (int)attributeValue;
            MainMax = BaseMax + (int)attributeValue;
            NotifyProperyChanged(nameof(Average));
        }
    }

    public class Armor : BaseModel
    {
        public double BaseArmor { get; }

        public double MainArmor { get; private set; }

        public Armor(double baseArmor, BitmapImage icon) : base(icon)
        {
            BaseArmor = baseArmor;
        }

        public void SetAgilityAttribute(Attribute agility)
        {
            agility.PropertyChanged += OnAgilityPropertyChange;
        }

        private void OnAgilityPropertyChange(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(Attribute.Value)) return;

            var agilityValue = ((Attribute) sender).Value;

            MainArmor = BaseArmor + agilityValue / 7.0;
            NotifyProperyChanged(nameof(MainArmor));
        }
    }

    public class MovementSpeed : BaseModel
    {
        public double BaseValue { get; }

        private double _bonusValue;
        public double BonusValue
        {
            get { return _bonusValue; }
            set
            {
                if (Math.Abs(_bonusValue - value) < 1e-5) return;

                _bonusValue = value;
                NotifyProperyChanged(nameof(TotalValue));
            }
        }

        public double TotalValue => BaseValue + BonusValue;

        public MovementSpeed(double baseValue, BitmapImage icon) : base(icon)
        {
            BaseValue = baseValue;
        }
    }

    public class Attribute : BaseModel
    {
        public AttributeType Type { get; }
        public double Value { get; private set; }
        public double Growth { get; }

        private double _bonusValue;
        public double BonusValue
        {
            get { return _bonusValue; }
            set
            {
                if (Math.Abs(_bonusValue - value) < 1e-5) return;

                _bonusValue = value;
                NotifyProperyChanged(nameof(BonusValue));
            }
        }

        private readonly double _startingValue;

        public Attribute(AttributeType type, double value, double growth, BitmapImage icon) : base(icon)
        {
            Type = type;
            Value = value;
            Growth = growth;
            _startingValue = value;
        }

        public void SetHero(Hero hero)
        {
            hero.PropertyChanged += OnHeroPropertyChange;
        }

        private void OnHeroPropertyChange(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(Hero.Level)) return;

            Value = _startingValue;
            var hero = (Hero) sender;

            for (var i = 0; i < hero.Level-1; i++)
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