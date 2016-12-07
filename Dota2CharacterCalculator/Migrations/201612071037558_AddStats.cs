namespace Dota2CharacterCalculator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Heroes", "PrimaryAttribute", c => c.String());
            AddColumn("dbo.Heroes", "BaseStrength", c => c.Int(nullable: false));
            AddColumn("dbo.Heroes", "StrengthGrowth", c => c.Double(nullable: false));
            AddColumn("dbo.Heroes", "BaseAgility", c => c.Int(nullable: false));
            AddColumn("dbo.Heroes", "AgilityGrowth", c => c.Double(nullable: false));
            AddColumn("dbo.Heroes", "BaseIntelligence", c => c.Int(nullable: false));
            AddColumn("dbo.Heroes", "IntelligenceGrowth", c => c.Double(nullable: false));
            AddColumn("dbo.Heroes", "BaseMovementSpeed", c => c.Int(nullable: false));
            AddColumn("dbo.Heroes", "BaseArmor", c => c.Double(nullable: false));
            AddColumn("dbo.Heroes", "BaseMinAttackDamage", c => c.Int(nullable: false));
            AddColumn("dbo.Heroes", "BaseMaxAttackDamage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Heroes", "BaseMaxAttackDamage");
            DropColumn("dbo.Heroes", "BaseMinAttackDamage");
            DropColumn("dbo.Heroes", "BaseArmor");
            DropColumn("dbo.Heroes", "BaseMovementSpeed");
            DropColumn("dbo.Heroes", "IntelligenceGrowth");
            DropColumn("dbo.Heroes", "BaseIntelligence");
            DropColumn("dbo.Heroes", "AgilityGrowth");
            DropColumn("dbo.Heroes", "BaseAgility");
            DropColumn("dbo.Heroes", "StrengthGrowth");
            DropColumn("dbo.Heroes", "BaseStrength");
            DropColumn("dbo.Heroes", "PrimaryAttribute");
        }
    }
}
