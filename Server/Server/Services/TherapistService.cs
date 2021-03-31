using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Models;

namespace Server.Services
{
    public interface ITherapistService
    {
        Task<Therapist> GetTherapist(string id);

        Task<Therapist> Authenticate(string username, string password);
        
        Task<Therapist> AddTherapist(string fullname, string username, string password);
    }
    
    public class TherapistService: ITherapistService
    {
        private readonly AppDbContext _appDbContext;

        public TherapistService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Therapist> GetTherapist(string id)
        {
            var therapist = await _appDbContext
                .Therapists
                .FirstOrDefaultAsync(t=> t.Id.ToString() == id);

            return therapist;
        }

        public async Task<Therapist> Authenticate(string username, string password)
        {
            var therapist = await _appDbContext.Therapists.FirstOrDefaultAsync(t => t.Username == username);
                
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

        public async Task<Therapist> AddTherapist(string fullname, string username, string password)
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