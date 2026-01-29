using FluentMigrator;
using NoteCloud_api.Auth.Models;
using NoteCloud_api.Auth.Services;
using NoteCloud_api.System.Id;
using System;
using System.Data;

namespace NoteCloud_api.Data.Migrations
{
    [Migration(290120261)]
    public class InitialSchemaAndSeed : Migration
    {
        public override void Up()
        {
            Execute.Sql("CREATE DATABASE IF NOT EXISTS notecloud_db");

            Create.Table("users")
                .WithColumn("id").AsString(100).PrimaryKey()
                .WithColumn("firstName").AsString(50).NotNullable()
                .WithColumn("lastName").AsString(50).NotNullable()
                .WithColumn("email").AsString(100).NotNullable().Unique()
                .WithColumn("role").AsString(50).NotNullable().WithDefaultValue(SystemRoles.User)
                .WithColumn("passwordHash").AsString(255).Nullable()
                .WithColumn("passwordSalt").AsString(255).Nullable()
                .WithColumn("createdAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("updatedAt").AsDateTime().Nullable()
                .WithColumn("lastLoginAt").AsDateTime().Nullable()
                .WithColumn("refreshToken").AsString(500).Nullable()
                .WithColumn("refreshTokenExpiryTime").AsDateTime().Nullable();

            Execute.Sql("ALTER TABLE users MODIFY id VARCHAR(100) NOT NULL DEFAULT (UUID())");

            Create.Table("categories")
                .WithColumn("id").AsString(100).PrimaryKey()
                .WithColumn("name").AsString(100).NotNullable().Unique();

            Execute.Sql("ALTER TABLE categories MODIFY id VARCHAR(100) NOT NULL DEFAULT (UUID())");

            Create.Table("notes")
                .WithColumn("id").AsString(100).PrimaryKey()
                .WithColumn("title").AsString(255).NotNullable()
                .WithColumn("content").AsString(5000).NotNullable()
                .WithColumn("categoryId").AsString(100).NotNullable()
                .WithColumn("userId").AsString(100).NotNullable()
                .WithColumn("isFavorite").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("date").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);

            Create.ForeignKey("FK_notes_users_userId")
                .FromTable("notes").ForeignColumn("userId")
                .ToTable("users").PrimaryColumn("id")
                .OnDeleteOrUpdate(Rule.Cascade);

            Create.ForeignKey("FK_notes_categories_categoryId")
                .FromTable("notes").ForeignColumn("categoryId")
                .ToTable("categories").PrimaryColumn("id")
                .OnDeleteOrUpdate(Rule.None);

            Create.Index("IX_notes_userId")
                .OnTable("notes")
                .OnColumn("userId");

            Create.Index("IX_notes_categoryId")
                .OnTable("notes")
                .OnColumn("categoryId");

            var adminId = "user-admin";
            var userAnaId = "user-ana";
            var userMihaiId = "user-mihai";

            var catWorkId = "cat-work";
            var catPersonalId = "cat-personal";
            var catIdeasId = "cat-ideas";
            var catTravelId = "cat-travel";

            var adminPassword = PasswordHasher.HashPassword("Admin123!");
            var anaPassword = PasswordHasher.HashPassword("User123!");
            var mihaiPassword = PasswordHasher.HashPassword("User123!");

            Insert.IntoTable("users").Row(new
            {
                id = adminId,
                firstName = "Admin",
                lastName = "User",
                email = "admin@notecloud.local",
                role = SystemRoles.Admin,
                passwordHash = adminPassword.Hash,
                passwordSalt = adminPassword.Salt,
                createdAt = DateTime.UtcNow
            });

            Insert.IntoTable("users").Row(new
            {
                id = userAnaId,
                firstName = "Ana",
                lastName = "Popescu",
                email = "ana@notecloud.local",
                role = SystemRoles.User,
                passwordHash = anaPassword.Hash,
                passwordSalt = anaPassword.Salt,
                createdAt = DateTime.UtcNow
            });

            Insert.IntoTable("users").Row(new
            {
                id = userMihaiId,
                firstName = "Mihai",
                lastName = "Ionescu",
                email = "mihai@notecloud.local",
                role = SystemRoles.User,
                passwordHash = mihaiPassword.Hash,
                passwordSalt = mihaiPassword.Salt,
                createdAt = DateTime.UtcNow
            });

            Insert.IntoTable("categories").Row(new { id = catWorkId, name = "work" });
            Insert.IntoTable("categories").Row(new { id = catPersonalId, name = "personal" });
            Insert.IntoTable("categories").Row(new { id = catIdeasId, name = "ideas" });
            Insert.IntoTable("categories").Row(new { id = catTravelId, name = "travel" });

            Insert.IntoTable("notes").Row(new
            {
                id = IdGenerator.New("note"),
                title = "Plan pentru saptamana",
                content = "Finalizeaza API-ul, scrie teste, pregateste demo.",
                categoryId = catWorkId,
                userId = userAnaId,
                isFavorite = true,
                date = DateTime.UtcNow.AddDays(-2)
            });

            Insert.IntoTable("notes").Row(new
            {
                id = IdGenerator.New("note"),
                title = "Lista cumparaturi",
                content = "Lapte, paine, cafea, legume.",
                categoryId = catPersonalId,
                userId = userAnaId,
                isFavorite = false,
                date = DateTime.UtcNow.AddDays(-1)
            });

            Insert.IntoTable("notes").Row(new
            {
                id = IdGenerator.New("note"),
                title = "Idei proiect",
                content = "Aplicatie note cu tag-uri si cautare full-text.",
                categoryId = catIdeasId,
                userId = userMihaiId,
                isFavorite = true,
                date = DateTime.UtcNow.AddDays(-3)
            });

            Insert.IntoTable("notes").Row(new
            {
                id = IdGenerator.New("note"),
                title = "Plan calatorie",
                content = "Bilete, cazare, itinerar pe zile.",
                categoryId = catTravelId,
                userId = userMihaiId,
                isFavorite = false,
                date = DateTime.UtcNow.AddDays(-5)
            });
        }

        public override void Down()
        {
            Delete.Table("notes");
            Delete.Table("categories");
            Delete.Table("users");
        }
    }
}
