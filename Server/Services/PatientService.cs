using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Models;
using Server.Models;
using Server.ViewModels;

namespace Server.Services
{
    public interface IPatientService
    {
        Task<List<Patient>> List(Therapist therapist);
        Task<Patient> Add(Therapist therapist, AddPatientViewModel viewModel);
        Task<Patient> Edit(PatientViewModel userViewModel);
        Task Remove(string id);
        Task RefreshToken(string id);
        Task<Patient> GetById(string id);
        Task<Patient> GetByToken(string token);
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
        
        public async Task<Patient> Add(Therapist therapist, AddPatientViewModel viewModel)
        {
            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            if (viewModel.Nhc is null && viewModel.Zip is null)
            {
                throw new ArgumentException("Nhc or zip must be not null");
            }

            if (await _context.Patients.Where(u => u.Nhc == viewModel.Nhc).AnyAsync())
            {
                throw new ArgumentException("This patient already exists in the db");
            }

            var patient = new Patient
            {
                Therapist = therapist,
                Nhc = viewModel.Nhc,
                Zip = viewModel.Zip,
                Token = await _tokenService.GenerateToken(),
                FullName = System.IO.Path.GetRandomFileName() // TODO Get name from the hospital api service
            };

            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();

            return patient;
        }

        public async Task<List<Patient>> List(Therapist therapist)
        {
            return await _context
                .Patients
                .Where(p=> p.Therapist == therapist)
                .Include(p => p.Therapist)
                .ToListAsync();
        }

        public async Task<Patient> GetById(string id)
        {
            var patient = await _context
                .Patients
                .Include(p => p.Therapist)
                .Include(p => p.Videos)
                    .ThenInclude(v => v.Category)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            return patient;
        }

        public async Task<Patient> Edit(PatientViewModel patientViewModel)
        {
            var patient = await GetById(patientViewModel.Id);

            patient.FullName = patientViewModel.Name;
            patient.Zip = patientViewModel.Zip;
            patient.Nhc = patientViewModel.Nhc;

            await _context.SaveChangesAsync();

            return patient;
        }

        public async Task Remove(string id)
        {
            var patient = await GetById(id);
            
            _context.Patients.Remove(patient);
            
            await _context.SaveChangesAsync();
        }

        public async Task RefreshToken(string id)
        {
            var patient = await GetById(id);

            patient.Token = await _tokenService.GenerateToken();
            
            await _context.SaveChangesAsync();
        }

        public async Task<Patient> GetByToken(string token)
        {
            return await _context
                .Patients
                .Where(p => p.Token == token)
                .Include(p => p.Therapist)
                .FirstOrDefaultAsync();
        }
    }
}