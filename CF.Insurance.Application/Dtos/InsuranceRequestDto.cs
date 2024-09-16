using System.ComponentModel.DataAnnotations;

namespace CF.Insurance.Application.Dtos;

public record InsuranceRequestDto
{
    [Required(ErrorMessage = "The Vehicle field is required.")]
    [Display(Name = "Vehicle Id")]
    public string VehicleId{ get; set; }
}