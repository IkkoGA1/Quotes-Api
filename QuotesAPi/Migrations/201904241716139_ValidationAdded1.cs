namespace QuotesAPi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValidationAdded1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Quotes", "Description", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quotes", "Description", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
