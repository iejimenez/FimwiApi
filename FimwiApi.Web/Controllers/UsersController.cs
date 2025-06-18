using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using FimwiApi.Core.Models.Responses;
using FimwiApi.Core.Entities;

namespace FimwiApi.Web.Controllers
{
    /*
    Estructura de los DTOs y respuestas:

    1. PagedResponse<UserDto> (GET /api/v1/users):
    {
        "data": [
            {
                "id": "guid",
                "username": "string",
                "email": "string",
                "roleId": "guid",
                "role": {
                    "id": "guid",
                    "name": "string"
                },
                "emailConfirmed": boolean,
                "createdAt": "datetime",
                "updatedAt": "datetime"
            }
        ],
        "pageNumber": integer,
        "pageSize": integer,
        "totalPages": integer,
        "totalRecords": integer,
        "hasPreviousPage": boolean,
        "hasNextPage": boolean
    }

    2. UserDto (GET /api/v1/users/{id}, PUT /api/v1/users/{id}):
    {
        "id": "guid",
        "username": "string",
        "email": "string",
        "roleId": "guid",
        "role": {
            "id": "guid",
            "name": "string"
        },
        "emailConfirmed": boolean,
        "createdAt": "datetime",
        "updatedAt": "datetime"
    }

    3. CreateUserDto (POST /api/v1/users):
    {
        "username": "string",
        "email": "string",
        "password": "string",
        "roleId": "guid"
    }

    Códigos de respuesta:
    - 200: OK (GET, PUT)
    - 201: Created (POST)
    - 204: No Content (DELETE)
    - 400: Bad Request
    - 401: Unauthorized
    - 404: Not Found
    */

    /// <summary>
    /// Controlador para la gestión de usuarios
    /// </summary>
    [ApiController]
    [Route("api/v1/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Obtiene una lista paginada de usuarios
        /// </summary>
        /// <param name="pageNumber">Número de página (por defecto: 1)</param>
        /// <param name="pageSize">Tamaño de página (por defecto: 10)</param>
        /// <returns>Lista paginada de usuarios</returns>
        /// <response code="200">Retorna la lista de usuarios</response>
        /// <response code="401">No autorizado</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<UserDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<PagedResponse<UserDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _userService.GetAllAsync(pageNumber, pageSize);
            return Ok(response);
        }

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Usuario encontrado</returns>
        /// <response code="200">Retorna el usuario</response>
        /// <response code="404">Usuario no encontrado</response>
        /// <response code="401">No autorizado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        /// <param name="createUserDto">Datos del usuario a crear</param>
        /// <returns>Usuario creado</returns>
        /// <response code="201">Usuario creado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="401">No autorizado</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
        {
            var user = await _userService.CreateAsync(createUserDto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <param name="userDto">Datos actualizados del usuario</param>
        /// <returns>Usuario actualizado</returns>
        /// <response code="200">Usuario actualizado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="404">Usuario no encontrado</response>
        /// <response code="401">No autorizado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserDto userDto)
        {
            userDto.Id = id;
            var user = await _userService.UpdateAsync(userDto);
            return Ok(user);
        }

        /// <summary>
        /// Elimina un usuario
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Sin contenido</returns>
        /// <response code="204">Usuario eliminado exitosamente</response>
        /// <response code="404">Usuario no encontrado</response>
        /// <response code="401">No autorizado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
} 