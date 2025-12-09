using LibrarySystem.Domain.Repositories;
using LibrarySystem.Entities.Models;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Services
{
    public class PublisherService : IPublisherService   
    {
        private readonly IGenericRepository<Publisher> _publisherRepo;

        public PublisherService(IGenericRepository<Publisher> publisherRepo)
        {
            _publisherRepo = publisherRepo;
        }

        public async Task AddPublisher(PublisherCreateDto dto)
        {
            bool exists = await _publisherRepo.GetQueryable()
                .AnyAsync(p => p.Name == dto.Name);

            if (exists)
                throw new Exception("Publisher already exists");

            var publisher = new Publisher
            {
                Name = dto.Name
            };

            await _publisherRepo.AddAsync(publisher);
            await _publisherRepo.SaveAsync();
        }

        public async Task EditPublisher(int id, PublisherUpdateDto dto)
        {
            var publisher = await _publisherRepo.GetByIdAsync(id);
            if (publisher == null)
                throw new Exception("Publisher not found");

            publisher.Name = dto.Name;

            await _publisherRepo.UpdateAsync(publisher);
            await _publisherRepo.SaveAsync();
        }

        public async Task DeletePublisher(int id)
        {
            var publisher = await _publisherRepo.GetByIdAsync(id);
            if (publisher == null)
                throw new Exception("Publisher not found");

            await _publisherRepo.SoftDeleteAsync(publisher);
            await _publisherRepo.SaveAsync();
        }

        public async Task<List<PublisherListDto>> ListPublishers()
        {
            var publishers = await _publisherRepo.GetAllAsync();

            return publishers.Select(p => new PublisherListDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }

    
    }
}
