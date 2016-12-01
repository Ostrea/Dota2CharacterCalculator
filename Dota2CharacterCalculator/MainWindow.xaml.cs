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
using Dota2CharacterCalculator.Models;
using Attribute = Dota2CharacterCalculator.Models.Attribute;

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

            _items.Add(new Item("Butterfly", LoadIcon("Butterfly", IconType.Items)));
            _items.Add(new Item("Satanic", LoadIcon("Satanic", IconType.Items)));

            _items.Add(new Item("Boots of Speed", LoadIcon("BootsOfSpeed", IconType.Items))
                {MovementSpeedBonus = 45.0});

            foreach (var child in Inventory.Children)
            {
                var inventoryItem = child as ComboBox;
                if (inventoryItem == null) continue;

                inventoryItem.ItemsSource = _items;
                inventoryItem.SelectionChanged += OnInventoryItemChange;
            }

            var attackDamageIcon = LoadIcon("AttackDamage", IconType.Stats);
            var armorIcon = LoadIcon("Armor", IconType.Stats);
            var msIcon = LoadIcon("MovementSpeed", IconType.Stats);

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
                    AttributeType.Agility
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
                    AttributeType.Intelligence
                );

            _heroes.Add(morph);
            _heroes.Add(invoker);

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

        private void OnInventoryItemChange(object sender, SelectionChangedEventArgs e)
        {
//            MessageBox.Show((sender as ComboBox).Name);
//            MessageBox.Show((FirstItemInInventory.SelectedItem as Item).Name);
        }
    }
}
