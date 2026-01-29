using FluentMigrator;
using NoteCloud_api.Auth.Models;
using NoteCloud_api.Auth.Services;
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

            Execute.Sql(@"
                CREATE TABLE `users` (
                    `id` BINARY(16) NOT NULL DEFAULT (UUID_TO_BIN(UUID())),
                    `firstName` NVARCHAR(50) NOT NULL,
                    `lastName` NVARCHAR(50) NOT NULL,
                    `email` NVARCHAR(100) NOT NULL,
                    `role` NVARCHAR(50) NOT NULL DEFAULT 'User',
                    `passwordHash` NVARCHAR(255),
                    `passwordSalt` NVARCHAR(255),
                    `createdAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    `updatedAt` DATETIME,
                    `lastLoginAt` DATETIME,
                    `refreshToken` NVARCHAR(500),
                    `refreshTokenExpiryTime` DATETIME,
                    CONSTRAINT `PK_users` PRIMARY KEY (`id`),
                    CONSTRAINT `UQ_users_email` UNIQUE (`email`)
                ) ENGINE=InnoDB;
            ");

            Execute.Sql(@"
                CREATE TABLE `categories` (
                    `id` BINARY(16) NOT NULL DEFAULT (UUID_TO_BIN(UUID())),
                    `name` NVARCHAR(100) NOT NULL,
                    CONSTRAINT `PK_categories` PRIMARY KEY (`id`),
                    CONSTRAINT `UQ_categories_name` UNIQUE (`name`)
                ) ENGINE=InnoDB;
            ");

            Execute.Sql(@"
                CREATE TABLE `notes` (
                    `id` BINARY(16) NOT NULL DEFAULT (UUID_TO_BIN(UUID())),
                    `title` NVARCHAR(255) NOT NULL,
                    `content` NVARCHAR(5000) NOT NULL,
                    `categoryId` BINARY(16) NOT NULL,
                    `userId` BINARY(16) NOT NULL,
                    `isFavorite` BIT(1) NOT NULL DEFAULT 0,
                    `date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    CONSTRAINT `PK_notes` PRIMARY KEY (`id`)
                ) ENGINE=InnoDB;
            ");

            Execute.Sql("ALTER TABLE `notes` ADD CONSTRAINT `FK_notes_users_userId` FOREIGN KEY (`userId`) REFERENCES `users`(`id`) ON DELETE CASCADE");
            Execute.Sql("ALTER TABLE `notes` ADD CONSTRAINT `FK_notes_categories_categoryId` FOREIGN KEY (`categoryId`) REFERENCES `categories`(`id`)");
            Execute.Sql("CREATE INDEX `IX_notes_userId` ON `notes` (`userId`)");
            Execute.Sql("CREATE INDEX `IX_notes_categoryId` ON `notes` (`categoryId`)");

            var adminPassword = PasswordHasher.HashPassword("Admin123!");
            var anaPassword = PasswordHasher.HashPassword("User123!");
            var mihaiPassword = PasswordHasher.HashPassword("User123!");

            var adminId = "11111111-1111-1111-1111-111111111111";
            var userAnaId = "22222222-2222-2222-2222-222222222222";
            var userMihaiId = "33333333-3333-3333-3333-333333333333";

            var catWorkId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
            var catPersonalId = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb";
            var catIdeasId = "cccccccc-cccc-cccc-cccc-cccccccccccc";
            var catTravelId = "dddddddd-dddd-dddd-dddd-dddddddddddd";

            Execute.Sql($@"
                INSERT INTO users (id, firstName, lastName, email, role, passwordHash, passwordSalt, createdAt)
                VALUES
                    (UUID_TO_BIN('{adminId}'), 'Admin', 'User', 'admin@notecloud.local', '{SystemRoles.Admin}', '{adminPassword.Hash}', '{adminPassword.Salt}', UTC_TIMESTAMP()),
                    (UUID_TO_BIN('{userAnaId}'), 'Ana', 'Popescu', 'ana@notecloud.local', '{SystemRoles.User}', '{anaPassword.Hash}', '{anaPassword.Salt}', UTC_TIMESTAMP()),
                    (UUID_TO_BIN('{userMihaiId}'), 'Mihai', 'Ionescu', 'mihai@notecloud.local', '{SystemRoles.User}', '{mihaiPassword.Hash}', '{mihaiPassword.Salt}', UTC_TIMESTAMP());

                INSERT INTO categories (id, name)
                VALUES
                    (UUID_TO_BIN('{catWorkId}'), 'work'),
                    (UUID_TO_BIN('{catPersonalId}'), 'personal'),
                    (UUID_TO_BIN('{catIdeasId}'), 'ideas'),
                    (UUID_TO_BIN('{catTravelId}'), 'travel');

                INSERT INTO notes (id, title, content, categoryId, userId, isFavorite, date)
                VALUES
                    (UUID_TO_BIN(UUID()), 'Plan pentru saptamana', 'Finalizeaza API-ul, scrie teste, pregateste demo.', UUID_TO_BIN('{catWorkId}'), UUID_TO_BIN('{userAnaId}'), 1, UTC_TIMESTAMP() - INTERVAL 2 DAY),
                    (UUID_TO_BIN(UUID()), 'Lista cumparaturi', 'Lapte, paine, cafea, legume.', UUID_TO_BIN('{catPersonalId}'), UUID_TO_BIN('{userAnaId}'), 0, UTC_TIMESTAMP() - INTERVAL 1 DAY),
                    (UUID_TO_BIN(UUID()), 'Idei proiect', 'Aplicatie note cu tag-uri si cautare full-text.', UUID_TO_BIN('{catIdeasId}'), UUID_TO_BIN('{userMihaiId}'), 1, UTC_TIMESTAMP() - INTERVAL 3 DAY),
                    (UUID_TO_BIN(UUID()), 'Plan calatorie', 'Bilete, cazare, itinerar pe zile.', UUID_TO_BIN('{catTravelId}'), UUID_TO_BIN('{userMihaiId}'), 0, UTC_TIMESTAMP() - INTERVAL 5 DAY);
            ");
        }

        public override void Down()
        {
            Delete.Table("notes");
            Delete.Table("categories");
            Delete.Table("users");
        }
    }
}
