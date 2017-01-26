namespace Manager_Employee.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Dept : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "dept", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "dept");
        }
    }
}
