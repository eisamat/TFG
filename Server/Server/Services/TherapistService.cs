using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Models;

namespace Server.Services
{
    public interface ITherapistService
    {
        Task<Therapist> ValidateCredentials(string username, string password);
        
        Task<Therapist> Get(string id);
        Task<Therapist> Add(string fullname, string username, string password);
    }
    
    public class TherapistService: ITherapistService
    {
        private readonly AppDbContext _appDbContext;

        public TherapistService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        
        public async Task<Therapist> ValidateCredentials(string username, string password)
        {
            var therapist = await _appDbContext
                .Therapists
                .Where(t => t.Username == username)
                .FirstOrDefaultAsync();
                
            if (therapist == null)
            {
                throw new ArgumentException("User does not exists");
            }
            
            var verified = BCrypt.Net.BCrypt.Verify(password, therapist.Password);

            if (!verified)
            {
                throw new ArgumentException("Invalid username or password");
            }
            
            return therapist;
        }

        public async Task<Therapist> Get(string id)
        {
            return await _appDbContext
                .Therapists
                .Where(t => t.Id == Guid.Parse(id))
                .FirstOrDefaultAsync();
        }

        public async Task<Therapist> Add(string fullname, string username, string password)
        {
            if (await _appDbContext.Therapists.AnyAsync(t => t.Username == username))
            {
                throw new ArgumentException("User already exists");
            }
            
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var therapist = new Therapist
            {
                Password = passwordHash,
                Username = username,
                FullName = fullname
            };

            await _appDbContext.Therapists.AddAsync(therapist);
            await _appDbContext.SaveChangesAsync();

            return therapist;
        }
    }
}