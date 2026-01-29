using System.Text;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using NoteCloud_api.Auth.Models;
using NoteCloud_api.Auth.Services;
using NoteCloud_api.Categories.Repository;
using NoteCloud_api.Categories.Service;
using NoteCloud_api.Data;
using NoteCloud_api.Notes.Repository;
using NoteCloud_api.Notes.Service;
using NoteCloud_api.Users.Repository;
using NoteCloud_api.Users.Service;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "NoteCloud API",
                Version = "v1",
                Description = "API for NoteCloud management"
            });

            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Enter 'Bearer {token}'",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
        });

        builder.Services.AddCors(options =>
            options.AddPolicy("notecloud_api", policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod()));

        var connectionString = builder.Configuration.GetConnectionString("Default")!;
        EnsureDatabaseExists(connectionString);

        var jwtSection = builder.Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSection["Key"] ?? "");

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("read:note", p => p.RequireAssertion(ctx =>
                ctx.User.HasClaim("permission", "read:note") || ctx.User.HasClaim("permission", "read")));

            options.AddPolicy("write:note", p => p.RequireAssertion(ctx =>
                ctx.User.HasClaim("permission", "write:note") || ctx.User.HasClaim("permission", "write")));

            options.AddPolicy("read:user", p => p.RequireAssertion(ctx =>
                ctx.User.HasClaim("permission", "read:user") || ctx.User.HasClaim("permission", "read")));

            options.AddPolicy("write:user", p => p.RequireAssertion(ctx =>
                ctx.User.HasClaim("permission", "write:user") || ctx.User.HasClaim("permission", "write")));

            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("User", policy => policy.RequireRole("User"));
        });

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 21))));

        builder.Services.AddScoped<IUserRepo, UserRepo>();
        builder.Services.AddScoped<ICommandServiceUser, CommandServiceUser>();
        builder.Services.AddScoped<IQueryServiceUser, QueryServiceUser>();

        builder.Services.AddScoped<INoteRepo, NoteRepo>();
        builder.Services.AddScoped<ICommandServiceNote, CommandServiceNote>();
        builder.Services.AddScoped<IQueryServiceNote, QueryServiceNote>();

        builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
        builder.Services.AddScoped<ICommandServiceCategory, CommandServiceCategory>();
        builder.Services.AddScoped<IQueryServiceCategory, QueryServiceCategory>();

        builder.Services.AddScoped<IUserAuthenticator, UserAuthenticator>();
        builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        builder.Services.AddScoped<IRolePermissionResolver, RolePermissionResolver>();

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
        builder.Services.Configure<RolePermissionsOptions>(builder.Configuration.GetSection("RolePermissions"));
        builder.Services.Configure<AuthDefaults>(builder.Configuration.GetSection("AuthDefaults"));


        builder.Services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(Program).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        using (var scope = app.Services.CreateScope())
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        app.UseCors("notecloud_api");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }

    private static void EnsureDatabaseExists(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        var builder = new MySqlConnectionStringBuilder(connectionString);
        var database = builder.Database;
        if (string.IsNullOrWhiteSpace(database))
        {
            return;
        }

        builder.Database = string.Empty;

        using var connection = new MySqlConnection(builder.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $"CREATE DATABASE IF NOT EXISTS `{database}`";
        command.ExecuteNonQuery();
    }
}
