namespace Manager_Employee.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Start_Date : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "start_date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "start_date", c => c.DateTime(nullable: false));
        }
    }
}
