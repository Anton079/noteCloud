using FluentMigrator;
using System.Data;

namespace NoteCloud_api.Data.Migrations
{
    [Migration(280120261)]
    public class CreateSchema : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsString(100).PrimaryKey()
                .WithColumn("firstName").AsString(50).NotNullable()
                .WithColumn("lastName").AsString(50).NotNullable()
                .WithColumn("email").AsString(100).NotNullable().Unique()
                .WithColumn("role").AsString(50).Nullable()
                .WithColumn("passwordHash").AsString(255).Nullable()
                .WithColumn("passwordSalt").AsString(255).Nullable()
                .WithColumn("createdAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("updatedAt").AsDateTime().Nullable()
                .WithColumn("lastLoginAt").AsDateTime().Nullable()
                .WithColumn("refreshToken").AsString(500).Nullable()
                .WithColumn("refreshTokenExpiryTime").AsDateTime().Nullable();

            Create.Table("notes")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("title").AsString(255).NotNullable()
                .WithColumn("content").AsString(5000).NotNullable()
                .WithColumn("category").AsString(100).NotNullable()
                .WithColumn("isFavorite").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("date").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
        }

        public override void Down()
        {
            Delete.Table("notes");
            Delete.Table("users");
        }
    }
}
