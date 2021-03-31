using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Models;
using Server.Models;

namespace Server.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllPatients(Therapist therapist);
        Task<PatientDto> AddPatient(Therapist therapist, AddPatientDto dto);
        Task<PatientDto> GetPatient(string id);
        Task<PatientDto> EditPatient(PatientDto userDto);
        Task RemovePatient(string id);
        Task RefreshPatientToken(string id);
        Task<Patient> GetPatientByToken(string token);
    }

    internal class PatientService: IPatientService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public PatientService(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        
        public async Task<PatientDto> AddPatient(Therapist therapist, AddPatientDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.Nhc is null && dto.Zip is null)
            {
                throw new ArgumentException("Nhc or zip must be not null");
            }

            if (await _context.Patients.Where(u => u.Nhc == dto.Nhc).AnyAsync())
            {
                throw new ArgumentException("This patient already exists in the db");
            }

            var patient = new Patient
            {
                Therapist = therapist,
                Nhc = dto.Nhc,
                Zip = dto.Zip,
                Token = await _tokenService.GenerateToken(),
                FullName = System.IO.Path.GetRandomFileName() // TODO Get name from the hospital api service
            };

            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();

            return new PatientDto
            {
                Id = patient.Id.ToString(),
                Name = patient.FullName,
                Nhc = patient.Nhc,
                Token = patient.Token,
                Zip = patient.Zip
            };
        }

        public async Task<List<PatientDto>> GetAllPatients(Therapist therapist)
        {
            var dbUsers = await _context.Patients
                .Where(p=> p.Therapist == therapist)
                .ToListAsync();

            return dbUsers.Select(patient =>  new PatientDto
            {
                Id = patient.Id.ToString(),
                Name = patient.FullName,
                Nhc = patient.Nhc,
                Token = patient.Token,
                Zip = patient.Zip
            }).ToList();
        }

        public async Task<PatientDto> GetPatient(string id)
        {
            var u = await _context.Patients.FindAsync(new Guid(id));

            return new PatientDto()
            {
                Id = u.Id.ToString(),
                Name = u.FullName,
                Nhc = u.Nhc,
                Token = u.Token,
                Zip = u.Zip
            };
        }

        public async Task<PatientDto> EditPatient(PatientDto patientDto)
        {
            var u = await _context.Patients.FindAsync(new Guid(patientDto.Id));

            u.FullName = patientDto.Name;
            u.Zip = patientDto.Zip;
            u.Nhc = patientDto.Nhc;

            await _context.SaveChangesAsync();

            return patientDto;
        }

        public async Task RemovePatient(string id)
        {
            var p = await _context.Patients.FindAsync(new Guid(id));
            
            _context.Patients.Remove(p);
            
            await _context.SaveChangesAsync();
        }

        public async Task RefreshPatientToken(string id)
        {
            var u = await _context.Patients.FindAsync(new Guid(id));

            u.Token = await _tokenService.GenerateToken();
            
            await _context.SaveChangesAsync();
        }

        public async Task<Patient> GetPatientByToken(string token)
        {
            return await _context.Patients
                .Where(u => u.Token == token)
                .FirstOrDefaultAsync();
        }
    }
}