using System.Net;
using CF.Vehicle.Application.Dtos;
using CF.Vehicle.Application.Facades.Interfaces;
using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CF.Api.Controllers;

[ApiController]
[Route("api/v1/vehicle")]
public class VehicleController(
    ICorrelationContextAccessor correlationContext,
    ILogger<VehicleController> logger,
    IVehicleFacade vehicleFacade) : ControllerBase
{
    [HttpPost]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid Request.")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Vehicle has been created successfully.")]
    public async Task<IActionResult> Post([FromBody] VehicleRequestDto vehicleRequestDto,
    CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var id = await vehicleFacade.CreateAsync(vehicleRequestDto, cancellationToken);

        return Created();
    }
}