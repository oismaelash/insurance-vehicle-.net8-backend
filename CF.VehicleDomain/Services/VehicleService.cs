using CF.Vehicle.Domain.Repositories;
using CF.Vehicle.Domain.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CF.Vehicle.Domain.Services;

public class VehicleService(IVehicleRepository vehicleRepository)
    : IVehicleService
{

    public async Task<long> CreateAsync(Entities.Vehicle vehicle, CancellationToken cancellationToken)
    {
        if (vehicle is null)
            throw new ValidationException("Vehicle is null.");

        vehicleRepository.Add(vehicle);
        await vehicleRepository.SaveChangesAsync(cancellationToken);

        return vehicle.Id;
    }
}