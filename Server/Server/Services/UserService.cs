using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Models;

namespace Server.Services
{
    public class CreateUserDto
    {
        public string Nhc { get; set; }
        public string Zip { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Nhc { get; set; }
        public string Zip { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
    }

    public interface IUserService
    {
        Task<User> CreateNewUser(CreateUserDto dto);

        Task<List<UserDto>> GetAllUsers();

        Task<UserDto> GetUser(int id);
    }

    internal class UserService: IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateNewUser(CreateUserDto dto)
        {
            // Validate all the arguments
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.Nhc is null && dto.Zip is null)
            {
                throw new ArgumentException("Nhc or zip must be not null");
            }

            // Check if user with this Nhc already exists
            if (await _context.Users.Where(u => u.Nhc == dto.Nhc).AnyAsync())
            {
                throw new ArgumentException("User already exists");
            }

            var newUser = new User
            {
                Nhc = dto.Nhc,
                Zip = dto.Zip,
                Token = "asd", // Get token from token service
                Name = "asd" // Get name from hospital service
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var dbUsers = await _context.Users.ToListAsync();

            return dbUsers.Select(u =>  new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Nhc = u.Nhc,
                Token = u.Token,
                Zip = u.Zip
            }).ToList();
        }

        public async Task<UserDto> GetUser(int id)
        {
            var u = await _context.Users.FindAsync(id);

            return new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Nhc = u.Nhc,
                Token = u.Token,
                Zip = u.Zip
            };
        }
    }
}