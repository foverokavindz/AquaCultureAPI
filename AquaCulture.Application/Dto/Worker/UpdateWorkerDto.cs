using AquaCulture.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace AquaCulture.Application.Dto.Worker
{
    public class UpdateWorkerDto
    {
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string? Name { get; set; }

        public string? ProfileImageUrl { get; set; }

        [Range(16, 100, ErrorMessage = "Age must be between 16 and 100")]
        public int? Age { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
        public string? Email { get; set; }

        [EnumDataType(typeof(CrewRole), ErrorMessage = "Invalid position value")]
        public CrewRole? Position { get; set; }

        public DateOnly? CertifiedUntil { get; set; }

        public Guid? FishFarmId { get; set; }
    }
}