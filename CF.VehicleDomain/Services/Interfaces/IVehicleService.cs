namespace CF.Vehicle.Domain.Services.Interfaces;

public interface IVehicleService
{
    Task<long> CreateAsync(Entities.Vehicle customer, CancellationToken cancellationToken);
}