using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;

namespace Dota2CharacterCalculator.ViewModels
{
    public class Hero : BaseModel
    {
        public string Name { get; }
        public AttackDamage Damage { get; }
        public Armor Armor { get; }
        public MovementSpeed MovementSpeed { get; }
        public Tuple<Attribute, Attribute, Attribute> Attributes { get; }
        public AttributeType PrimaryAttribute { get; }
        public Health Health { get; }
        public Mana Mana { get; }

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
                    int level, AttributeType primaryAttribute, Health health, Mana mana) : base(icon)
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

            Health = health;
            Health.SetStrengthAttribute(Attributes.Item1);

            Mana = mana;
            Mana.SetIntelligenceAttribute(Attributes.Item3);

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
            if (previousItem?.AgilityBonus != null)
            {
                Attributes.Item2.BonusValue -= previousItem.AgilityBonus.Value;
            }
            if (previousItem?.IntelligenceBonus != null)
            {
                Attributes.Item3.BonusValue -= previousItem.IntelligenceBonus.Value;
            }
            if (previousItem?.ArmorBonus != null)
            {
                Armor.BonusArmor -= previousItem.ArmorBonus.Value;
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
            if (newItem.StrengthBonus != null)
            {
                Attributes.Item1.BonusValue += newItem.StrengthBonus.Value;
            }
            if (newItem.AgilityBonus != null)
            {
                Attributes.Item2.BonusValue += newItem.AgilityBonus.Value;
            }
            if (newItem.IntelligenceBonus != null)
            {
                Attributes.Item3.BonusValue += newItem.IntelligenceBonus.Value;
            }
            if (newItem.ArmorBonus != null)
            {
                Armor.BonusArmor += newItem.ArmorBonus.Value;
            }
        }
    }
}