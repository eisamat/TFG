using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Models;

namespace Server.Services
{
    public interface IVideoService
    {
        Task AssignVideo(string patientId, string videoId);
        Task UnassignVideo(string patientId, string videoId);
        
        Task<ICollection<Video>> GetVideos();
    }
    
    internal class VideoService: IVideoService
    {
        private readonly AppDbContext _dbContext;

        public VideoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task AssignVideo(string patientId, string videoId)
        {
            var video = _dbContext.Videos.FirstOrDefault(v => v.Id == Guid.Parse(videoId));
            var patient = _dbContext
                .Patients
                .Include(p => p.Videos)
                .FirstOrDefault(p => p.Id == Guid.Parse(patientId));

            if (video == null || patient == null)
            {
                throw new ArgumentException("Video or patient null");
            }
            else
            {
                patient.Videos.Add(video);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UnassignVideo(string patientId, string videoId)
        {
            var video = _dbContext.Videos.FirstOrDefault(v => v.Id == Guid.Parse(videoId));
            var patient = _dbContext
                .Patients
                .Include(p => p.Videos)
                .FirstOrDefault(p => p.Id == Guid.Parse(patientId));

            if (video == null || patient == null)
            {
                throw new ArgumentException("Video or patient null");
            }
            else
            {
                patient.Videos.Remove(video);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<ICollection<Video>> GetVideos()
        {
            return await _dbContext.Videos
                .Include(v => v.Category)
                .ToListAsync();
        }
    }
}