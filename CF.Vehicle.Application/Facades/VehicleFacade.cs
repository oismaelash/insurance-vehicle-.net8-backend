using AutoMapper;
using CF.Vehicle.Application.Dtos;
using CF.Vehicle.Application.Facades.Interfaces;
using CF.Vehicle.Domain.Services.Interfaces;

namespace CF.Vehicle.Application.Facades;

public class VehicleFacade(IVehicleService vehicleService, IMapper mapper) : IVehicleFacade
{
    public async Task<long> CreateAsync(VehicleRequestDto customerRequestDto, CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Domain.Entities.Vehicle>(customerRequestDto);

        var id = await vehicleService.CreateAsync(customer, cancellationToken);

        return id;
    }
}