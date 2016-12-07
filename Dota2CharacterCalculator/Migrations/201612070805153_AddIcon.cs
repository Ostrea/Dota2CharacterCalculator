namespace Dota2CharacterCalculator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIcon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Heroes", "Icon", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Heroes", "Icon");
        }
    }
}
