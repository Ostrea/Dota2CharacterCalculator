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

            foreach (var child in Inventory.Children)
            {
                var inventoryItem = child as ComboBox;
                if (inventoryItem != null)
                {
                    inventoryItem.ItemsSource = _items;
                }

            }

            var attackDamageIcon = LoadIcon("AttackDamage", IconType.Stats);
            var armorIcon = LoadIcon("Armor", IconType.Stats);
            var msIcon = LoadIcon("MovementSpeed", IconType.Stats);

            var strengthIcon = LoadIcon("Strength", IconType.Stats);
            var agilityIcon = LoadIcon("Agility", IconType.Stats);
            var intelligenceIcon = LoadIcon("Intelligence", IconType.Stats);

            _heroes.Add(new Hero
                (
                    "Morphling",
                    LoadIcon("Morphling", IconType.Heroes),
                    new AttackDamage(9, 18, attackDamageIcon),
                    new Armor(-2.0, armorIcon),
                    new MovementSpeed(285.0, msIcon),
                    new Tuple<Attribute, Attribute, Attribute>
                        (
                            new Attribute("Strength", 19.0, 2.0, strengthIcon),
                            new Attribute("Agility", 24.0, 3.7, agilityIcon),
                            new Attribute("Intelligence", 17.0, 1.1, intelligenceIcon)
                        ),
                    1
                )
            );
            _heroes.Add(new Hero
                (
                    "Invoker",
                    LoadIcon("Invoker", IconType.Heroes),
                    new AttackDamage(19, 25, attackDamageIcon),
                    new Armor(-1.0, armorIcon),
                    new MovementSpeed(280.0, msIcon),
                    new Tuple<Attribute, Attribute, Attribute>
                        (
                            new Attribute("Strength", 17.0, 1.7, strengthIcon),
                            new Attribute("Agility", 14.0, 1.9, agilityIcon),
                            new Attribute("Intelligence", 16.0, 4.0, intelligenceIcon)
                        ),
                    1
                )
            );

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
            e.CanExecute = true;
        }

        private void IncreaseHeroLevelCommand_OnExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Hello increase level handler");
        }

        private void DecreaseHeroLevelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DecreaseHeroLevelCommand_OnExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Hello decrease level handler");
        }
    }
}
