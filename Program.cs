using FastDrive.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Authorization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddDbContext<FastDriveContext>(
        o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add(new AuthorizeFilter()); //Implementing this, all controllers and action methos will require authorization by default
        })
        .AddNewtonsoftJson()
        .AddJsonOptions(options =>
        {
            // Configure JSON serialization to retain property names as defined in the C# model.
            // This disables the default camelCase naming policy.
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
        // Configure JWT authentication in the application.
        builder.Services.AddAuthentication(options =>
        {
            // Set the default authentication scheme to JWT Bearer.
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer("Bearer", options =>
        {
            // Configure JWT token validation parameters.
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // Skip validating the token issuer 
                    ValidateAudience = true, // Skip validating the token audience 
                    ValidateIssuerSigningKey = true, // Ensure the token's signing key is valid.
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Use a symmetric key from configuration for token validation.
                };
        });



        builder.Services.AddEndpointsApiExplorer(); //Enables discovery of minimal API endpoints for tools like Swagger
        builder.Services.AddSwaggerGen();
        

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}