using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Dota2CharacterCalculator.Models;
using Dota2CharacterCalculator.ViewModels;
using Hero = Dota2CharacterCalculator.ViewModels.Hero;

namespace Dota2CharacterCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Item> _items = new ObservableCollection<Item>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            var butterfly = new Item("Butterfly", LoadItemIcon("Butterfly"))
                {AttackDamageBonus = 30, AgilityBonus = 35};
            butterfly.AdditionalPassiveProperties.Add("Attack speed +30");
            butterfly.AdditionalPassiveProperties.Add("Evasion +35%");
            butterfly.AdditionalActiveProperties.Add("Flutter");

            _items.Add(butterfly);

            var satanic = new Item("Satanic", LoadItemIcon("Satanic"))
                {AttackDamageBonus = 20, StrengthBonus = 25, ArmorBonus = 5};
            satanic.AdditionalPassiveProperties.Add("Lifesteal 25%");
            satanic.AdditionalActiveProperties.Add("Unholy Rage");

            _items.Add(satanic);

            _items.Add(new Item("Boots of Speed", LoadItemIcon("BootsOfSpeed"))
                {MovementSpeedBonus = 45.0});

            _items.Add(new Item("Blades of Attack", LoadItemIcon("BladesOfAttack"))
                {AttackDamageBonus = 9});

            _items.Add(new Item("Staff of Wizardry", LoadItemIcon("StaffOfWizardry"))
                {IntelligenceBonus = 10});

            _items.Add(new Item("Platemail", LoadItemIcon("Platemail"))
                {ArmorBonus = 10});

            var phaseBoots = new Item("Phase Boots", LoadItemIcon("PhaseBoots"))
            {MovementSpeedBonus = 45, AttackDamageBonus = 24};
            phaseBoots.AdditionalActiveProperties.Add("Phase");
            _items.Add(phaseBoots);

            foreach (var child in Inventory.Children)
            {
                var inventoryItem = child as ComboBox;
                if (inventoryItem == null) continue;

                inventoryItem.ItemsSource = _items;
            }

            var heroRepository = new HeroRepository();
            Heroes.ItemsSource = heroRepository.GetHeroes();
        }

        private void Window_OnLoad(object sender, RoutedEventArgs e)
        {
//            var downloadService = new DownloadService();
//            downloadService.DownloadHeroes();
        }

        private static BitmapImage LoadItemIcon(string iconName)
        {
            return new BitmapImage(new Uri($"Assets/Items/{iconName}.png", UriKind.Relative));
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
