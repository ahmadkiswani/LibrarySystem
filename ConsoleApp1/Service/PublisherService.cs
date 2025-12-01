using LibrarySystem.DTOs;
using LibrarySystem.DTOs.PublisherDTOs;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LibrarySystem.Service
{
    public class PublisherService
    {
        private readonly LibraryContext _context;
        private List<Publisher> _publishers => _context.Publishers;
        private int _idCounter = 1;


        public PublisherService(LibraryContext context)
        {
            _context = context;
        }

        public void AddPublisher(PublisherCreateDto dto)
        {
            Publisher p = new Publisher();
            p.Id = _idCounter++;
            p.Name = dto.Name;
            p.CreatedBy = 1;
            p.CreatedDate = DateTime.Now;
            _publishers.Add(p);
        }

        public List<PublisherListDto> ListPublishers()
        {
            List<PublisherListDto> result = new List<PublisherListDto>(); 

            return _publishers
                .Where(p => !p.IsDeleted)
             .Select(p => new PublisherListDto
             {
                  Id = p.Id,
                  Name = p.Name
             }).ToList();
        }    
        public PublisherDetailsDto GetPublisherById(int id)
        {
            Publisher p = _publishers.FirstOrDefault(z => z.Id == id);

            if (p == null)
                return null;

            PublisherDetailsDto dto = new PublisherDetailsDto();
            dto.Id = p.Id;
            dto.Name = p.IsDeleted ? "Unknown" : p.Name;
            return dto;
        }
        public void EditPublisher(int id, PublisherUpdateDto dto)
        {
            Publisher existing = _publishers.FirstOrDefault(p => p.Id == id);

            if (existing != null)
            {
                existing.Name = dto.Name;
                existing.LastModifiedBy = 1;
                existing.LastModifiedDate = DateTime.Now;
            }
        }
        public void DeletePublisher(int id)
        {
            Publisher existing = _publishers.FirstOrDefault(p => p.Id == id);
            if (existing != null)
                if (existing == null)
                    throw new Exception("Publisher not found");
            existing.IsDeleted = true;
            existing.DeletedBy = id;
            existing.DeletedDate=DateTime.Now;

        }
        public List<PublisherListDto> Search(PublisherSearchDto dto)
        {
            int page = dto.Page <= 0 ? 1 : dto.Page;
            int pageSize = dto.PageSize <= 0 || dto.PageSize > 200 ? 10 : dto.PageSize;
            return _publishers
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
