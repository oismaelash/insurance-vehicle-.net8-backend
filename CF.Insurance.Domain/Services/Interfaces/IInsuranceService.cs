namespace CF.Insurance.Domain.Services.Interfaces;

public interface IInsuranceService
{
    Task<long> CreateAsync(Entities.Insurance customer, CancellationToken cancellationToken);
}