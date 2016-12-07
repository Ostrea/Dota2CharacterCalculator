using System.Windows.Input;

namespace Dota2CharacterCalculator
{
    public static class Commands
    {
       public static readonly RoutedUICommand IncreaseHeroLevel = new RoutedUICommand
           (
               "Increase hero level",
               "Increase hero level",
               typeof(Commands)
           );
       public static readonly RoutedUICommand DecreaseHeroLevel = new RoutedUICommand
           (
               "Decrease hero level",
               "Decrease hero level",
               typeof(Commands)
           );

       public static readonly RoutedUICommand DownloadHeroData = new RoutedUICommand
           (
               "Download hero data",
               "Download hero data",
               typeof(Commands)
           );
    }
}