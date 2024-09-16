using CF.Insurance.Application.Dtos;

namespace CF.Insurance.Application.Facades.Interfaces;

public interface IInsuranceFacade
{
    Task<long> CreateAsync(InsuranceRequestDto customerRequestDto, CancellationToken cancellationToken);
}