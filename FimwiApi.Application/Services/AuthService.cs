using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FimwiApi.Core.Configuration;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Exceptions;
using FimwiApi.Core.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using FimwiApi.Core.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using FimwiApi.Core.Interfaces.Repositories;
using System.Collections.Generic;

namespace FimwiApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IOptions<JwtSettings> jwtSettings,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<(User user, string token)> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                throw new UnauthorizedException("Invalid password");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, roles);
            return (user, token);
        }

        public Task LogoutAsync(string token)
        {
            // TODO: Implement token invalidation if needed
            return Task.CompletedTask;
        }

        public Task<bool> ValidateTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public async Task<(User user, string token)> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new AppException("Email already registered");
            }

            var defaultRole = await _roleRepository.GetByNameAsync("User");
            if (defaultRole == null)
            {
                throw new AppException("Default role not found");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                RoleId = defaultRole.Id,
                EmailConfirmed = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);
            // Aquí podrías guardar cambios si usas UnitOfWork

            var roles = new List<string> { user.Role?.Name ?? "User" };
            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, roles);
            return (user, token);
        }

        public Task<string> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            // Implementación básica: podrías validar el refreshToken y generar un nuevo JWT
            // Aquí solo se retorna un nuevo token para el usuario si el refreshToken es válido
            // (esto es un ejemplo, deberías tener una lógica real de refresh tokens)
            throw new NotImplementedException("RefreshToken logic not implemented");
        }

        public Task RevokeTokenAsync(RevokeTokenDto revokeTokenDto)
        {
            // Implementación básica: podrías invalidar el refreshToken en la base de datos
            // (esto es un ejemplo, deberías tener una lógica real de revocación de tokens)
            throw new NotImplementedException("RevokeToken logic not implemented");
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
} 