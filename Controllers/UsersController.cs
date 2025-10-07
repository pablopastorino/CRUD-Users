using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Controllers
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Range(18, 100)]
        public int Age { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;
    }

    public class CreateUserRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Range(18, 100)]
        public int Age { get; set; }
    }

    public class UpdateUserRequest
    {
        [StringLength(100, MinimumLength = 2)]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Range(18, 100)]
        public int? Age { get; set; }

        public bool? IsActive { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // Simulamos una base de datos en memoria
        private static List<User> _users = new()
        {
            new User { Id = 1, Name = "Juan Pérez", Email = "juan@email.com", Age = 25 },
            new User { Id = 2, Name = "María García", Email = "maria@email.com", Age = 30 },
            new User { Id = 3, Name = "Carlos López", Email = "carlos@email.com", Age = 28 }
        };

        private static int _nextId = 4;

        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        [HttpGet]
        [ProducesResponseType<List<User>>(StatusCodes.Status200OK)]
        public ActionResult<List<User>> GetAllUsers()
        {
            return Ok(_users.Where(u => u.IsActive).ToList());
        }

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Usuario encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType<User>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetUserById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id && u.IsActive);

            if (user == null)
            {
                return NotFound(new { message = $"Usuario con ID {id} no encontrado" });
            }

            return Ok(user);
        }

        /// <summary>
        /// Busca usuarios por nombre o email
        /// </summary>
        /// <param name="search">Término de búsqueda</param>
        /// <returns>Lista de usuarios que coinciden</returns>
        [HttpGet("search")]
        [ProducesResponseType<List<User>>(StatusCodes.Status200OK)]
        public ActionResult<List<User>> SearchUsers([FromQuery] string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return Ok(_users.Where(u => u.IsActive).ToList());
            }

            var results = _users
                .Where(u => u.IsActive &&
                       (u.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        u.Email.Contains(search, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return Ok(results);
        }

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        /// <param name="request">Datos del usuario a crear</param>
        /// <returns>Usuario creado</returns>
        [HttpPost]
        [ProducesResponseType<User>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<User> CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar si el email ya existe
            if (_users.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return Conflict(new { message = "Ya existe un usuario con ese email" });
            }

            var newUser = new User
            {
                Id = _nextId++,
                Name = request.Name,
                Email = request.Email,
                Age = request.Age,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _users.Add(newUser);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = newUser.Id },
                newUser
            );
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        /// <param name="id">ID del usuario a actualizar</param>
        /// <param name="request">Datos a actualizar</param>
        /// <returns>Usuario actualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType<User>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<User> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _users.FirstOrDefault(u => u.Id == id && u.IsActive);

            if (user == null)
            {
                return NotFound(new { message = $"Usuario con ID {id} no encontrado" });
            }

            // Verificar email único si se está actualizando
            if (!string.IsNullOrEmpty(request.Email) &&
                _users.Any(u => u.Id != id && u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return Conflict(new { message = "Ya existe un usuario con ese email" });
            }

            // Actualizar solo los campos enviados
            if (!string.IsNullOrEmpty(request.Name))
                user.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Email))
                user.Email = request.Email;

            if (request.Age.HasValue)
                user.Age = request.Age.Value;

            if (request.IsActive.HasValue)
                user.IsActive = request.IsActive.Value;

            return Ok(user);
        }

        /// <summary>
        /// Actualiza parcialmente un usuario (PATCH)
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <param name="request">Campos a actualizar</param>
        /// <returns>Usuario actualizado</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType<User>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> PatchUser(int id, [FromBody] UpdateUserRequest request)
        {
            return UpdateUser(id, request); // Reutilizamos la lógica del PUT
        }

        /// <summary>
        /// Elimina un usuario (soft delete)
        /// </summary>
        /// <param name="id">ID del usuario a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id && u.IsActive);

            if (user == null)
            {
                return NotFound(new { message = $"Usuario con ID {id} no encontrado" });
            }

            // Soft delete - marcamos como inactivo en lugar de eliminar
            user.IsActive = false;

            return NoContent();
        }

        /// <summary>
        /// Elimina permanentemente un usuario
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Confirmación de eliminación</returns>
        [HttpDelete("{id}/permanent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteUserPermanent(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new { message = $"Usuario con ID {id} no encontrado" });
            }

            _users.Remove(user);

            return NoContent();
        }

        /// <summary>
        /// Reactiva un usuario eliminado
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Usuario reactivado</returns>
        [HttpPatch("{id}/activate")]
        [ProducesResponseType<User>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> ActivateUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new { message = $"Usuario con ID {id} no encontrado" });
            }

            user.IsActive = true;
            return Ok(user);
        }

        /// <summary>
        /// Obtiene estadísticas de usuarios
        /// </summary>
        /// <returns>Estadísticas</returns>
        [HttpGet("stats")]
        [ProducesResponseType<object>(StatusCodes.Status200OK)]
        public ActionResult GetUserStats()
        {
            var stats = new
            {
                TotalUsers = _users.Count,
                ActiveUsers = _users.Count(u => u.IsActive),
                InactiveUsers = _users.Count(u => !u.IsActive),
                AverageAge = _users.Where(u => u.IsActive).Average(u => u.Age),
                NewestUser = _users.Where(u => u.IsActive).OrderByDescending(u => u.CreatedAt).FirstOrDefault()?.Name,
                OldestUser = _users.Where(u => u.IsActive).OrderBy(u => u.CreatedAt).FirstOrDefault()?.Name
            };

            return Ok(stats);
        }
    }
}
