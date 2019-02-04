namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelsUpdates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseDate",
                c => new
                    {
                        CourseDateID = c.Int(nullable: false, identity: true),
                        FirstCourse = c.DateTime(nullable: false),
                        Day = c.Int(nullable: false),
                        StartHour = c.String(),
                        Duration = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseDateID)
                .ForeignKey("dbo.Course", t => t.CourseID, cascadeDelete: true)
                .Index(t => t.CourseID);
            
            AddColumn("dbo.Person", "ProfilePictureLink", c => c.String());
            AlterColumn("dbo.Person", "Password", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseDate", "CourseID", "dbo.Course");
            DropIndex("dbo.CourseDate", new[] { "CourseID" });
            AlterColumn("dbo.Person", "Password", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Person", "ProfilePictureLink");
            DropTable("dbo.CourseDate");
        }
    }
}
