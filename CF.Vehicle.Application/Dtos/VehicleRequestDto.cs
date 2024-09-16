using System.ComponentModel.DataAnnotations;

namespace CF.Vehicle.Application.Dtos;

public record VehicleRequestDto
{
    [Required(ErrorMessage = "The Id field is required.")]
    [Display(Name = "Vehicle Id")]
    public string Id { get; set; }
}