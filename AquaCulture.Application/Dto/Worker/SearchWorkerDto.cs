using AquaCulture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCulture.Application.Dto.Worker
{
    public class SearchWorkerDto
    {
        public string? SearchTerm { get; set; }
        public CrewRole? Position { get; set; }
        public string? SortBy { get; set; }
        public bool? IsAssigned { get; set; }
    }
}
