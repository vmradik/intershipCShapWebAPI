namespace StoreApp.WebApi.Simple.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameActionToActivity : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Actions", newName: "Activities");
            DropForeignKey("dbo.Comments", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Actions", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.Actions", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Activities", new[] { "User_Id" });
            DropIndex("dbo.Activities", new[] { "Task_Id" });
            DropIndex("dbo.Tasks", new[] { "Project_Id" });
            DropIndex("dbo.Comments", new[] { "Task_Id" });
            RenameColumn(table: "dbo.Comments", name: "Task_Id", newName: "TaskId");
            RenameColumn(table: "dbo.Tasks", name: "Project_Id", newName: "ProjectId");
            RenameColumn(table: "dbo.Comments", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Activities", name: "Task_Id", newName: "TaskId");
            RenameColumn(table: "dbo.Activities", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Comments", name: "IX_User_Id", newName: "IX_UserId");
            AddColumn("dbo.Projects", "nameololo", c => c.String());
            AddColumn("dbo.Projects", "nameololo2", c => c.String());
            AlterColumn("dbo.Activities", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Activities", "TaskId", c => c.Long(nullable: false));
            AlterColumn("dbo.Tasks", "ProjectId", c => c.Long(nullable: false));
            AlterColumn("dbo.Comments", "TaskId", c => c.Long(nullable: false));
            CreateIndex("dbo.Activities", "TaskId");
            CreateIndex("dbo.Activities", "UserId");
            CreateIndex("dbo.Tasks", "ProjectId");
            CreateIndex("dbo.Comments", "TaskId");
            AddForeignKey("dbo.Comments", "TaskId", "dbo.Tasks", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tasks", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Activities", "TaskId", "dbo.Tasks", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Activities", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Activities", "TaskId", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Comments", "TaskId", "dbo.Tasks");
            DropIndex("dbo.Comments", new[] { "TaskId" });
            DropIndex("dbo.Tasks", new[] { "ProjectId" });
            DropIndex("dbo.Activities", new[] { "UserId" });
            DropIndex("dbo.Activities", new[] { "TaskId" });
            AlterColumn("dbo.Comments", "TaskId", c => c.Long());
            AlterColumn("dbo.Tasks", "ProjectId", c => c.Long());
            AlterColumn("dbo.Activities", "TaskId", c => c.Long());
            AlterColumn("dbo.Activities", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Projects", "nameololo2");
            DropColumn("dbo.Projects", "nameololo");
            RenameIndex(table: "dbo.Comments", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Activities", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Activities", name: "TaskId", newName: "Task_Id");
            RenameColumn(table: "dbo.Comments", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Tasks", name: "ProjectId", newName: "Project_Id");
            RenameColumn(table: "dbo.Comments", name: "TaskId", newName: "Task_Id");
            CreateIndex("dbo.Comments", "Task_Id");
            CreateIndex("dbo.Tasks", "Project_Id");
            CreateIndex("dbo.Activities", "Task_Id");
            CreateIndex("dbo.Activities", "User_Id");
            AddForeignKey("dbo.Actions", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Actions", "Task_Id", "dbo.Tasks", "Id");
            AddForeignKey("dbo.Tasks", "Project_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.Comments", "Task_Id", "dbo.Tasks", "Id");
            RenameTable(name: "dbo.Activities", newName: "Actions");
        }
    }
}
