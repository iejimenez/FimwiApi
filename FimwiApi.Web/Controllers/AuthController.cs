using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using FimwiApi.Core.Common;
using FimwiApi.Core.Exceptions;
using FimwiApi.Application.Services;

namespace FimwiApi.Web.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var (user, token) = await _authService.LoginAsync(loginDto);
            if (user != null)
            {
                return Ok(new { user, token });
            }
            return BadRequest("Invalid login attempt.");
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _authService.LogoutAsync(token);
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var (user, token) = await _authService.RegisterAsync(registerDto);
            if (user != null)
            {
                return Ok(new { user, token });
            }
            return BadRequest("Registration failed.");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var token = await _authService.RefreshTokenAsync(refreshTokenDto);
            if (!string.IsNullOrEmpty(token))
            {
                return Ok(new { token });
            }
            return BadRequest("Token refresh failed.");
        }

        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto revokeTokenDto)
        {
            await _authService.RevokeTokenAsync(revokeTokenDto);
            return Ok();
        }
    }
} 