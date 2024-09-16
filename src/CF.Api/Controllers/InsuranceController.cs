using System.Net;
using CF.Api.Helpers;
using CF.Insurance.Application.Dtos;
using CF.Insurance.Application.Facades.Interfaces;
using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CF.Api.Controllers;

[ApiController]
[Route("api/v1/insurance")]
public class InsuranceController(
    ICorrelationContextAccessor correlationContext,
    ILogger<InsuranceController> logger,
    IInsuranceFacade insuranceFacade) : ControllerBase
{
    [HttpPost]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid Request.")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Insurance has been created successfully.")]
    public async Task<IActionResult> Post([FromBody] InsuranceRequestDto insuranceRequestDto,
    CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var id = await insuranceFacade.CreateAsync(insuranceRequestDto, cancellationToken);

        return Created();
    }
}