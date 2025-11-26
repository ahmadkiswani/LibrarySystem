using LibrarySystem.Models;
using LibrarySystem.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class PublisherService
    {
        private List<Publisher> _publishers;
        private int _idCounter = 1;

        public PublisherService(List<Publisher> publishers)
        {
            _publishers = publishers;
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

            foreach (var p in _publishers)
            {
                PublisherListDto dto = new PublisherListDto();
                dto.Id = p.Id;
                dto.Name = p.Name;

                result.Add(dto);
            }

            return result;
        }

        public PublisherDetailsDto GetPublisherById(int id)
        {
            Publisher p = _publishers.FirstOrDefault(z => z.Id == id);

            if (p == null)
                return null;

            PublisherDetailsDto dto = new PublisherDetailsDto();
            dto.Id = p.Id;
            dto.Name = p.Name;

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
            {
                _publishers.Remove(existing);
            }
        }
    }
}
