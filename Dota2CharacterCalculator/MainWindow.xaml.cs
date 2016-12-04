using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dota2CharacterCalculator.ViewModels;
using Attribute = Dota2CharacterCalculator.ViewModels.Attribute;

namespace Dota2CharacterCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Item> _items = new ObservableCollection<Item>();
        private readonly ObservableCollection<Hero> _heroes = new ObservableCollection<Hero>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            var butterfly = new Item("Butterfly", LoadIcon("Butterfly", IconType.Items))
                {AttackDamageBonus = 30, AgilityBonus = 35};
            butterfly.AdditionalPassiveProperties.Add("Attack speed +30");
            butterfly.AdditionalPassiveProperties.Add("Evasion +35%");
            butterfly.AdditionalActiveProperties.Add("Flutter");

            _items.Add(butterfly);

            var satanic = new Item("Satanic", LoadIcon("Satanic", IconType.Items))
                {AttackDamageBonus = 20, StrengthBonus = 25, ArmorBonus = 5};
            satanic.AdditionalPassiveProperties.Add("Lifesteal 25%");
            satanic.AdditionalActiveProperties.Add("Unholy Rage");

            _items.Add(satanic);

            _items.Add(new Item("Boots of Speed", LoadIcon("BootsOfSpeed", IconType.Items))
                {MovementSpeedBonus = 45.0});

            _items.Add(new Item("Blades of Attack", LoadIcon("BladesOfAttack", IconType.Items))
                {AttackDamageBonus = 9});

            _items.Add(new Item("Staff of Wizardry", LoadIcon("StaffOfWizardry", IconType.Items))
                {IntelligenceBonus = 10});

            _items.Add(new Item("Platemail", LoadIcon("Platemail", IconType.Items))
                {ArmorBonus = 10});

            var phaseBoots = new Item("Phase Boots", LoadIcon("PhaseBoots", IconType.Items))
            {MovementSpeedBonus = 45, AttackDamageBonus = 24};
            phaseBoots.AdditionalActiveProperties.Add("Phase");
            _items.Add(phaseBoots);

            foreach (var child in Inventory.Children)
            {
                var inventoryItem = child as ComboBox;
                if (inventoryItem == null) continue;

                inventoryItem.ItemsSource = _items;
            }

            var attackDamageIcon = LoadIcon("AttackDamage", IconType.Stats);
            var armorIcon = LoadIcon("Armor", IconType.Stats);
            var msIcon = LoadIcon("MovementSpeed", IconType.Stats);
            var healthIcon = LoadIcon("Health", IconType.Stats);
            var manaIcon = LoadIcon("Mana", IconType.Stats);

            var strengthIcon = LoadIcon("Strength", IconType.Stats);
            var agilityIcon = LoadIcon("Agility", IconType.Stats);
            var intelligenceIcon = LoadIcon("Intelligence", IconType.Stats);

            var morph = new Hero
                (
                    "Morphling",
                    LoadIcon("Morphling", IconType.Heroes),
                    new AttackDamage(9, 18, attackDamageIcon),
                    new Armor(-2.0, armorIcon),
                    new MovementSpeed(285.0, msIcon),
                    new Tuple<Attribute, Attribute, Attribute>
                    (
                        new Attribute(AttributeType.Strength, 19.0, 2.0, strengthIcon),
                        new Attribute(AttributeType.Agility, 24.0, 3.7, agilityIcon),
                        new Attribute(AttributeType.Intelligence, 17.0, 1.1, intelligenceIcon)
                    ),
                    1,
                    AttributeType.Agility,
                    new Health(healthIcon),
                    new Mana(manaIcon)
                );

            var invoker = new Hero
                (
                    "Invoker",
                    LoadIcon("Invoker", IconType.Heroes),
                    new AttackDamage(19, 25, attackDamageIcon),
                    new Armor(-1.0, armorIcon),
                    new MovementSpeed(280.0, msIcon),
                    new Tuple<Attribute, Attribute, Attribute>
                    (
                        new Attribute(AttributeType.Strength, 17.0, 1.7, strengthIcon),
                        new Attribute(AttributeType.Agility, 14.0, 1.9, agilityIcon),
                        new Attribute(AttributeType.Intelligence, 16.0, 4.0, intelligenceIcon)
                    ),
                    1,
                    AttributeType.Intelligence,
                    new Health(healthIcon),
                    new Mana(manaIcon)
                );

            var omniknight = new Hero
            (
                "Omniknight",
                LoadIcon("Omniknight", IconType.Heroes),
                new AttackDamage(31, 41, attackDamageIcon),
                new Armor(3.0, armorIcon),
                new MovementSpeed(305.0, msIcon),
                new Tuple<Attribute, Attribute, Attribute>
                (
                    new Attribute(AttributeType.Strength, 22.0, 2.8, strengthIcon),
                    new Attribute(AttributeType.Agility, 15.0, 1.75, agilityIcon),
                    new Attribute(AttributeType.Intelligence, 17.0, 1.8, intelligenceIcon)
                ),
                1,
                AttributeType.Strength,
                new Health(healthIcon),
                new Mana(manaIcon)
            );

            _heroes.Add(morph);
            _heroes.Add(invoker);
            _heroes.Add(omniknight);

            Heroes.ItemsSource = _heroes;
        }

        private enum IconType
        {
            Heroes,
            Stats,
            Items,
        }

        private static BitmapImage LoadIcon(string iconName, IconType iconType)
        {
            return new BitmapImage(new Uri($"Assets/{iconType}/{iconName}.png", UriKind.Relative));
        }

        private void IncreaseHeroLevelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var selectedHero = Heroes.SelectedItem as Hero;
            e.CanExecute = selectedHero?.CanIncreaseLevel() ?? false;
        }

        private void IncreaseHeroLevelCommand_OnExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedHero = Heroes.SelectedItem as Hero;
            selectedHero?.IncreaseLevel();
        }

        private void DecreaseHeroLevelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var selectedHero = Heroes.SelectedItem as Hero;
            e.CanExecute = selectedHero?.CanDecreaseLevel() ?? false;
        }

        private void DecreaseHeroLevelCommand_OnExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedHero = Heroes.SelectedItem as Hero;
            selectedHero?.DecreaseLevel();
        }

        private void Heroes_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StrengthAttribute.Foreground = AgilityAttribute.Foreground
                = IntelligenceAttribute.Foreground = Brushes.Black;

            var selectedHero = (Hero) Heroes.SelectedItem;
            switch (selectedHero.PrimaryAttribute)
            {
                case AttributeType.Strength:
                    StrengthAttribute.Foreground = Brushes.Red;
                    break;
                case AttributeType.Agility:
                    AgilityAttribute.Foreground = Brushes.Green;
                    break;
                case AttributeType.Intelligence:
                    IntelligenceAttribute.Foreground = Brushes.Blue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
