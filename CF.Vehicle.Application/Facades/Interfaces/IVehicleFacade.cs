using CF.Vehicle.Application.Dtos;

namespace CF.Vehicle.Application.Facades.Interfaces;

public interface IVehicleFacade
{
    Task<long> CreateAsync(VehicleRequestDto customerRequestDto, CancellationToken cancellationToken);
}