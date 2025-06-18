using System;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FimwiApi.Infrastructure.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IConfiguration configuration,
            ILogger<DbInitializer> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Aplicar migraciones pendientes
                await _context.Database.MigrateAsync();

                // Crear roles si no existen
                await CreateRolesAsync();

                // Crear usuario administrador si no existe
                await CreateAdminUserAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database");
                throw;
            }
        }

        private async Task CreateRolesAsync()
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var role = new Role
                    {
                        Name = roleName,
                        Description = $"Role for {roleName}",
                        CreatedAt = DateTime.UtcNow
                    };
                    await _roleManager.CreateAsync(role);
                }
            }
        }

        private async Task CreateAdminUserAsync()
        {
            var adminSection = _configuration.GetSection("AdminUser");
            var adminEmail = adminSection["Email"];
            var adminPassword = adminSection["Password"];

            _logger.LogInformation("Configuration values - Email: {Email}, Password: {Password}", 
                adminEmail ?? "null", 
                string.IsNullOrEmpty(adminPassword) ? "null" : "****");

            if (string.IsNullOrEmpty(adminEmail))
            {
                _logger.LogError("Admin email is null or empty");
                throw new Exception("Admin user email not configured in appsettings.json");
            }

            if (string.IsNullOrEmpty(adminPassword))
            {
                _logger.LogError("Admin password is null or empty");
                throw new Exception("Admin user password not configured in appsettings.json");
            }

            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                _logger.LogInformation("Admin user not found, creating new admin user");
                var adminRole = await _roleManager.FindByNameAsync("Admin");
                if (adminRole == null)
                {
                    _logger.LogError("Admin role not found");
                    throw new Exception("Admin role not found");
                }

                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    RoleId = adminRole.Id,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to create admin user: {Errors}", errors);
                    throw new Exception($"Failed to create admin user: {errors}");
                }

                _logger.LogInformation("Admin user created successfully");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
                _logger.LogInformation("Admin user already exists");
            }
        }
    }
} 