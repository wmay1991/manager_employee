namespace Manager_Employee.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Start_DAte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "start_date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "start_date");
        }
    }
}
