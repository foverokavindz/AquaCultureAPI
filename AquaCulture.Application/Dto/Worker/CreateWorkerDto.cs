using AquaCulture.Domain.Entities;
using System.ComponentModel.DataAnnotations;


namespace AquaCulture.Application.Dto.Worker
{
    public class CreateWorkerDto
    {
        [Required(ErrorMessage = "Worker name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        public string? ProfileImageUrl { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(18, 100, ErrorMessage = "Age must be between 16 and 100")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
        public string Email { get; set; } = string.Empty;

        [EnumDataType(typeof(CrewRole), ErrorMessage = "Invalid position value")]
        public CrewRole? Position { get; set; }

        [Required(ErrorMessage = "Certified until date is required")]
        public DateOnly CertifiedUntil { get; set; }

        public Guid? FishFarmId { get; set; }
    }
}
