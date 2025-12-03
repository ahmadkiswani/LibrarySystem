using LibrarySystem.DTOs;
using LibrarySystem.DTOs.PublisherDTOs;
using LibrarySystem.Models;
using LibrarySystem.Repository;

namespace LibrarySystem.Service
{
    public class PublisherService
    {
        private readonly IGenericRepository<Publisher> _publisherRepo;

        public PublisherService(IGenericRepository<Publisher> publisherRepo)
        {
            _publisherRepo = publisherRepo;
        }

        public async Task AddPublisher(PublisherCreateDto dto)
        {
            var publisher = new Publisher
            {
                Name = dto.Name,
                CreatedBy = 0,
                CreatedDate = DateTime.Now
            };

            await _publisherRepo.AddAsync(publisher);
            await _publisherRepo.SaveAsync();
        }

        public async Task EditPublisher(int id, PublisherUpdateDto dto)
        {
            var publisher = await _publisherRepo.GetByIdAsync(id);
            if (publisher == null || publisher.IsDeleted)
                throw new Exception("Publisher not found");

            publisher.Name = dto.Name;
            publisher.LastModifiedBy = 1;
            publisher.LastModifiedDate = DateTime.Now;

            await _publisherRepo.Update(publisher);
            await _publisherRepo.SaveAsync();
        }

        public async Task DeletePublisher(int id)
        {
            var publisher = await _publisherRepo.GetByIdAsync(id);
            if (publisher == null)
                throw new Exception("Publisher not found");

            publisher.IsDeleted = true;
            publisher.DeletedBy = 1;
            publisher.DeletedDate = DateTime.Now;

            await _publisherRepo.Update(publisher);
            await _publisherRepo.SaveAsync();
        }

        public async Task<List<PublisherListDto>> ListPublishers()
        {
            var publishers = await _publisherRepo.FindAsync(p => !p.IsDeleted);
            return publishers.Select(p => new PublisherListDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }
    }
}
