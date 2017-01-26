namespace Manager_Employee.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        emp_id = c.Guid(nullable: false),
                        first_name = c.String(),
                        last_name = c.String(),
                        mgr_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.emp_id)
                .ForeignKey("dbo.Managers", t => t.mgr_id, cascadeDelete: true)
                .Index(t => t.mgr_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "mgr_id", "dbo.Managers");
            DropIndex("dbo.Employees", new[] { "mgr_id" });
            DropTable("dbo.Employees");
        }
    }
}
