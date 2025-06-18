using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Interfaces.Repositories;
using FimwiApi.Core.Interfaces.Services;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Models.Responses;
using FimwiApi.Core.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Security.Cryptography;
using FimwiApi.Core.Interfaces;

namespace FimwiApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {id} not found");
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<PagedResponse<UserDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var users = await _userRepository.GetAllAsync();
            var totalRecords = users.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var pagedData = users.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedResponse<UserDto>
            {
                Data = _mapper.Map<IEnumerable<UserDto>>(pagedData),
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException($"User with email {email} not found");
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
            if (existingUser != null)
            {
                throw new ValidationException("A user with this email already exists");
            }

            var role = await _roleRepository.GetByIdAsync(createUserDto.RoleId);
            if (role == null)
            {
                throw new ValidationException("Invalid role ID");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = createUserDto.Username,
                Email = createUserDto.Email,
                PasswordHash = HashPassword(createUserDto.Password),
                RoleId = createUserDto.RoleId,
                EmailConfirmed = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateAsync(UserDto userDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(userDto.Id);
            if (existingUser == null)
            {
                throw new NotFoundException($"User with ID {userDto.Id} not found");
            }

            var duplicateUser = await _userRepository.GetByEmailAsync(userDto.Email);
            if (duplicateUser != null && duplicateUser.Id != userDto.Id)
            {
                throw new ValidationException("A user with this email already exists");
            }

            var user = _mapper.Map<User>(userDto);
            user.UpdatedAt = DateTime.UtcNow;
            user.PasswordHash = existingUser.PasswordHash; // Preserve existing password hash
            user.EmailConfirmed = existingUser.EmailConfirmed; // Preserve email confirmation status

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {id} not found");
            }

            await _userRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            return VerifyPassword(password, user.PasswordHash);
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found");
            }

            if (!VerifyPassword(currentPassword, user.PasswordHash))
            {
                throw new ValidationException("Current password is incorrect");
            }

            user.PasswordHash = HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException($"User with email {email} not found");
            }

            // Generate reset token and send email
            // Implementation depends on your email service
            return true;
        }

        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException($"User with email {email} not found");
            }

            // Validate token and confirm email
            // Implementation depends on your token validation logic
            user.EmailConfirmed = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
} 