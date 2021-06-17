using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Models;
using Server.ViewModels;

namespace Server.Services
{
    public interface IVideoService
    {
        Task<ICollection<Category>> GetVideosToAssign();
        Task<ICollection<Video>> GetAssignedVideos(Patient patient);
        Task<ICollection<PreviouslyAssignedVideosDto>> GetPreviouslyAssignedVideos(string patientId);
        Task<CategoryDto> GetCategory(string id);
        Task AssignVideo(AssignVideoViewModel model);
        Task UnassignVideo(string patientId, string videoId);
        Task AddNewCategory(string categoryName);
        Task AddNewVideo(AddVideoViewModel viewModel);
        Task EditCategory(EditCategoryModel viewModel);
        Task DeleteCategory(string categoryId);
        Task<VideoDto> GetVideo(string id);
        Task EditVideo(VideoDto model);
        Task DeleteVideo(Guid modelId);
    }

    internal class VideoService : IVideoService
    {
        private readonly AppDbContext _dbContext;

        public VideoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AssignVideo(AssignVideoViewModel model)
        {
            var patient = await GetPatient(model.Id);

            var videos = _dbContext.Videos.Where(v => model.Videos.Contains(v.Id.ToString()))
                .ToList();

            patient.Videos.Clear();

            if (videos.Count == 0 || patient == null)
            {
                throw new ArgumentException("Video or patient null");
            }

            var assignmentRecordId = Guid.NewGuid();
            var assignmentDate = DateTime.Now;

            foreach (var v in videos)
            {
                patient.Videos.Add(v);

                await _dbContext.AssignmentRecords.AddAsync(new AssignmentRecord
                {
                    Date = assignmentDate,
                    AssignmentId = assignmentRecordId,
                    Video = v,
                    Patient = patient
                });
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task UnassignVideo(string patientId, string videoId)
        {
            var video = _dbContext.Videos.FirstOrDefault(v => v.Id == Guid.Parse(videoId));
            var patient = await GetPatient(patientId);

            if (video == null || patient == null)
            {
                throw new ArgumentException("Video or patient null");
            }

            patient.Videos.Remove(video);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Category>> GetVideosToAssign()
        {
            return await _dbContext.Categories
                .Include(c => c.Videos)
                .ThenInclude(v => v.Category)
                .ToListAsync();
        }

        public async Task<ICollection<Video>> GetAssignedVideos(Patient patient)
        {
            var assignedPatient = await GetPatient(patient.Id.ToString());
            return assignedPatient != null ? assignedPatient.Videos : ArraySegment<Video>.Empty;
        }

        public async Task<ICollection<PreviouslyAssignedVideosDto>> GetPreviouslyAssignedVideos(string patientId)
        {
            var assignedPatient = await GetPatient(patientId);

            var recordGroups = _dbContext.AssignmentRecords
                .Include(p => p.Video)
                .ThenInclude(v => v.Category)
                .Where(a => a.Patient == assignedPatient)
                .OrderByDescending(a => a.Date)
                .AsEnumerable()
                .GroupBy(a => a.AssignmentId)
                .Skip(1)
                .Take(10);

            var recordDto = new List<PreviouslyAssignedVideosDto>();

            foreach (var recordGroup in recordGroups)
            {
                var assignmentRecords = recordGroup.ToList();

                var dto = new PreviouslyAssignedVideosDto()
                {
                    Id = recordGroup.Key,
                    Date = assignmentRecords.First().Date,
                    Videos = assignmentRecords.Adapt<ICollection<VideoDto>>()
                };

                recordDto.Add(dto);
            }

            return recordDto;
        }

        public async Task AddNewCategory(string categoryName)
        {
            _dbContext.Categories.Add(new Category
            {
                Name = categoryName
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddNewVideo(AddVideoViewModel viewModel)
        {
            var category = await _dbContext.Categories
                .Where(c => c.Id == Guid.Parse(viewModel.CategoryId))
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new ArgumentException("Category does not exists");
            }

            await _dbContext.Videos.AddAsync(new Video
            {
                Category = category,
                Name = viewModel.Name,
                YoutubeId = viewModel.YoutubeId
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task EditCategory(EditCategoryModel viewModel)
        {
            var category = await _dbContext.Categories
                .Where(c => c.Id == Guid.Parse(viewModel.Id))
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new ArgumentException("Category does not exists");
            }

            category.Name = viewModel.Name;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategory(string categoryId)
        {
            var category = await _dbContext.Categories
                .Where(c => c.Id == Guid.Parse(categoryId))
                .Include(c => c.Videos)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new ArgumentException("Category does not exists");
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<VideoDto> GetVideo(string id)
        {
            var video = await _dbContext.Videos
                .Where(v => v.Id == Guid.Parse(id))
                .Include(v => v.Category)
                .FirstOrDefaultAsync();

            if (video == null)
            {
                throw new ArgumentException("Video is null");
            }

            return new VideoDto
            {
                Id = video.Id,
                Name = video.Name,
                CategoryId = video.Category.Id.ToString(),
                CategoryName = video.Category.Name,
                YoutubeId = video.YoutubeId
            };
        }

        public async Task EditVideo(VideoDto model)
        {
            var video = await _dbContext.Videos
                .Where(v => v.Id == model.Id)
                .Include(v => v.Category)
                .FirstOrDefaultAsync();
            
            if (video == null)
            {
                throw new ArgumentException("Video is null");
            }

            video.Name = model.Name;    
            video.YoutubeId = model.YoutubeId;

            if (video.Category.Id.ToString() != model.CategoryId)
            {
                // Change the category of this video
                var category = await _dbContext.Categories
                    .Where(c => c.Id == Guid.Parse(model.CategoryId))
                    .FirstOrDefaultAsync();

                video.Category = category;
            }

            video.YoutubeId = model.YoutubeId;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteVideo(Guid modelId)
        {
            var video = await _dbContext.Videos
                .Where(v => v.Id == modelId)
                .Include(v => v.Category)
                .FirstOrDefaultAsync();

            _dbContext.Videos.Remove(video);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CategoryDto> GetCategory(string categoryId)
        {
            var category = await _dbContext.Categories
                .Where(c => c.Id == Guid.Parse(categoryId))
                .Include(c => c.Videos)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new ArgumentException("Category is null");
            }

            return new CategoryDto
            {
                Id = categoryId,
                Name = category.Name
            };
        }

        private async Task<Patient> GetPatient(string patientId)
        {
            return await _dbContext
                .Patients
                .Include(p => p.Videos)
                .ThenInclude(v => v.Category)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(patientId));
        }
    }
}