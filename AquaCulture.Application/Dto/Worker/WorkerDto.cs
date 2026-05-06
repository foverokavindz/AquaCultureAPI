using AquaCulture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCulture.Application.Dto.Worker
{
    public class WorkerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public CrewRole Position { get; set; }
        public DateOnly CertifiedUntil { get; set; }
        public Guid FishFarmId { get; set; }
    }
}
