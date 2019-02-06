namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatesDatabase : DbMigration
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
            
            AddColumn("dbo.Person", "UserName", c => c.String(nullable: false));
            AddColumn("dbo.Person", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Person", "Password", c => c.String(maxLength: 100));
            AddColumn("dbo.Person", "ConfirmPassword", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Person", "ProfilePictureLink", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseDate", "CourseID", "dbo.Course");
            DropIndex("dbo.CourseDate", new[] { "CourseID" });
            DropColumn("dbo.Person", "ProfilePictureLink");
            DropColumn("dbo.Person", "ConfirmPassword");
            DropColumn("dbo.Person", "Password");
            DropColumn("dbo.Person", "Email");
            DropColumn("dbo.Person", "UserName");
            DropTable("dbo.CourseDate");
        }
    }
}
