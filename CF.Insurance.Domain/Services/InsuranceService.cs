using CF.Insurance.Domain.Repositories;
using CF.Insurance.Domain.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CF.Insurance.Domain.Services;

public class InsuranceService(IInsuranceRepository insuranceRepository)
    : IInsuranceService
{

    public async Task<long> CreateAsync(Entities.Insurance insurance, CancellationToken cancellationToken)
    {
        if (insurance is null)
            throw new ValidationException("Insurance is null.");

        insuranceRepository.Add(insurance);
        await insuranceRepository.SaveChangesAsync(cancellationToken);

        return insurance.Id;
    }
}