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

            _items.Add(new Item("Butterfly", new BitmapImage(new Uri("Assets/Items/Butterfly.png", UriKind.Relative))));
            _items.Add(new Item("Satanic", new BitmapImage(new Uri("Assets/Items/Satanic.png", UriKind.Relative))));

            foreach (var child in Inventory.Children)
            {
                var inventoryItem = child as ComboBox;
                if (inventoryItem != null)
                {
                    inventoryItem.ItemsSource = _items;
                }

            }

            _heroes.Add(new Hero("Morphling", new BitmapImage(new Uri("Assets/Heroes/Morphling.png", UriKind.Relative))));
            _heroes.Add(new Hero("Invoker", new BitmapImage(new Uri("Assets/Heroes/Invoker.png", UriKind.Relative))));

            Heroes.ItemsSource = _heroes;
        }
    }
}
