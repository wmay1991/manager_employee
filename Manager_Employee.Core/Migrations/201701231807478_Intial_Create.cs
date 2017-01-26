namespace Manager_Employee.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Intial_Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        mgr_id = c.Guid(nullable: false),
                        first_name = c.String(),
                        last_name = c.String(),
                    })
                .PrimaryKey(t => t.mgr_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Managers");
        }
    }
}
