using System.Collections.Generic;
using System.Linq;
using System.Net;
using AngleSharp;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using Dota2CharacterCalculator.Models;

namespace Dota2CharacterCalculator.Services
{
    public class DownloadService
    {
        public void DownloadHeroes()
        {
            const string heroTableAddress = "http://dota2.gamepedia.com/Table_of_hero_attributes";
            var document = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(heroTableAddress).Result;

            var heroes = new List<Hero>();
            var heroesAttributesTable = document.QuerySelector<IHtmlTableElement>("table.wikitable");
            foreach (var row in heroesAttributesTable.Rows.Skip(1))
            {
                var hero = new Hero();

                // Icon and name
                var iconAndName = row.Cells[0].QuerySelector<IHtmlSpanElement>("span");

                // Name
                hero.Name = iconAndName.QuerySelector<IHtmlAnchorElement>("a:nth-child(2)").Text;

                // He is unreleased so is missing almost all info
                if (hero.Name == "Monkey King") continue;

                // Icon
                var icon = iconAndName.QuerySelector<IHtmlImageElement>("a img");

                // Need to use http, cause https throws exception
                var iconUrl = icon.Source;
                const string protocol = "http://";
                var restOfUrl = iconUrl.Substring(8);
                iconUrl = protocol + restOfUrl;

                using (var webClient = new WebClient())
                {
                    var iconBytes = webClient.DownloadData(iconUrl);
                    hero.Icon = iconBytes;
                }

                // Primary attribute
                hero.PrimaryAttribute = row.Cells[1].QuerySelector<IHtmlAnchorElement>("a").Title;

                // Attributes
                hero.BaseStrength = int.Parse(row.Cells[2].TextContent.Trim());
                hero.StrengthGrowth = double.Parse(row.Cells[3].TextContent.Trim());

                hero.BaseAgility = int.Parse(row.Cells[4].TextContent.Trim());
                hero.AgilityGrowth = double.Parse(row.Cells[5].TextContent.Trim());

                hero.BaseIntelligence = int.Parse(row.Cells[6].TextContent.Trim());
                hero.IntelligenceGrowth = double.Parse(row.Cells[7].TextContent.Trim());

                hero.BaseMovementSpeed = int.Parse(row.Cells[11].TextContent.Trim());

                var baseArmor = row.Cells[12].QuerySelector<IHtmlSpanElement>("span").Title;
                hero.BaseArmor = double.Parse(baseArmor.Split(' ')[0]);

                var baseMinAttackDamage = row.Cells[13].QuerySelector<IHtmlSpanElement>("span").Title;
                hero.BaseMinAttackDamage = int.Parse(baseMinAttackDamage.Split(' ')[0]);
                var baseMaxAttackDamage = row.Cells[14].QuerySelector<IHtmlSpanElement>("span").Title;
                hero.BaseMaxAttackDamage = int.Parse(baseMaxAttackDamage.Split(' ')[0]);

                heroes.Add(hero);
            }

            var heroRepository = new HeroRepository();
            heroRepository.DeleteAllHeroes();
            heroRepository.Save(heroes);
        }
    }
}