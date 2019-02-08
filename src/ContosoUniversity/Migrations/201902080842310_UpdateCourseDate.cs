namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCourseDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CourseDate", "StartHour", c => c.Int(nullable: false));
            AlterColumn("dbo.CourseDate", "Duration", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CourseDate", "Duration", c => c.Int(nullable: false));
            AlterColumn("dbo.CourseDate", "StartHour", c => c.String());
        }
    }
}
