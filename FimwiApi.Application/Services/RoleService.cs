using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Interfaces.Repositories;
using FimwiApi.Core.Interfaces.Services;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Exceptions;
using FimwiApi.Core.Interfaces;
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoleDto> GetByIdAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new NotFoundException($"Role with ID {id} not found");
            }
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<PagedResponse<RoleDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var roles = await _roleRepository.GetAllAsync();
            var totalRecords = roles.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            
            var pagedRoles = roles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(pagedRoles);

            return new PagedResponse<RoleDto>
            {
                Data = roleDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            };
        }

        public async Task<RoleDto> CreateAsync(RoleDto roleDto)
        {
            var existingRole = await _roleRepository.GetByNameAsync(roleDto.Name);
            if (existingRole != null)
            {
                throw new ValidationException("A role with this name already exists");
            }

            var role = _mapper.Map<Role>(roleDto);
            role.CreatedAt = DateTime.UtcNow;
            role.UpdatedAt = DateTime.UtcNow;

            await _roleRepository.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto> UpdateAsync(RoleDto roleDto)
        {
            var existingRole = await _roleRepository.GetByIdAsync(roleDto.Id);
            if (existingRole == null)
            {
                throw new NotFoundException($"Role with ID {roleDto.Id} not found");
            }

            var duplicateRole = await _roleRepository.GetByNameAsync(roleDto.Name);
            if (duplicateRole != null && duplicateRole.Id != roleDto.Id)
            {
                throw new ValidationException("A role with this name already exists");
            }

            var role = _mapper.Map<Role>(roleDto);
            role.UpdatedAt = DateTime.UtcNow;
            role.CreatedAt = existingRole.CreatedAt; // Preserve original creation date

            await _roleRepository.UpdateAsync(role);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RoleDto>(role);
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new NotFoundException($"Role with ID {id} not found");
            }

            await _roleRepository.DeleteAsync(role);
            await _unitOfWork.SaveChangesAsync();
        }
    }
} 