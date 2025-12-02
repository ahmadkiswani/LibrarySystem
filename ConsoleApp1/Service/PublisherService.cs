using LibrarySystem.Data;
using LibrarySystem.DTOs;
using LibrarySystem.DTOs.PublisherDTOs;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class PublisherService
    {
        private readonly LibraryDbContext _context;

        public PublisherService(LibraryDbContext context)
        {
            _context = context;
        }

        public void AddPublisher(PublisherCreateDto dto)
        {
            var publisher = new Publisher
            {
                Name = dto.Name,
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };

            _context.Publishers.Add(publisher);
            _context.SaveChanges();
        }

        public List<PublisherListDto> ListPublishers()
        {
            return _context.Publishers
                .Where(p => !p.IsDeleted)
                .Select(p => new PublisherListDto
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();
        }

        public PublisherDetailsDto GetPublisherById(int id)
        {
            var publisher = _context.Publishers.FirstOrDefault(p => p.Id == id);

            if (publisher == null)
                return null;

            return new PublisherDetailsDto
            {
                Id = publisher.Id,
                Name = publisher.IsDeleted ? "Unknown" : publisher.Name
            };
        }

        public void EditPublisher(int id, PublisherUpdateDto dto)
        {
            var publisher = _context.Publishers.FirstOrDefault(p => p.Id == id && !p.IsDeleted);

            if (publisher == null)
                throw new Exception("Publisher not found");

            publisher.Name = dto.Name;
            publisher.LastModifiedBy = 1;
            publisher.LastModifiedDate = DateTime.Now;

            _context.SaveChanges();
        }

        public void DeletePublisher(int id)
        {
            var publisher = _context.Publishers.FirstOrDefault(p => p.Id == id);

            if (publisher == null)
                throw new Exception("Publisher not found");

            publisher.IsDeleted = true;
            publisher.DeletedBy = 1;
            publisher.DeletedDate = DateTime.Now;

            _context.SaveChanges();
        }

        public List<PublisherListDto> Search(PublisherSearchDto dto)
        {
            int page = dto.Page <= 0 ? 1 : dto.Page;
            int pageSize = dto.PageSize <= 0 || dto.PageSize > 200 ? 10 : dto.PageSize;

            return _context.Publishers
                .Where(p => !p.IsDeleted)
                .Where(p =>
                    (dto.Text == null || p.Name.ToLower().Contains(dto.Text.ToLower())) &&
                    (dto.Number == null || p.Id == dto.Number)
                )
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PublisherListDto
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();
        }
    }
}
